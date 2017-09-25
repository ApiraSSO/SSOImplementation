namespace Kernel.Serialisation
{
    public class SerialisationSettings<TSettings>
    {
        public TSettings Settings { get; private set; }

        public SerialisationSettings(TSettings settings)
        {
            this.Settings = settings;
        }
    }
}