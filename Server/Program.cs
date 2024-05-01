using Radzen;
using Certify.Server.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveWebAssemblyComponents();
builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddHttpClient();
builder.Services.AddScoped<Certify.Server.CertifyAppService>();
builder.Services.AddDbContext<Certify.Server.Data.CertifyAppContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CertifyAppConnection"));
});
builder.Services.AddControllers().AddOData(opt =>
{
    var oDataBuilderCertifyApp = new ODataConventionModelBuilder();
    oDataBuilderCertifyApp.EntitySet<Certify.Server.Models.CertifyApp.Customer>("Customers");
    oDataBuilderCertifyApp.EntitySet<Certify.Server.Models.CertifyApp.Menu>("Menus");
    oDataBuilderCertifyApp.EntitySet<Certify.Server.Models.CertifyApp.Product>("Products");
    oDataBuilderCertifyApp.EntitySet<Certify.Server.Models.CertifyApp.Role>("Roles");
    oDataBuilderCertifyApp.EntitySet<Certify.Server.Models.CertifyApp.RoleMenuMapping>("RoleMenuMappings");
    oDataBuilderCertifyApp.EntitySet<Certify.Server.Models.CertifyApp.Store>("Stores");
    oDataBuilderCertifyApp.EntitySet<Certify.Server.Models.CertifyApp.User>("Users");
    opt.AddRouteComponents("odata/CertifyApp", oDataBuilderCertifyApp.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddScoped<Certify.Client.CertifyAppService>();
builder.Services.AddScoped<Certify.Client.CustomService>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveWebAssemblyRenderMode().AddAdditionalAssemblies(typeof(Certify.Client._Imports).Assembly);
app.Run();