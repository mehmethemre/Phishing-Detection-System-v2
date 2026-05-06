// Strateji 2: AI Duygu Analizi (Simülasyon)
public class SentimentAnalysisStrategy : IDocumentAnalysisStrategy
{
    public string Analyze(string text)
    {
        // Burada ileride gerçek bir AI API'sine istek atılabilir.
        bool isPhishingSuspicious = text.ToLower().Contains("şifre") || text.ToLower().Contains("acil");
        return isPhishingSuspicious ? "Duygu Analizi: Şüpheli (Phishing riski yüksek)" : "Duygu Analizi: Normal";
    }
}