# Phishing Detective - Siber Güvenlik Analiz Sistemi

## Proje Açıklaması
Phishing Detective, internet kullanıcılarını oltalama (phishing) saldırılarına karşı korumak amacıyla geliştirilmiş, yapay zeka destekli dinamik bir siber güvenlik analiz sistemidir. Sistem, kullanıcıların şüpheli e-posta, SMS, URL veya sosyal medya içeriklerini analiz ederek anında tehdit değerlendirmesi yapar ve oltalama (phishing) girişimlerini kullanıcılara Matrix temalı dinamik bir arayüz ile bildirir.

## Kullanılan Teknolojiler
* **Backend:** C#, .NET 9.0, ASP.NET Core MVC
* **Frontend:** HTML5, CSS3, JavaScript (Dinamik Matrix DOM manipülasyonu)
* **Veritabanı (ORM):** Supabase (PostgreSQL), Entity Framework Core (Repository Pattern)
* **Yapay Zeka:** Google Gemini API (Duygu ve Tehdit Analizi için Strategy Pattern ile)
* **DevOps & Bulut:** GitHub Actions (CI/CD Pipeline), Render (Production Deployment)
* **Proje Yönetimi:** Jira (Kanban, Çevik Yazılım Geliştirme)

## Kurulum Adımları
Projeyi yerel ortamınızda (local) çalıştırmak için aşağıdaki adımları izleyin:

1. Depoyu bilgisayarınıza klonlayın:
   `git clone https://github.com/mehmethemre/Phishing-Detection-System-v2.git`
2. Proje dizinine gidin:
   `cd Phishing-Detection-System-v2`
3. Gerekli .NET paketlerini yükleyin:
   `dotnet restore`
4. Ortam değişkenlerini (API ve Veritabanı) ayarlayın:
   Projeyi çalıştırmak için `appsettings.json` dosyasına (veya User Secrets / Environment Variables) Supabase veritabanı bağlantı dizesini (Connection String) ve `GEMINI_API_KEY` değerinizi ekleyin.

## Çalıştırma Talimatları
* Kurulum adımlarını tamamladıktan sonra terminal veya komut satırına `dotnet run` yazarak projeyi başlatın.
* Tarayıcınızda `http://localhost:5000` (veya console'da belirtilen port) adresine giderek uygulamaya erişebilirsiniz.
* Arayüzdeki metin kutusuna şüpheli metni girin ve "Analizi Başlat" butonuna tıklayarak AI destekli sonucu görüntüleyin.

## Ekran Görüntüleri
*(Not: Bu bölüme GitHub üzerinden projenin kırmızı tehdit ekranının ve yeşil güvenli ekranının resimlerini yükleyebilirsiniz.)*

## Takım Üyeleri ve Rolleri
* **Mehmet Emre Erdoğan** - Proje Yöneticisi / Full Stack & AI Geliştirici *(Proje mimarisi, Gemini API entegrasyonu, CI/CD Pipeline kurulumu ve bulut dağıtım süreçleri)*
* **Rıfat Ertuğrul Kumru** - Backend ve Veritabanı (ORM) Sorumlusu *(Supabase PostgreSQL veritabanı bağlantıları, Entity Framework Core yapılandırması ve Repository Pattern'in uygulanması)*
* **Murat Tunç** - Frontend Geliştirici ve Test Sorumlusu *(Matrix temalı dinamik DOM UI tasarımları, arayüz fonksiyonları ve xUnit birim testlerinin (Unit Testing) yazılması)*
