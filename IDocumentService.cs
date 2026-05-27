using System.Threading.Tasks;

namespace phishing
{
    public interface IDocumentService
    {
        Task<string> AnalyzeTextAsync(string text);
    }
}