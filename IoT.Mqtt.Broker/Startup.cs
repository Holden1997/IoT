using IoT.Mqtt.Server.Broker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet.AspNetCore;
using MQTTnet.Server;
using System.Text;

namespace IoT.Mqtt.Broker
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
            string hostIp = Configuration["MqttOption:HostIp"];
            string hostPort = Configuration["MqttOption:HostPort"];
            #region Mqtt Configuration
            services.AddSingleton<MqttClientConnectedHandler>();
            services.AddSingleton<MqttClientDisconnectedHandler>();

           
            services.AddMqttServer(optionBuilder => {
                optionBuilder.WithDefaultEndpointPort(1884);
                optionBuilder.WithApplicationMessageInterceptor(context =>
                {
                    if (string.IsNullOrEmpty(context.ClientId))
                        return;
                    var payload = Encoding.UTF8.GetString(context.ApplicationMessage.Payload);
                    context.ApplicationMessage.Payload = Encoding.UTF8.GetBytes(context.ClientId + "/" + payload);
                });
            });
            services.AddConnections();
            services.AddMqttConnectionHandler();

            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            app.UseConnections(c => c.MapConnectionHandler<MqttConnectionHandler>("/data", options =>
            {
                options.WebSockets.SubProtocolSelector = MQTTnet.AspNetCore.ApplicationBuilderExtensions.SelectSubProtocol;
            }));
            app.UseMqttEndpoint("/data");
            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}

