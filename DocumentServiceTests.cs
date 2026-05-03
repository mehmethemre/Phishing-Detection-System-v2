using System.Threading.Tasks;
using Moq;
using Xunit;

public class DocumentServiceTests
{
    [Fact]
    public async Task AnalyzeAndUpdateDocumentAsync_ShouldUpdateDocument_WhenStrategyIsSet()
    {
        // Arrange (Hazırlık)
        int documentId = 1;
        var fakeDocument = new Document 
        { 
            Id = documentId, 
            Content = "Lütfen acil olarak şifrenizi güncelleyin." 
        };

        // IRepository mock'lanıyor (Sahte veritabanı davranışı)
        var mockRepository = new Mock<IRepository<Document>>();
        mockRepository.Setup(repo => repo.GetByIdAsync(documentId))
                      .ReturnsAsync(fakeDocument);

        var documentService = new DocumentService(mockRepository.Object);
        
        // Strateji olarak Duygu Analizi seçiliyor
        documentService.SetAnalysisStrategy(new SentimentAnalysisStrategy());

        // Act (Eylem)
        await documentService.AnalyzeAndUpdateDocumentAsync(documentId);

        // Assert (Doğrulama)
        // 1. Analiz sonucunun doğru atanıp atanmadığını kontrol et
        Assert.Equal("Duygu Analizi: Şüpheli (Phishing riski yüksek)", fakeDocument.AnalysisResult);
        
        // 2. Repository'nin UpdateAsync metodunun tam olarak 1 kez çağrıldığını doğrula
        mockRepository.Verify(repo => repo.UpdateAsync(fakeDocument), Times.Once);
    }
}