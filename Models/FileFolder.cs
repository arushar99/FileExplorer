namespace FileBrowser.Models
{
    public class FileFolder
    {

        public string? Name { get; set; }
        public long Size { get; set; }
        public bool IsFolder { get; set; }

        public int? fileCount { get; set; }
    }
}
