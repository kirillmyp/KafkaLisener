namespace KafkaLisener.Infrastructure.Config
{
    /// <summary>
    /// Configuration class with a structure that matches a configuration.
    /// </summary>
    public class AppSettingsOptions
    {
        /// <summary>
        /// The section of configuration that provides kafka settings.
        /// </summary>
        public KafkaOptions KafkaOptions { get; set; }
    }
}
