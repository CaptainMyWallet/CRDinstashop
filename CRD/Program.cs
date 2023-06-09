global using CRD.Models;
using CRD.Interfaces;
using CRD.Repository;
using CRD.Services;
using log4net.Config;
using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using CRD.Middleware;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:3000", "http://localhost:3000").AllowAnyHeader()
                                                  .AllowAnyMethod(); 
                      });
});
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Standard Authorization header. Bearer",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer
(options =>
{
options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
{

    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8
    .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
    ValidateIssuer = false,
    ValidateAudience = false
};
});



builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<LoanRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<SaleShopsRepository>();
builder.Services.AddScoped<ShopsRepository>();
builder.Services.AddScoped<TagsRepository>();
builder.Services.AddScoped<WeekShopRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISaleShopsService, SaleShopsService>();
builder.Services.AddScoped<IShopsService, ShopService>();
builder.Services.AddScoped<ITagsService, TagsService>();
builder.Services.AddScoped<IWeekShopService, WeekShopService>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseMiddleware(typeof(CheckMiddleware));
app.UseMiddleware(typeof(ErrorHandlingMiddleware));

//app.UseHttpLogging();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

var repository = LogManager.CreateRepository("Rolling");

XmlConfigurator.Configure(repository, new FileInfo("log4net.xml"));

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors(MyAllowSpecificOrigins);
app.MapControllers();

app.Run();
