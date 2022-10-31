using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace musica_mensageria_consumer;

public class Worker : BackgroundService, IHostedService
{
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Musica();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        private static void Musica()
        {
            IConnectionFactory factory = ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var musicaConsummerEvent = new EventingBasicConsumer(channel);

            musicaConsummerEvent.Received += (model, basicDeliverEventArgs) => 
            {
                basicDeliverEventArgs.RoutingKey = "tocar-route";
                basicDeliverEventArgs.Exchange = "musica-exchange";

                var body = basicDeliverEventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Mensagem recebido no consumer musica | {message}");
            };

            channel.BasicConsume(queue: "musica-queue",
                                 autoAck: true,
                                 consumer: musicaConsummerEvent);
        }



        /// <summary>
        /// Criar uma ConnectionFactory
        /// Configuracao da mensageria no consumidor
        /// </summary>
        /// <returns></returns>
        private static IConnectionFactory ConnectionFactory()
        {
            ConnectionFactory factory = new()
            {
                VirtualHost = "musica",
                Uri = new Uri("amqp://guest:guest@host.docker.internal:5672/")
            };

            return factory;
        }
    }