using System;
using System.Globalization;

namespace Kernel.Federation.MetaData.Configuration
{
    public class LocalizedConfigurationEntry
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public CultureInfo Language { get; set; }
        public LocalizedConfigurationEntry()
        {
            this.Language = CultureInfo.CurrentCulture;
        }
    }
    public class LocalizedUrlEntry
    {
        public Uri Url { get; set; }
        
        public CultureInfo Language { get; set; }
        public LocalizedUrlEntry()
        {
            this.Language = CultureInfo.CurrentCulture;
        }
    }
}