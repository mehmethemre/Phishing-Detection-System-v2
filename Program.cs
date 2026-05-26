using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore; // Veritabanı işlemleri için eklendi

var builder = WebApplication.CreateBuilder(args);

// MVC (Controller ve View) desteğini uygulamaya ekliyoruz
builder.Services.AddControllersWithViews();

// Veritabanı bağlantısını PostgreSQL (Supabase) kullanacak şekilde ekliyoruz
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseRouting();

// Varsayılan açılış sayfasını bizim tasarladığımız DocumentController'a yönlendiriyoruz
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Document}/{action=Index}/{id?}");

app.Run();