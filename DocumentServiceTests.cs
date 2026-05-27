using System.Threading.Tasks;
using Xunit;

namespace phishing
{
    public class DocumentServiceTests
    {
        [Fact]
        public async Task AnalyzeTextAsync_ShouldReturnSuspicious_WhenTextContainsAcil()
        {
            // Arrange (Hazırlık)
            // Sentiment stratejimizi güncel servisimize enjekte ediyoruz
            var strategy = new SentimentAnalysisStrategy();
            var documentService = new DocumentService(strategy);
            
            string testMetni = "Lütfen acil olarak şifrenizi güncelleyin.";

            // Act (Eylem)
            string result = await documentService.AnalyzeTextAsync(testMetni);

            // Assert (Doğrulama)
            Assert.Equal("Duygu Analizi: Şüpheli (Phishing riski yüksek)", result);
        }
    }
}