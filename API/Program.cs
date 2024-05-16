using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using API.Extensions;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration); // some application services have been moved into extensions
builder.Services.AddIdentityServices(builder.Configuration); // some authentication services have been moved into extensions
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

app.UseHttpsRedirection();

// following 2 are middlewares to authenticate and authorise user
app.UseAuthentication(); // check whether the user has valid token
app.UseAuthorization(); // check what the user can do with the token

app.MapControllers();

app.Run();
