using Microsoft.AspNetCore.Builder;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load Ocelot config file
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT at gateway (simple version; plug your real settings)
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5001"; // your identity service or mock
        options.RequireHttpsMetadata = false;
        options.Audience = "license-api";
    });

 // Add Ocelot
builder.Services.AddOcelot(builder.Configuration);

/*var jwtSection = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.RequireHttpsMetadata = false;
		options.SaveToken = true;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtSection["Issuer"],
			ValidAudience = jwtSection["Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(key)
		};
	});*/

//builder.Services.AddAuthorization();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//app.UseAuthentication();
//app.UseAuthorization();

// Ocelot middleware LAST
await app.UseOcelot();

app.Run();
