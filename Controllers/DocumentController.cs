using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PhishingDetection.Controllers
{
    public class DocumentController : Controller
    {
        // Kullanıcı arayüzden metin gönderdiğinde çalışacak yönlendirici metot
        [HttpPost]
        public IActionResult AnalyzeText(string text)
        {
            // İleride bu metni yapay zeka servisimize (Gemini) gönderip analiz sonucunu ekrana basacağız
            ViewBag.Result = "Metin başarıyla alındı ve analiz ediliyor...";
            return View("Index");
        }
    }
}