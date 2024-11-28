using BE_ThuyDuong.DataContext;
using BE_ThuyDuong.Interfaces;
using BE_ThuyDuong.PayLoad.Converter;
using BE_ThuyDuong.PayLoad.DTO;
using BE_ThuyDuong.PayLoad.Response;
using BE_ThuyDuong.Service.Implement;
using BE_ThuyDuong.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using BE_ThuyDuong.Implements;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//v1

//Ket noi database
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//Cho phép Fe truy cập
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy",
        policy =>
        {
            policy.WithOrigins("*")
                    .AllowAnyMethod().AllowAnyHeader();
        });
});




builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Swagger eShop Solution", Version = "v1" });
    x.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Làm theo mẫu này. Example: Bearer {Token} ",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
                    });
    //x.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection("AppSettings:SecretKey").Value!))
    };
});





// Add services to the container.
builder.Services.AddControllers();


builder.Services.AddScoped<IService_Authen, Service_Authen>();
builder.Services.AddScoped<IService_Product, Service_Product>();
builder.Services.AddScoped<IService_Card, Service_Card>();
builder.Services.AddScoped<IService_HistotyPay, Service_HistoryPay>();
builder.Services.AddScoped<IService_Product, Service_Product>();
builder.Services.AddScoped<IVNPayService, VNPayService>();
builder.Services.AddScoped<IService_Trademark, Service_Trademark>();




builder.Services.AddScoped<Converter_User>();
builder.Services.AddScoped<Converter_Trademark>();
builder.Services.AddScoped<Converter_Historypay>();
builder.Services.AddScoped<BE_ThuyDuong.PayLoad.Converter.Coverter_Product>();


builder.Services.AddScoped<ResponseBase>();
builder.Services.AddScoped<ResponseObject<DTO_Token>>();
builder.Services.AddScoped<ResponseObject<DTO_Bill>>();
builder.Services.AddScoped<ResponseObject<DTO_Product>>();
builder.Services.AddScoped<ResponseObject<DTO_User>>();
builder.Services.AddScoped<ResponseObject<DTO_Trademark>>();




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("MyPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
