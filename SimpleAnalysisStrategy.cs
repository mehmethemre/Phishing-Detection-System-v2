using System;

// Strateji 1: Basit Kelime Analizi
public class SimpleAnalysisStrategy : IDocumentAnalysisStrategy
{
    public string Analyze(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return "Metin boş.";
        int wordCount = text.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        return $"Basit Analiz: Metin {wordCount} kelimeden oluşuyor.";
    }
}