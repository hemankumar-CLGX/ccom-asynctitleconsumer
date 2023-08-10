using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using Serilog;
using System.Collections.Generic;
using CCOM.AsyncTitleServiceConsumer.Services;
using CCOM.AsyncTitleServiceConsumer.Consumers;
using CCOM.AsyncTitleServiceConsumer.Contracts;
using MassTransit.Transports.Fabric;

namespace CCOM.AsyncTitleServiceConsumer
{

    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args)
             .UseWindowsService()
             .Build()
             .RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
              .ConfigureServices((hostContext, services) =>
              {
                  var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

                  var config = new ConfigurationBuilder()
                  .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"appsettings.{environment}.json", optional: true)
                  .AddEnvironmentVariables()
                  .Build();
                  
                  services.AddHttpClient<TitleAsyncService>(client =>
                  {                      
                      //client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
                      //client.BaseAddress = new Uri("");
                  });
                  services.AddTransient<ITitleAsyncService, TitleAsyncService>();

                  ConfigureMassTransit(services, config);
              }).UseSerilog((ctx, services, logger) =>
              {
                  logger.ReadFrom.Configuration(ctx.Configuration);
              });

        private static void ConfigureMassTransit(IServiceCollection services, IConfigurationRoot config)
        {

            services.AddMassTransit(x =>
            {
                var rabbit = config.GetSection("RabbitMQ");
                var hostName = rabbit["host"];
                var vhost = rabbit["vhost"];
                var queue = rabbit["queue"];
                var exchange = rabbit["ExchangeName"];
                var exchangeType = rabbit["Exchangetype"];

                Console.WriteLine($"Server: {hostName} - Virtual Host: {vhost} - Queue: {queue} - Exchange: {exchange}");

                x.AddConsumer<AsyncTitleConsumer>();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                   
                    cfg.Host(hostName, host =>
                    {
                        host.Username(rabbit["Username"]);
                        host.Password(rabbit["Password"]);
                    });

                    cfg.ClearSerialization();
                    cfg.UseRawJsonSerializer();

                    cfg.ReceiveEndpoint(queue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        // Less memory stuff
                        //e.Lazy = true;
                        // e.Bind(exchange);
                       
                        e.ExchangeType = "direct";
                        e.PrefetchCount = int.Parse(rabbit["PrefetchCount"]);
                        e.UseMessageRetry(r =>
                        {
                            r.Interval(int.Parse(rabbit["RetryCount"]), int.Parse(rabbit["RetryInterval"]));
                            r.Ignore<ArgumentNullException>();
                                //r.Ignore<SqlException>(x => x.Message.Contains("Incorrect syntax"));
                                //r.Ignore<SqlException>(x => x.Message.Contains("String or binary data would be truncated"));
                            r.Ignore<KeyNotFoundException>();
                        });
                        e.ConfigureConsumer<AsyncTitleConsumer>(ctx);
                    });
                    cfg.Message<AysncTitleProviderMessage>(x => x.SetEntityName(exchange));
                });

            });

        }
    }
}