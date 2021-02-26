namespace DepsWebApp.Options
{
    public class RatesOptions
    {
        public string BaseCurrency { get; set; }
        public bool IsValid => !string.IsNullOrWhiteSpace(BaseCurrency);
    }
}
