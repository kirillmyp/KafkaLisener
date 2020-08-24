﻿using System;
using System.Threading;
using Confluent.Kafka;
using KafkaLisener.Domain.Listeners.Interface;
using Microsoft.AspNetCore.Mvc;

namespace KafkaLisener.Controllers
{
    [Produces("application/json")]
    [Route("home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IListenerKafka _listenerKafka;
        public HomeController(IListenerKafka listenerKafka) => (_listenerKafka) = (listenerKafka);

        [HttpGet("Get")]
        public void Get()
        {
            _listenerKafka.ListenMessage();
        }

        public void Get2(string broker = "127.0.0.1", string topic = "quickstart-events-topic")//string brokerList, List<string> topics) //, CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                // the group.id property must be specified when creating a consumer, even 
                // if you do not intend to use any consumer group functionality.
                GroupId = new Guid().ToString(),
                BootstrapServers = broker,
                // partition offsets can be committed to a group even by consumers not
                // subscribed to the group. in this example, auto commit is disabled
                // to prevent this from occurring.
                EnableAutoCommit = true
            };

            CancellationTokenSource cts = new CancellationTokenSource();

            using (var consumer =
                new ConsumerBuilder<Ignore, string>(config)
                    .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                    .Build())
            {
                consumer.Assign(new TopicPartitionOffset(topic, 0, Offset.Beginning));

                try
                {
                    //while (true)
                    //{
                    try
                    {
                        //consumeResult.Offset - 

                        var consumeResult = consumer.Consume(cts.Token);
                        Console.WriteLine($"Received message at {consumeResult.TopicPartitionOffset}: ${consumeResult.Message.Value}");
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Consume error: {e.Error.Reason}");
                    }
                    //}
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
