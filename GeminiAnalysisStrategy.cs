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
            // Şifremiz başarıyla okunuyor
            _apiKey = configuration["GEMINI_API_KEY"] ?? Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? "API_KEY_EKSIK";
        }

        public async Task<string> AnalyzeAsync(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) 
                return "// Lütfen analiz için geçerli bir metin girin.";

            try
            {
                // 1. ADIM: Google'a "Şu an 2026 yılında hangi modellerin aktif?" diye soruyoruz
                string listUrl = $"https://generativelanguage.googleapis.com/v1beta/models?key={_apiKey}";
                var listResponse = await _httpClient.GetAsync(listUrl);
                listResponse.EnsureSuccessStatusCode();
                
                var listJson = await listResponse.Content.ReadAsStringAsync();
                using JsonDocument listDoc = JsonDocument.Parse(listJson);
                
                string workingModel = "";
                
                // Gelen listeden metin üretebilen (generateContent) ilk güncel "gemini" modelini buluyoruz
                foreach (var model in listDoc.RootElement.GetProperty("models").EnumerateArray())
                {
                    var name = model.GetProperty("name").GetString();
                    var methods = model.GetProperty("supportedGenerationMethods");
                    
                    bool supportsGeneration = false;
                    foreach (var method in methods.EnumerateArray())
                    {
                        if (method.GetString() == "generateContent")
                        {
                            supportsGeneration = true;
                            break;
                        }
                    }
                    
                    // Güncel ve çalışan modeli (Örn: models/gemini-2.5-flash) seçiyoruz
                    if (supportsGeneration && name != null && name.StartsWith("models/gemini"))
                    {
                        workingModel = name; 
                        break;
                    }
                }

                if (string.IsNullOrEmpty(workingModel))
                {
                    return "// Gemini API Hatası: Aktif ve uyumlu hiçbir yapay zeka modeli bulunamadı.";
                }

                // 2. ADIM: Bulduğumuz ve %100 çalıştığından emin olduğumuz model ile asıl analizi gerçekleştiriyoruz
                string generateUrl = $"https://generativelanguage.googleapis.com/v1beta/{workingModel}:generateContent?key={_apiKey}";

                var requestBody = new
                {
                    contents = new[]
                    {
                        new { parts = new[] { new { text = $"Bu metni siber güvenlik ve oltalama (phishing) açısından analiz et. Kısa bir özet ver ve eğer zararlıysa 'phishing', 'sahte' veya 'tehdit' kelimelerinden birini kesinlikle kullan. Eğer zararsızsa 'güvenli' veya 'temiz' kelimesini kullan. Metin: {content}" } } }
                    }
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(generateUrl, jsonContent);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return $"// Gemini API Hatası ({response.StatusCode}): {workingModel} bağlantıyı reddetti. Detay: {errorContent}";
                }
                
                var responseString = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(responseString);
                var candidates = doc.RootElement.GetProperty("candidates");
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