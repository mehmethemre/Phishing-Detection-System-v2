using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// MVC (Controller ve View) desteğini uygulamaya ekliyoruz
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseRouting();

// Varsayılan açılış sayfasını bizim tasarladığımız DocumentController'a yönlendiriyoruz
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Document}/{action=Index}/{id?}");

app.Run();