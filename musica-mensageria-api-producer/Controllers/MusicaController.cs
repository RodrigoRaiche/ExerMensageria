using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using musica_mensageria_api_producer.Dto;
using RabbitMQ.Client;

namespace musica_mensageria_api_producer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MusicaController : ControllerBase
{

    [HttpGet("musica")]
    public ActionResult<bool> Musica()
    {

        var factory = ConnectionFactory();

        using (var connection = factory.CreateConnection())
        {
            using (var channel = connection.CreateModel())
            {

                while (true)
                {
                    channel.ExchangeDeclare("musica-exchange", "direct");
                    channel.QueueDeclare(queue: "musica-queue",
                        durable: true,
                        exclusive: false,
                        autoDelete: false);

                    channel.QueueBind(queue: "musica-queue",
                        exchange: "musica-exchange",
                        routingKey: "tocar-route");

                    GuitarraDto guitarra = new GuitarraDto(1, "Guitarra 1");

                    string jsonString = JsonSerializer.Serialize(guitarra);
                    var body = Encoding.UTF8.GetBytes(jsonString);

                    channel.BasicPublish(exchange: "musica-exchange",
                        routingKey: "tocar-route",
                        body: body);

                }
            }
        }

        return Ok(true);
    }

    /// <summary>
    /// Criar uma ConnectionFactory
    /// Configuracao da mensageria no producer
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