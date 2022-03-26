using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace Elogy.ApiPriorityAppDmz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrepPdfController : ControllerBase
    {

        private IConfiguration Configuration { get; }
        protected readonly ILogger<PrepPdfController> _logger;
        public PrepPdfController(IConfiguration configuration, ILogger<PrepPdfController> logger)
        {
            Configuration = configuration;
        }


        [HttpGet]
        public async Task<ActionResult> DownloadFile(string qrCode)
        {
            try
            {

                qrCode = "OI_EKB9471_101_21_en_GB.pdf";
                string tmpPath = Configuration["Priority_Dir:BaseDir"] + qrCode;

                var bytes = await System.IO.File.ReadAllBytesAsync(tmpPath);
                return File(bytes, "application/pdf", Path.GetFileName(tmpPath));
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message);
                throw;
            }
        }


    }
}
