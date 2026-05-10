using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PhishingDetection.Strategies // Kendi projenin ad alanına göre burayı düzenleyebilirsin
{
    // Sınıfımız (Class) burası
    public class GeminiAnalysisStrategy : IDocumentAnalysisStrategy
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        // API anahtarını güvenli bir şekilde dışarıdan alıyoruz
        public GeminiAnalysisStrategy(string apiKey, HttpClient httpClient)
        {
            _apiKey = apiKey;
            _httpClient = httpClient;
        }

        // İşte bahsettiğimiz Analyze metodu (fonksiyonu) burası! Gerçek API kodlarını buraya koyduk.
        public string Analyze(string text)
        {
            var requestBody = new
            {
                contents = new[] {
                    new { parts = new[] { new { text = $"Lütfen şu metni analiz et ve oltalama (phishing) riski olup olmadığını kısa bir cümleyle açıkla: {text}" } } }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}";

            var response = _httpClient.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result; 
            }

            return "Analiz yapılamadı, API bağlantısında bir sorun oluştu.";
        }
    }
}