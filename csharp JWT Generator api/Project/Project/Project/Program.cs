using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Project.Auth;
using Project.Data;
using System.Text;


namespace Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ProjectContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("ProjectContext") ?? throw new InvalidOperationException("Connection string 'ProjectContext' not found.")));

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.InputFormatters.Add(new Microsoft.AspNetCore.Mvc.Formatters.XmlSerializerInputFormatter(options));
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by a space and the token"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader());
            });


            builder.Services.AddSingleton<TokenGenerator>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(options =>
               {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = false, 
               ValidateAudience = false, 
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ForTheLoveOfGodStoreAndLoadThisSecurely"))
           };
             });


            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors("AllowAll");


            app.MapControllers();

            app.Run();
        }
    }
}
