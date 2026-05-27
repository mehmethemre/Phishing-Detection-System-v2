using System.Threading.Tasks;

namespace phishing
{
    public class SentimentAnalysisStrategy : IDocumentAnalysisStrategy
    {
        public async Task<string> AnalyzeAsync(string text)
        {
            bool isPhishingSuspicious = text.ToLower().Contains("şifre") || text.ToLower().Contains("acil");
            string result = isPhishingSuspicious 
                ? "Duygu Analizi: Şüpheli (Phishing riski yüksek)" 
                : "Duygu Analizi: Normal";

            return await Task.FromResult(result);
        }
    }
}