using System;
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
    }
}
