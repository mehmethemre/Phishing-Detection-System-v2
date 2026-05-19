using System.Threading.Tasks;
using PhishingDetection.Strategies; // Stratejilerin olduğu klasöre göre düzenle

namespace PhishingDetection.Services
{
    public class DocumentService
    {
        private readonly IRepository<Document> _repository;
        private IDocumentAnalysisStrategy _analysisStrategy;

        public DocumentService(IRepository<Document> repository)
        {
            _repository = repository;
        }

        // Strategy Pattern kullanımı [2]
        public void SetAnalysisStrategy(IDocumentAnalysisStrategy strategy)
        {
            _analysisStrategy = strategy;
        }

        // Asenkron metod yapısı [1]
        public async Task AnalyzeAndUpdateDocumentAsync(int id)
        {
            var doc = await _repository.GetByIdAsync(id);
            if (doc != null && _analysisStrategy != null)
            {
                // Stratejiyi asenkron olarak çağırıp sonucunu bekliyoruz
                doc.AnalysisResult = await _analysisStrategy.AnalyzeAsync(doc.Content);
                await _repository.UpdateAsync(doc);
            }
        }
    }
}