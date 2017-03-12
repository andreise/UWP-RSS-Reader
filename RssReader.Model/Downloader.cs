using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using static System.FormattableString;

namespace RssReader.Model
{

    static class Downloader
    {

        public static async Task<IStorageFile> CreateTempFileAsync() => await ApplicationData.Current.TemporaryFolder.CreateFileAsync(
            Path.GetRandomFileName(),
            CreationCollisionOption.GenerateUniqueName
        );

        public static IAsyncOperationWithProgress<DownloadOperation, DownloadOperation> StartDownloadFileAsync(Uri uri, IStorageFile resultFile) =>
            new BackgroundDownloader().CreateDownload(uri, resultFile).StartAsync();

        public static async Task<DownloadOperation> DownloadFileHelperAsync(Uri uri, IStorageFile resultFile) =>
            await StartDownloadFileAsync(uri, resultFile);

        public static async Task<DownloadOperation> DownloadFileHelperAsync(Uri uri) =>
            await DownloadFileHelperAsync(uri, await CreateTempFileAsync());

        public static async Task<IStorageFile> DownloadFileAsync(Uri uri, IStorageFile resultFile)
        {
            var operation = await DownloadFileHelperAsync(uri, resultFile);

            if (operation.Progress.Status != BackgroundTransferStatus.Completed)
            {
                string message = Invariant(
                    $"File downloading is not completed successfully (current status: {operation.Progress.Status})."
                );
                throw new InvalidOperationException(message);
            }

            return operation.ResultFile;
        }

        public static async Task<IStorageFile> DownloadFileAsync(Uri uri) =>
            await DownloadFileAsync(uri, await CreateTempFileAsync());

    }

}
