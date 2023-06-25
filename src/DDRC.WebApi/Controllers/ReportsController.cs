using DDRC.WebApi.Reports;
using Microsoft.AspNetCore.Mvc;

namespace DDRC.WebApi.Controllers
{
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IVideoStoreReport _report;

        public ReportsController(IVideoStoreReport report)
        {
            _report = report;
        }

        [HttpGet("api/reports:video-store/{videoStoreName}")]
        public IActionResult Report(string videoStoreName)
        {
            var result = _report.Generate();
            var filtered = result?.VideoStores.Where(x => x.VideoStore == videoStoreName);
            return Ok(filtered);
        }
    }
}
