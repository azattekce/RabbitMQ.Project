using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Consumer
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

                string queueName = channel.QueueDeclare().QueueName;
                string routingKey = ".Tegmen";

                routingKey = args[0] switch
                {
                    "1" => $"*.*.Tegmen",
                    "2" => $"*.#.Yuzbasi",
                    "3" => $"#.Binbasi.#",
                    "4" => $"Asker.Subay.Tegmen",
                };

                channel.QueueBind(queue: queueName, exchange: "topicexchange", routingKey: routingKey);

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queueName, false, consumer);
                consumer.Received += (sender, e) =>
                {
                    Console.WriteLine($"{routingKey} {Encoding.UTF8.GetString(e.Body)} görevi aldı.");
                    channel.BasicAck(e.DeliveryTag, false);
                };
                Console.Read();
            }
        }
    }
}
