using System.ComponentModel;

namespace AzureBlobProject.Services
{
    public interface IContainerService
    {
        Task<List<string>> GetAllContainerAndBlobs();
        Task<List<string>> GetAllContainer();
        Task CreateContainer(string containerName, string AccessLevel);
        Task DeleteContainer(string containerName);
    }
}
