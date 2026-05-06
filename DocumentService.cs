using System;
using System.Threading.Tasks;

public class DocumentService
{
    private readonly IRepository<Document> _documentRepository;
    private IDocumentAnalysisStrategy _analysisStrategy;

    // Dependency Injection ile IRepository içeri alınıyor
    public DocumentService(IRepository<Document> documentRepository)
    {
        _documentRepository = documentRepository;
    }

    // Çalışma zamanında (Runtime) stratejiyi değiştirmemizi sağlayan metod
    public void SetAnalysisStrategy(IDocumentAnalysisStrategy strategy)
    {
        _analysisStrategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public async Task AnalyzeAndUpdateDocumentAsync(int documentId)
    {
        if (_analysisStrategy == null)
            throw new InvalidOperationException("Lütfen analiz işleminden önce bir strateji belirleyin.");

        // 1. Veri Erişim: Dokümanı veritabanından çek
        var document = await _documentRepository.GetByIdAsync(documentId);
        if (document == null)
            throw new Exception("Doküman bulunamadı.");

        // 2. İş Mantığı: Seçilen stratejiye göre metni analiz et
        document.AnalysisResult = _analysisStrategy.Analyze(document.Content);

        // 3. Veri Erişim: Güncellenmiş dokümanı veritabanına kaydet
        await _documentRepository.UpdateAsync(document);
    }
}