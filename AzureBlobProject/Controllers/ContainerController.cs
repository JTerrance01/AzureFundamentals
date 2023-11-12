using AzureBlobProject.Models;
using AzureBlobProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobProject.Controllers
{
    public class ContainerController : Controller
    {
        private readonly IContainerService _containerService;
        public ContainerController(IContainerService containerService)
        {
            _containerService = containerService;
        }

        public async Task<IActionResult> Index()
        {
            var allContainer = await _containerService.GetAllContainer();
            return View(allContainer);
        }

        public async Task<IActionResult> Delete(string containerName)
        {
            await _containerService.DeleteContainer(containerName);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View(new Container());
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Name, string AccessLevel)
        {
            await _containerService.CreateContainer(Name.ToLower().Replace(" ",""), AccessLevel);
            return RedirectToAction(nameof(Index));
        }
    }
}
