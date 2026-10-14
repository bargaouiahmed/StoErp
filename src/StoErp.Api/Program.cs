using StoErp.Modules.Auth.Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore.Design;
using StoErp.Shared.Infrastructure.Messaging;
using StoErp.Shared.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using StoErp.Modules.Emailing.Infrastructure.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMediator(config =>
{
    config.ServiceLifetime = ServiceLifetime.Scoped;
});
builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtOptions:Issuer"] ?? throw new InvalidOperationException("JWT Issuer not found in configuration."),
        ValidAudience = builder.Configuration["JwtOptions:Audience"] ?? throw new InvalidOperationException("JWT Audience not found in configuration."),
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["JwtOptions:Secret"] ?? throw new InvalidOperationException("JWT Secret not found in configuration.")))
    };
});
builder.Services.AddAuthInfrastructure(builder.Configuration);
builder.Services.AddSharedInfrastructure();
builder.Services.AddEmailingInfrastructure(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseAuthentication();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
