using System;

namespace KafkaClient.Configs.ConfigurationPrviders
{
    [Flags]
    public enum ConfigurationScope
    {
        Producer = 0,
        Consumer = 1,
        Connection = 2,
    }
}