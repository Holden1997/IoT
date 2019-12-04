using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using IoT.Common.SharedMessages.Models;
using IoT.WebApi.Attributs;
using IoT.WebApi.Extentions;
using IoT.WebApi.NServiceBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NServiceBus;

namespace IoT.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
       
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ExeptionFilltreAttribute));

            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.RegisterModule(Configuration);
         
            #region NserviceBus
            services.AddNServiceBus("IoT.WebApi", configuration =>
            { 
                configuration.UseSerialization<NewtonsoftSerializer>();
                configuration.EnableInstallers();
                configuration.EnableCallbacks();
                configuration.MakeInstanceUniquelyAddressable("Callback");

                var transport = configuration.UseTransport<RabbitMQTransport>();
                configuration.UseTransport<RabbitMQTransport>();
                transport.UseDirectRoutingTopology();
                transport.ConnectionString("host=rabbitmq");
                var routing = transport.Routing();

                routing.RouteToEndpoint(typeof(UserMessage).Assembly, "IoT.DeviceListener");
             
            });
            #endregion
         
            #region jwt
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOpt =>
                {
                    jwtOpt.RequireHttpsMetadata = false;
                    jwtOpt.SaveToken = true;
                    jwtOpt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidAudience = Configuration["Tokens:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            #endregion

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
               app.UseHsts();
            }

            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            //});
            // app.UseHttpsRedirection();
            
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
