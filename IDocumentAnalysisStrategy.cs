using System.Threading.Tasks;

namespace PhishingDetection.Strategies
{
    public interface IDocumentAnalysisStrategy
    {
        // Geri dönüş tipi Task<string> ve metod ismi AnalyzeAsync olmalı
        Task<string> AnalyzeAsync(string text);
    }
}