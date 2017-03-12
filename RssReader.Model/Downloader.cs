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

        public static IStorageFile DownloadFileFromUri(Uri uri)
        {
            var operationWithProgressTask = Task.Run(
                async () =>
                    new BackgroundDownloader().
                    CreateDownload(uri, await CreateTempFileAsync()).
                    StartAsync()
            );
            var operationWithProgressResultTask = operationWithProgressTask.Result.AsTask();
            operationWithProgressResultTask.Wait();

            if (operationWithProgressResultTask.Result.Progress.Status != BackgroundTransferStatus.Completed)
            {
                string message = Invariant(
                    $"File download not completed successfully (current status: {operationWithProgressResultTask.Result.Progress.Status})."
                );
                throw new InvalidOperationException(message);
            }

            return operationWithProgressResultTask.Result.ResultFile;
        }

    }
}
