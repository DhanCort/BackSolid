/*TASK RP.StartUp Relevant Part Satartup*/
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Odyssey2Backend.Alert;
using System;
using System.Text;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: April 13, 2020.

namespace Odyssey2Backend
{
    //==================================================================================================================
    public class Startup
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public IConfiguration Configuration { get; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.
        public Startup(
            IConfiguration configuration
            )
        {
            Configuration = configuration;
        }

        //--------------------------------------------------------------------------------------------------------------
        public void ConfigureServices(
            //                                              //This method gets called by the runtime. Use this method to 
            //                                              //      add services to the container.

            IServiceCollection services
            )
        {
            services.AddControllers()
                .AddXmlDataContractSerializerFormatters();

            services.AddAuthentication()
               .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //                                      //What about token to validate.
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    //                                      //Valid data about token.
                    ValidIssuer = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection(
                        "Odyssey2Settings")["MI4PK"],
                    ValidAudience = "front",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new ConfigurationBuilder().
                        AddJsonFile("appsettings.json").Build().GetSection("Odyssey2Settings")["MI4PK"]))
                })
               .AddJwtBearer("Wisnet", options =>
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       //                                      //What about token to validate.
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,

                       //                                      //Valid data about token.
                       ValidIssuer = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection(
                            "WisnetAPI")["Issuer"],
                       ValidAudience = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection(
                            "WisnetAPI")["Issuer"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new ConfigurationBuilder().
                            AddJsonFile("appsettings.json").Build().GetSection("WisnetAPI")["Key"]))
                   }
               );

            //services.AddCors(o => o.AddPolicy("CorsPolicy", builder => {
            //    builder
            //    .AllowAnyMethod()
            //    .AllowAnyHeader()
            //    .AllowCredentials()
            //    .WithOrigins("http://192.168.0.2:5002/");
            //}));

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
            });

            services.AddSignalR();
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Configure(
            //                                              //This method gets called by the runtime. Use this method to 
            //                                              //      configure the HTTP request pipeline.

            IApplicationBuilder app,
            IWebHostEnvironment env
            )
        {
            if (
                env.IsDevelopment()
                )
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ConnectionHub>("/connectionHub");
            });
        }
    }
}
