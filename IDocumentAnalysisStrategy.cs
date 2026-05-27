using System.Threading.Tasks;

namespace phishing
{
    public interface IDocumentAnalysisStrategy
    {
        Task<string> AnalyzeAsync(string content);
    }
}