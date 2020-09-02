using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WallPaperApp.Dto;
using WallPaperApp.Infrastructure;
using WallPaperApp.Entity;
using WallPaperApp.Utility;

namespace WallPaperApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        private readonly ILogger<UploadController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UploadController(IWebHostEnvironment environment, ILogger<UploadController> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _environment = environment;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public class FileUploadApi
        {
            public IFormFile files { get; set; }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] FileUploadApi objFile)
        {
            try
            {
                if (objFile.files.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "/Uploads/"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "/Uploads/");
                    }

                    var host = "http://" + _httpContextAccessor.HttpContext.Request.Host.Value;

                    await using var fileStream =
                        System.IO.File.Create(_environment.WebRootPath + "/Uploads/" + objFile.files.FileName);
                    await objFile.files.CopyToAsync(fileStream);
                    fileStream.Flush();
                    return Ok(new UploadResult {path = host + StaticFiles.path + "/Uploads/" + objFile.files.FileName});
                }
                else
                {
                    return StatusCode(HttpConstants.NotExtended, new ErrorModel {message = Messages.DefaultMessage});
                }
            }
            catch (Exception ex)
            {
                return StatusCode(HttpConstants.NotExtended, ex);
            }
        }
    }
}