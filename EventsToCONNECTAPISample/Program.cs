using EventsToCONNECTAPISample.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

var auth0Settings = builder.Configuration.GetSection("auth0");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = auth0Settings.GetValue<string>("Authority");
    options.Audience = auth0Settings.GetValue<string>("Audience");
});

builder.Services.AddSingleton<FailedRequestsService>();
builder.Services.AddSingleton<EventsService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
