using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PhishingDetection.Strategies
{
    public class GeminiAnalysisStrategy : IDocumentAnalysisStrategy
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        // API anahtarını Program.cs üzerinden, GitHub Secrets'tan okuyarak alıyoruz [1]
        public GeminiAnalysisStrategy(string apiKey, HttpClient httpClient)
        {
            _apiKey = apiKey;
            _httpClient = httpClient;
        }

        // Metodu asenkron (async/await) yapıya çeviriyoruz [5]
        public async Task<string> AnalyzeAsync(string text)
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                return "Hata: API anahtarı bulunamadı. Lütfen GitHub Secrets ayarlarını kontrol edin.";
            }

            // Yapay zekaya gönderilecek istek gövdesi
            var requestBody = new
            {
                contents = new[] {
                    new { parts = new[] { new { text = $"Sen bir siber güvenlik uzmanısın. Lütfen şu metni analiz et ve oltalama (phishing) riski olup olmadığını kısa bir cümleyle açıkla: {text}" } } }
                }
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}";

            try
            {
                // Asenkron POST isteği atıyoruz
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    
                    // Gemini'den gelen ham JSON içinden sadece metin kısmını ayıklıyoruz
                    using var doc = JsonDocument.Parse(responseString);
                    var resultText = doc.RootElement
                        .GetProperty("candidates")
                        .GetProperty("content")
                        .GetProperty("parts")
                        .GetProperty("text")
                        .GetString();

                    return resultText ?? "Analiz sonucu boş döndü.";
                }

                return $"API Hatası: {response.StatusCode} - Analiz gerçekleştirilemedi.";
            }
            catch (Exception ex)
            {
                return $"Bağlantı Hatası: {ex.Message}";
            }
        }
    }
}