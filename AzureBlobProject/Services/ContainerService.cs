using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.ComponentModel;
using System.Drawing;

namespace AzureBlobProject.Services
{
    public class ContainerService: IContainerService
    {
        private readonly BlobServiceClient _blobClient;

        public ContainerService(BlobServiceClient blobClient)
        {
            _blobClient = blobClient;       
        }

        public async Task CreateContainer(string containerName, string AccessLevel)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            
            if (AccessLevel != null)
            {
                switch (AccessLevel)
                {
                    case "None":
                        await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.None);
                        break;
                    case "Blob":
                        await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
                        break;
                    case "BlobContainer":
                        await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
                        break;                    
                    default:
                        await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
                        break;
                }                
            }
            else
            {
                await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
            }
            

        }

        public async Task DeleteContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            await blobContainerClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllContainer()
        {
            List<string> containerName = new();

            await foreach (BlobContainerItem blobkContainerItem in _blobClient.GetBlobContainersAsync())
            {
                containerName.Add(blobkContainerItem.Name);
            }

            return containerName;
        }

        public async Task<List<string>> GetAllContainerAndBlobs()
        {
            List<string> containerAndBlobNames = new();
            containerAndBlobNames.Add("Account Name : " + _blobClient.AccountName);
            containerAndBlobNames.Add("------------------------------------------------------------------------------------------------------------");
            await foreach (BlobContainerItem blobContainerItem in _blobClient.GetBlobContainersAsync())
            {
                containerAndBlobNames.Add("--" + blobContainerItem.Name);
                BlobContainerClient _blobContainer =
                      _blobClient.GetBlobContainerClient(blobContainerItem.Name);
                await foreach (BlobItem blobItem in _blobContainer.GetBlobsAsync())
                {
                    //get metadata
                    var blobClient = _blobContainer.GetBlobClient(blobItem.Name);
                    BlobProperties blobProperties = await blobClient.GetPropertiesAsync();
                    string blobToAdd = blobItem.Name;
                    if (blobProperties.Metadata.ContainsKey("title"))
                    {
                        blobToAdd += "(" + blobProperties.Metadata["title"] + ")";
                    }
                    if (blobProperties.Metadata.ContainsKey("comment"))
                    {
                        blobToAdd += "(" + blobProperties.Metadata["comment"] + ")";
                    }

                    containerAndBlobNames.Add("------" + blobToAdd);
                }
                containerAndBlobNames.Add("------------------------------------------------------------------------------------------------------------");

            }
            return containerAndBlobNames;
        }
    }
}
