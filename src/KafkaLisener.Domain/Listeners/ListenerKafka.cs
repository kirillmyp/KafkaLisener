using KafkaLisener.Domain.Listeners.Interface;
using KafkaLisener.Infrastructure.Repositories.Interface;

namespace KafkaLisener.Domain.Listeners
{
    public class ListenerKafka: IListenerKafka
    {
        private IKafka _kafka;
        public ListenerKafka(IKafka kafka) => (_kafka) = (kafka);

        public void ListenMessage()
        {
            _kafka.GetMessage();
        }
    }
}
