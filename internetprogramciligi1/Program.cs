using internetprogramciligi1.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using internetprogramciligi1.Hubs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSignalR();


builder.Services.ConfigureApplicationCookie(options =>
{
    
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;

   
    options.Cookie.MaxAge = null;
});


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
});


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
});

builder.Services.AddScoped<internetprogramciligi1.Repositories.CategoryRepository>();

builder.Services.AddScoped<internetprogramciligi1.Repositories.CategoryRepository>();
builder.Services.AddScoped<internetprogramciligi1.Repositories.InstructorRepository>(); // Yeni
builder.Services.AddScoped<internetprogramciligi1.Repositories.CourseRepository>();     // Yeni


builder.Services.AddControllersWithViews();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapHub<GeneralHub>("/general-hub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // 1. Rolleri oluþtur (Yoksa ekle)
    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    if (!await roleManager.RoleExistsAsync("Uye"))
        await roleManager.CreateAsync(new IdentityRole("Uye"));

    // 2. Admin kullanýcýsýný oluþtur (Kendi numaranýzý veya mailinizi yazýn)
    // DÝKKAT: Giriþ yaparken bu maili ve þifreyi kullanacaksýnýz.
    var adminEmail = "ikaraboga579@gmail.com"; // BURAYI KENDÝ MAÝLÝNÝZ YAPIN
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };

        // Þifreniz: "123" (Program.cs ayarlarýnýzda 3 karaktere izin verdiðiniz için 123 yeterli)
        await userManager.CreateAsync(adminUser, "123");

        // Kullanýcýya Admin yetkisi ver
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}
// --- BÝTÝÞ ---

app.Run();