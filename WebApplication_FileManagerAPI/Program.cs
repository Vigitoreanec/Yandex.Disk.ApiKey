using GleamTech.AspNet.Core;
using GleamTech.FileUltimate;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//----------------------
//Add GleamTech to the ASP.NET Core services container.
builder.Services.AddGleamTech();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

//----------------------
//Register GleamTech to the ASP.NET Core HTTP request pipeline.
app.UseGleamTech();
//----------------------
//Set this property only if you have a valid license key, otherwise do not
//set it so FileUltimate runs in trial mode.
//"FileUltimate:LicenseKey": "QQJDJLJP34...";
//FileUltimateConfiguration.Current.LicenseKey = "QQJDJLJP34...";

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
