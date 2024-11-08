
using Application.Services;
using Domain;
using Domain.Enteties;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


//Authenticanton configuration 
AuthenticationSettings authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);

//JWT configuration 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    };
});


//Adding services

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen();

//AutoMapper added (Library for simplifying entities to DTO's proces)
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
//User password hashing service
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
//My services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IContactService, ContactService>();

//Database configuration
builder.Services.AddDbContext<DataContext>(opt => {
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



//Auto databese update
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var contex = services.GetRequiredService<DataContext>();
    contex.Database.Migrate();
}
catch(Exception ex)
{
    var logger = services.GetRequiredService<ILogger>();
    logger.LogError(ex, "Error accured during migration!");
}

app.Run();
