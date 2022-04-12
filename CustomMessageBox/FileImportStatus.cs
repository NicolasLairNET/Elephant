namespace MessageBox_wpf
{
    public class FileImportStatus
    {
        public string Name { get; set; } = string.Empty;
        public StatusMessage Status { get; set; }
    }

    public enum StatusMessage
    {
        Error,
        Success
    }
}
