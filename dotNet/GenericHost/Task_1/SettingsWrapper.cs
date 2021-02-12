namespace Task_1
{
    public class SettingsWrapper
    {
        public Settings Settings { get; set; }
        public bool IsSuccess { get; set; }
        public string Error { get; set; }

        public SettingsWrapper(Settings settings, bool isSuccess, string error = "")
        {
            Settings = settings;
            IsSuccess = isSuccess;
            Error = error;
        }
    }
}