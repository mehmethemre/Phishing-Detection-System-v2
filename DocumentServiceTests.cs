using System.Threading.Tasks;
using Moq;
using Xunit;
using PhishingDetection.Services;   // DocumentService'i bulabilmesi için
using PhishingDetection.Strategies; // SentimentAnalysisStrategy'yi bulabilmesi için

// Eğer Document ve IRepository sınıfların farklı bir klasördeyse orayı da eklemelisin
// örneğin: using PhishingDetection.Models; 

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
            Content = "Lütfen acil olarak şifrenizi güncelleyin.",
            AnalysisResult = "" 
        };

        var mockRepository = new Mock<IRepository<Document>>();
        mockRepository.Setup(repo => repo.GetByIdAsync(documentId))
                      .ReturnsAsync(fakeDocument);

        var documentService = new DocumentService(mockRepository.Object);
        
        // Strateji adresini yukarıda using ile eklediğimiz için artık tanınacaktır
        documentService.SetAnalysisStrategy(new SentimentAnalysisStrategy());

        // Act (Eylem)
        await documentService.AnalyzeAndUpdateDocumentAsync(documentId);

        // Assert (Doğrulama)
        Assert.Equal("Duygu Analizi: Şüpheli (Phishing riski yüksek)", fakeDocument.AnalysisResult);
        mockRepository.Verify(repo => repo.UpdateAsync(fakeDocument), Times.Once);
    }
}