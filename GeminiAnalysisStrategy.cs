using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace phishing
{
    public class GeminiAnalysisStrategy : IDocumentAnalysisStrategy
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiAnalysisStrategy(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            // Şifre başarıyla okunuyor!
            _apiKey = configuration["GEMINI_API_KEY"] ?? Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? "API_KEY_EKSIK";
        }

        public async Task<string> AnalyzeAsync(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) 
                return "// Lütfen analiz için geçerli bir metin girin.";

            // ÇÖZÜM BURADA: Model ismini "gemini-1.5-flash" yerine tüm bölgelerde desteklenen "gemini-pro" yaptık.
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={_apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = $"Bu metni siber güvenlik ve oltalama (phishing) açısından analiz et. Kısa bir özet ver ve eğer zararlıysa 'phishing', 'sahte' veya 'tehdit' kelimelerinden birini kesinlikle kullan. Eğer zararsızsa 'güvenli' veya 'temiz' kelimesini kullan. Metin: {content}" } } }
                }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, jsonContent);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return $"// Gemini API Hatası ({response.StatusCode}): Google bağlantıyı reddetti. Detay: {errorContent}";
                }
                
                var responseString = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(responseString);
                var root = doc.RootElement;
                var candidates = root.GetProperty("candidates");
                var textResult = candidates.GetProperty("content").GetProperty("parts").GetProperty("text").GetString();

                return textResult ?? "// Analiz sonucu alınamadı.";
            }
            catch (Exception ex)
            {
                return $"// Sistem Hatası: {ex.Message}";
            }
        }
    }
}