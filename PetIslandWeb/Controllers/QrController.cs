using Microsoft.AspNetCore.Mvc;
using PetIsland.Utility;
using PetIsland.Utility.Crypto;
using System.Text;
using System.Threading.Tasks;

namespace PetIslandWeb.Controllers
{
    public class QrController : Controller
    {
        [HttpGet]
        public IActionResult QrCodeScanner()
        {
            return View();
        }

        [HttpPost]
        public IActionResult QrCodeScanner(IFormFile file, string key)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Result = "No file selected.";
                return View();
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                ViewBag.Result = "Public key is required.";
                return View();
            }

            using var stream = file.OpenReadStream();
            var qrText = QrCode.ReadQrCode(stream);

            if (string.IsNullOrEmpty(qrText))
            {
                ViewBag.Result = "QR code not detected.";
                return View();
            }

            var parts = qrText.Split('.');
            if (parts.Length != 2)
            {
                ViewBag.Result = "Invalid QR format.";
                return View();
            }

            string base64Data = parts[0];
            string base64Signature = parts[1];

            string data = Encoding.UTF8.GetString(Convert.FromBase64String(base64Data));

            bool valid = EdDSA.Verify(base64Data, base64Signature, key);

            ViewBag.Result = valid
                ? $"✅ Signature VALID\n\nPayload:\n{data}"
                : "❌ Signature INVALID";

            return View();
        }

    }
}
