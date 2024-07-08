using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Application.Contracts.Infrastructure;
using Application.Models;

namespace Infrastructure
{
    public class StorageService : IStorageService
    {
        private readonly CloudBlobClient blobClient;
        private readonly AzureStorage _azureStorage;
        private readonly JwtSettings _jwtSettings;

        public StorageService(AzureStorage azureStorage, IOptions<JwtSettings> jwtSettings)
        {
            _azureStorage = azureStorage;
            _jwtSettings = jwtSettings.Value;
            var creds = new StorageCredentials(_azureStorage.StorageAccountName, _azureStorage.ConnectionString);
            var account = new CloudStorageAccount(creds, true);

            blobClient = account.CreateCloudBlobClient();
        }

        public async Task<string> GetBlobSasToken(string containerName)
        {
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();

            var sasConstraints = new SharedAccessBlobPolicy
            {
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Delete | SharedAccessBlobPermissions.Read
            };

            var sasBlobToken = container.GetSharedAccessSignature(sasConstraints);
            return sasBlobToken;
        }

        public async Task<Stream> GetFileAsync(string containerName, string fileName)
        {
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();

            CloudBlockBlob blob = container.GetBlockBlobReference(fileName);
            return await blob.OpenReadAsync();
        }

        public async Task<Storage> UploadFile(string containerName, IFormFile formFile)
        {
            var result = new Storage { FileName = formFile.FileName };

            try
            {
                using var fileStream = formFile.OpenReadStream();
                var length = (int)fileStream.Length;
                var file = new byte[length];

                await fileStream.ReadAsync(file, 0, length);
                string blobName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);

                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                await container.CreateIfNotExistsAsync();
                CloudBlockBlob blob = container.GetBlockBlobReference(formFile.FileName);

                fileStream.Position = 0;
                await blob.UploadFromStreamAsync(fileStream);

                result.Succeeded = true;
                result.Url = $"https://{blob.Uri.Host}/{containerName}/{blob.Name}";
                result.Hash = blob.Name;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Succeeded = false;
                throw;
            }

            return result;
        }
    }
}
