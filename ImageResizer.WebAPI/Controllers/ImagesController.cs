using ImageResizer.WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageResizer.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IResizeImageFileService _resizeImageFileService;

        public ImagesController(IResizeImageFileService resizeImageFileService)
        {
            _resizeImageFileService = resizeImageFileService;
        }

        [HttpPost]
        public async Task<IActionResult> ResizeImageFile(string[] urls)
        {
            foreach (var url in urls)
            {
               _resizeImageFileService.ResizeImageFile(url);
            }
            return Ok();
        }
    }
}
