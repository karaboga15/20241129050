using internetprogramciligi1.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabaný Baðlantýsý
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Identity (Kimlik) Ayarlarý (SADECE BÝR KERE YAZILMALI)
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Çerez (Cookie) Ayarlarýný Düzenle
builder.Services.ConfigureApplicationCookie(options =>
{
    // Çerezin sadece 30 dakika geçerli olmasýný saðla.
    // Eðer kullanýcý aktifse süre yenilenir (SlidingExpiration: true).
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;

    // ÖNEMLÝ: Tarayýcý kapatýlýnca çerezin süresini doldurmasýný garantilemek için MaxAge null kalmalý.
    options.Cookie.MaxAge = null;
});

// 3. Þifre Basit Olsun (Test için)
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
});

// 4. Giriþ Yapýlmadýysa Nereye Gitsin? (Yönlendirme Ayarý)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
});

// Repository Tanýmlamalarý
builder.Services.AddScoped<internetprogramciligi1.Repositories.CategoryRepository>();

// Repository Tanýmlamalarý
builder.Services.AddScoped<internetprogramciligi1.Repositories.CategoryRepository>();
builder.Services.AddScoped<internetprogramciligi1.Repositories.InstructorRepository>(); // Yeni
builder.Services.AddScoped<internetprogramciligi1.Repositories.CourseRepository>();     // Yeni

// 5. MVC Servisleri
builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- Uygulama Baþlatma Ayarlarý ---

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Önce Kimlik, Sonra Yetki
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();