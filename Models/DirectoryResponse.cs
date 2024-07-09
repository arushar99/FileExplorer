namespace FileBrowser.Models
{
    public class DirectoryResponse
    {
       
        
            public List<FileFolder> Files { get; set; }
            public List<FileFolder> Folders { get; set; }
            public string Path { get; set; }
            public int FileCount { get; set; }
            public int FolderCount { get; set; }
            public long TotalSize { get; set; }
        
    }
}
