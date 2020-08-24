namespace KafkaLisener.Infrastructure.Repositories.Interface
{
    public interface IKafka
    {
        public void GetMessage(string broker = null, string topic = null);
    }
}
