using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace S_EDex365.Controllers
{
    public class APKController : Controller
    {
        [HttpGet]
        public IActionResult DownloadApk()
        {
            // Path to your APK file on the server
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "app-arm64-v8a-release.apk");

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("The requested APK file does not exist.");
            }

            // Serve the file as a download
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = "app-arm64-v8a-release.apk";

            return File(fileBytes, "application/vnd.android.package-archive", fileName);
        }
    }
}
