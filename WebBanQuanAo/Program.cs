using Microsoft.EntityFrameworkCore;
using WebBanQuanAo.Models;
using Microsoft.Extensions.DependencyInjection;
using WebBanQuanAo.Serveice;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký DbContext với chuỗi kết nối trong appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Thêm dịch vụ Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian timeout của Session
    options.Cookie.HttpOnly = true; // Bảo mật cookie
    options.Cookie.IsEssential = true; // Cookie cần thiết
});

// Thêm dịch vụ MVC
builder.Services.AddControllersWithViews();

// Đăng ký dịch vụ Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// Cấu hình middleware cho ứng dụng
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Trang lỗi chi tiết trong chế độ phát triển
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Kích hoạt Session
app.UseSession();

// Kích hoạt Authorization
app.UseAuthorization();

// Định tuyến mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
