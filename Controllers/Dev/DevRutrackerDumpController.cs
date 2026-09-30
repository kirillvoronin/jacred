using System;
using System.Globalization;
using JacRed.Application.Dev;
using Microsoft.AspNetCore.Mvc;

namespace JacRed.Controllers.Dev
{
    [Route("/dev/[action]")]
    public class DevRutrackerDumpController : Controller
    {
        readonly RutrackerDumpImportService _service;

        public DevRutrackerDumpController(RutrackerDumpImportService service)
        {
            _service = service;
        }

        public ActionResult ImportRutrackerDump(string path, string dumpDate)
        {
            if (!DateTime.TryParseExact(dumpDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out DateTime date))
                return BadRequest("dumpDate required: yyyy-MM-dd");

            return Content(_service.Start(path, date));
        }

        public JsonResult ImportRutrackerDumpStatus() => Json(_service.Status());
    }
}
