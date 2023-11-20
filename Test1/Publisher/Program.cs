using RabbitMQ.Client;
using System;
using System.Text;
namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("topicexchange", type: ExchangeType.Topic);
                for (int i = 1; i <= 100; i++)
                {
                    byte[] bytemessage = Encoding.UTF8.GetBytes($"{i}. görev verildi.");

                    IBasicProperties properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    channel.BasicPublish(exchange: "topicexchange", routingKey: $"Asker.Subay.{(i % 2 == 0 ? "Yuzbasi" : (i % 11 == 0 ? "Binbasi" : "Tegmen"))}", basicProperties: properties, body: bytemessage);
                }
            }
        }
    }
}
