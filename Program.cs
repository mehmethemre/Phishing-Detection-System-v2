using Microsoft.EntityFrameworkCore;
using phishing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Gemini API için HttpClient ve Servis kayıtları
builder.Services.AddHttpClient<IDocumentAnalysisStrategy, GeminiAnalysisStrategy>();
builder.Services.AddScoped<IDocumentAnalysisStrategy, GeminiAnalysisStrategy>();
builder.Services.AddScoped<IDocumentService, DocumentService>();

// Veritabanı bağlantısı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? Environment.GetEnvironmentVariable("DATABASE_URL")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Document}/{action=Index}/{id?}");

app.Run();