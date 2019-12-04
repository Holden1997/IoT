using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MQTTnet.Adapter;
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

        [System.Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            string hostIp = Configuration["MqttOption:HostIp"];
            string hostPort = Configuration["MqttOption:HostPort"];
            string hostPassword = Configuration["MqttOption:Password"];
            #region Mqtt Configuration
            services.AddSingleton<MqttClientConnectedHandler>();
            services.AddSingleton<MqttClientDisconnectedHandler>();
            services.AddSingleton<MqttBrokerStartedHandler>();
            services.AddSingleton<MqttBrokerStoppedHandler>();

            services.AddHostedMqttServer(optionBuilder =>
            {
                optionBuilder.WithDefaultEndpointPort(int.Parse(hostPort));
                optionBuilder.WithApplicationMessageInterceptor(context =>
                {
                    if (string.IsNullOrEmpty(context.ClientId))
                        return;
                    var payload = Encoding.UTF8.GetString(context.ApplicationMessage.Payload);
                    context.ApplicationMessage.Payload = Encoding.UTF8.GetBytes(context.ClientId + "/" + payload);
                });
                optionBuilder.WithConnectionValidator(validator =>
                 {
                     if (validator.Password != hostPassword)
                         validator.ReturnCode = MQTTnet.Protocol.MqttConnectReturnCode.ConnectionRefusedNotAuthorized;

                     return;
                 });
            })
                .AddMqttConnectionHandler()
                .AddConnections()
                .AddMqttWebSocketServerAdapter();

            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseConnections(c => c.MapConnectionHandler<MqttConnectionHandler>("/data", options =>
            {
                options.WebSockets.SubProtocolSelector = MQTTnet.AspNetCore.ApplicationBuilderExtensions.SelectSubProtocol;
            }));
            app.UseMqttServer(broker =>
            {
                broker.ClientConnectedHandler = app.ApplicationServices.GetService<MqttClientConnectedHandler>();
                broker.ClientDisconnectedHandler = app.ApplicationServices.GetService<MqttClientDisconnectedHandler>();
                broker.StartedHandler = app.ApplicationServices.GetService<MqttBrokerStartedHandler>();
                broker.StoppedHandler = app.ApplicationServices.GetService<MqttBrokerStoppedHandler>();
            });

            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseStaticFiles();

        }
    }
}

