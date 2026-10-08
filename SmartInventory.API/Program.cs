using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartInventory.BL.Services.OrderService;
using SmartInventory.BL.Services.ProductService;
using SmartInventory.DAL.Data;
using SmartInventory.DAL.Models;
using SmartInventory.DAL.Repositories.Implementations;
using SmartInventory.DAL.Repositories.Interfaces;
using System.Text;
using Microsoft.OpenApi.Models;
using SmartInventory.BL.Services.AccountService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region Swagger Gen
var openApiSecurityDefinition = new OpenApiSecurityScheme
{
    Name = "Authorization",
    Description = "Enter valid token",
    In = ParameterLocation.Header,
    BearerFormat = "JWT",
    Scheme = "Bearer",
    Type = SecuritySchemeType.Http
};

var openApiSecurityRequirment = new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        },
        new string[] {}
    }
};

builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "Smart Inventory", Version = "v1" });

    x.AddSecurityDefinition("Bearer", openApiSecurityDefinition);

    x.AddSecurityRequirement(openApiSecurityRequirment);
}
);
#endregion

#region Add Connection string
var connection = builder.Configuration.GetConnectionString("SmartInventoryApiDb");
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(connection));
#endregion

#region Add Scoped
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IItemRepo, ItemRepo>();
builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAccountService, AccountService>();
#endregion

#region Identity 
builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(x =>
{
    x.User.RequireUniqueEmail = true;
    x.Password.RequireDigit = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
#endregion

#region Jwt
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    });
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
