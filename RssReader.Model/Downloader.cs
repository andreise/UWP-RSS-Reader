using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using static System.FormattableString;

namespace RssReader.Model
{

    static class Downloader
    {

        private static string GenerateUniqueFileName() => Path.GetRandomFileName();

        private static async Task<IStorageFile> CreateTempFileAsync() => await ApplicationData.Current.TemporaryFolder.CreateFileAsync(
            GenerateUniqueFileName(),
            CreationCollisionOption.GenerateUniqueName
        );

        private static async Task<DownloadOperation> DownloadFileHelperAsync(Uri uri) =>
            await new BackgroundDownloader().
            CreateDownload(uri, await CreateTempFileAsync()).
            StartAsync();

        public static async Task<IStorageFile> DownloadFileAsync(Uri uri)
        {
            var operation = await DownloadFileHelperAsync(uri);

            if (operation.Progress.Status != BackgroundTransferStatus.Completed)
            {
                string message = Invariant(
                    $"File downloading is not completed successfully (current status: {operation.Progress.Status})."
                );
                throw new InvalidOperationException(message);
            }

            return operation.ResultFile;
        }

    }

}
