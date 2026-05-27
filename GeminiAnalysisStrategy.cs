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
            _apiKey = configuration["GEMINI_API_KEY"] ?? Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? "API_KEY_EKSIK";
        }

        public async Task<string> AnalyzeAsync(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) 
                return "// Lütfen analiz için geçerli bir metin girin.";

            try
            {
                // 1. ADIM: Dinamik olarak güncel ve aktif modeli buluyoruz
                string listUrl = $"https://generativelanguage.googleapis.com/v1beta/models?key={_apiKey}";
                var listResponse = await _httpClient.GetAsync(listUrl);
                listResponse.EnsureSuccessStatusCode();
                
                var listJson = await listResponse.Content.ReadAsStringAsync();
                using JsonDocument listDoc = JsonDocument.Parse(listJson);
                
                string workingModel = "";
                
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

                // 2. ADIM: Bulduğumuz güncel model ile asıl analizi gerçekleştiriyoruz
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
                
                // İŞTE KESİN ÇÖZÜM:  indeksleri başarıyla eklendi!
                JsonElement root = doc.RootElement;
                
                // 1. candidates dizisini al ve  ile İLK elemanına gir:
                JsonElement candidatesArray = root.GetProperty("candidates");
                JsonElement firstCandidate = candidatesArray; 
                
                // 2. content objesini al:
                JsonElement contentObj = firstCandidate.GetProperty("content");
                
                // 3. parts dizisini al ve  ile İLK elemanına gir:
                JsonElement partsArray = contentObj.GetProperty("parts");
                JsonElement firstPart = partsArray; 
                
                // 4. Son olarak içindeki "text" değerini string olarak çek:
                string textResult = firstPart.GetProperty("text").GetString();

                return textResult ?? "// Analiz sonucu alınamadı.";
            }
            catch (Exception ex)
            {
                return $"// Sistem Hatası: {ex.Message}";
            }
        }
    }
}