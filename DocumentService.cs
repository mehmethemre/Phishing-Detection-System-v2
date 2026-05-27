using System.Threading.Tasks;

namespace phishing
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentAnalysisStrategy _analysisStrategy;

        public DocumentService(IDocumentAnalysisStrategy analysisStrategy)
        {
            _analysisStrategy = analysisStrategy;
        }

        public async Task<string> AnalyzeTextAsync(string text)
        {
            return await _analysisStrategy.AnalyzeAsync(text);
        }
    }
}