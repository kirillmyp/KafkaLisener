using Confluent.Kafka;
using KafkaLisener.Infrastructure.Config;
using KafkaLisener.Infrastructure.Repositories.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Threading;

namespace KafkaLisener.Infrastructure.Repositories
{
    public class Kafka : IKafka
    {
        private KafkaOptions _kafkaOptions { get; }
        private Offset ConsumerOffset { get; set; }

        public Kafka(IOptions<AppSettingsOptions> appSettingsOptions)
        {
            _kafkaOptions = appSettingsOptions?.Value?.KafkaOptions ?? throw new ArgumentNullException(nameof(appSettingsOptions));
        }

        public void GetMessage(string broker = null, string topic = null)
        {
            broker = string.IsNullOrEmpty(broker) ? _kafkaOptions.Broker : broker;
            topic = string.IsNullOrEmpty(topic) ? _kafkaOptions.Topic : topic;

            var config = new ConsumerConfig
            {
                GroupId = new Guid().ToString(),
                ClientId = topic,
                BootstrapServers = broker,
                EnableAutoCommit = true
            };

            CancellationTokenSource cts = new CancellationTokenSource();

            using (var consumer =
                new ConsumerBuilder<Ignore, string>(config)
                    .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                    .Build())
            {
                consumer.Assign(new TopicPartitionOffset(topic, 0, ConsumerOffset != null ? ConsumerOffset : Offset.Beginning));

                try
                {
                    while (true)
                    {
                        try
                        {
                            //consumeResult.Offset - session variable

                            var consumeResult = consumer.Consume(cts.Token);
                            Console.WriteLine($"Received message at {consumeResult.TopicPartitionOffset}: ${consumeResult.Message.Value}");
                            ConsumerOffset = consumeResult.Offset;
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Consume error: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Closing consumer.");
                    consumer.Close();
                }
            }
        }
    }
}
