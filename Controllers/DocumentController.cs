using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace phishing.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AnalyzeText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                ViewBag.Result = "// Lütfen analiz için geçerli bir metin girin.";
                return View("Index");
            }

            var result = await _documentService.AnalyzeTextAsync(text);
            ViewBag.Result = result;
            return View("Index");
        }
    }
}