using System;
using System.Threading.Tasks;

namespace phishing
{
    public class SimpleAnalysisStrategy : IDocumentAnalysisStrategy
    {
        public async Task<string> AnalyzeAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) 
                return await Task.FromResult("Metin boş.");

            int wordCount = text.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
            return await Task.FromResult($"Basit Analiz: Metin {wordCount} kelimeden oluşuyor.");
        }
    }
}