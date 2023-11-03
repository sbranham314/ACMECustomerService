using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.EntityFrameworkCore;
using ACMECorpCustomerService.Data;
using Microsoft.AspNetCore.Authentication.Certificate;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate(options =>
{
    options.Events = new CertificateAuthenticationEvents
    {
        OnCertificateValidated = context =>
        {
            var claims = new[]
            {
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        context.ClientCertificate.Subject,
                        ClaimValueTypes.String, context.Options.ClaimsIssuer),
                    new Claim(
                        ClaimTypes.Name,
                        context.ClientCertificate.Subject,
                        ClaimValueTypes.String, context.Options.ClaimsIssuer)
                };

            context.Principal = new ClaimsPrincipal(
                new ClaimsIdentity(claims, context.Scheme.Name));
            context.Success();

            return Task.CompletedTask;
        }
    };
}).AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

builder.Services.AddDbContextFactory<ApplicationDBContext>(
    o => SqlServerDbContextOptionsExtensions.UseSqlServer(o, configuration.GetConnectionString("SqlServer")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{

}
