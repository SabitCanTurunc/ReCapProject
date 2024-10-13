using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.Abstract;
using Business.Concrete;
using Business.DependencyResloves.Autofac;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using Microsoft.Extensions.Configuration;
using Core.Extensions;

using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Core.Utilities.Security.Encryption;
using Core.Utilities.IoC;
using Core.DependencyResolvers;
using Entities.Concrete;




internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var configuration = builder.Configuration; // Configuration nesnesini bu þekilde alýn


        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
        {
            builder.RegisterModule(new AutofacBusinessModule());
        });


        builder.Services.AddDependencyResolvers(new ICoreModule[]
        {
    new CoreModule()
        });

        builder.Services.AddControllers();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                policy => policy
                    .AllowAnyOrigin() // Tüm kaynaklardan gelen istekleri kabul eder
                    .AllowAnyHeader() // Herhangi bir baþlýk türünü kabul eder
                    .AllowAnyMethod()); // Herhangi bir HTTP metodunu kabul eder
        });

        var tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = tokenOptions.Issuer,
                ValidAudience = tokenOptions.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SecurityKeyHelper.CreateSecurtyKey(tokenOptions.SecurityKey),
            };
        });




        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.Configure<CloudinarySettings>(
            builder.Configuration.GetSection("CloudinarySettings"));




        /*Autofac ile baðýmlýlýklar yönetildi

        builder.Services.AddSingleton<ICarService, CarManager>();
        builder.Services.AddSingleton<ICarDal,EfCarDal>();

        builder.Services.AddSingleton<IColorService, ColorManager>();
        builder.Services.AddSingleton<IColorDal, EfColorDal>();

        builder.Services.AddSingleton<IBrandService, BrandManager>();
        builder.Services.AddSingleton<IBrandDal, EfBrandDal>();

        builder.Services.AddSingleton<ICustomerService, CustomerManager>();
        builder.Services.AddSingleton<ICustomerDal, EfCustomerDal>();

        builder.Services.AddSingleton<IRentalService, RentalManager>();
        builder.Services.AddSingleton<IRentalDal, EfRentalDal>();

        builder.Services.AddSingleton<IUserService, UserManager>();
        builder.Services.AddSingleton<IUserDal, EfUserDal>();
        */



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.ConfigureCustomExceptionMiddleware();

        app.UseCors("AllowAllOrigins");
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();


        app.Run();
    }
}