using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PhishingDetection.Controllers
{
    public class DocumentController : Controller
    {
        // 1. Tarayıcıdan adrese girildiğinde sayfanın İLK AÇILIŞINI sağlayan metot
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // 2. Senin yazdığın: Kullanıcı arayüzden metin gönderdiğinde çalışacak metot
        [HttpPost]
        public IActionResult AnalyzeText(string text)
        {
            // İleride bu metni yapay zeka servisimize (Gemini) gönderip analiz sonucunu ekrana basacağız
            ViewBag.Result = "Metin başarıyla alındı ve analiz ediliyor...";
            return View("Index");
        }
    }
}