using Microsoft.AspNetCore.Mvc;
using FileBrowser.Models;

namespace FileBrowser.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectoryController : ControllerBase
    {
        private readonly string HOME_DIR;
        

        public DirectoryController(String homeDir)
        {
            this.HOME_DIR = homeDir;
        }

        [HttpGet]
        public IActionResult GetDirectoryContents(string? path)
        {
            string fullPath = HOME_DIR;
            if(path != null)
            {
                fullPath = Path.Combine(HOME_DIR, path);
            }

            try
            {
                var filesAndFolders = GetFilesAndFolders(fullPath);
                return Ok(new DirectoryResponse
                {
                    Files = filesAndFolders.Where(f => !f.IsFolder).ToList(),
                    Folders = filesAndFolders.Where(f => f.IsFolder).ToList(),
                    Path = path,
                    FileCount = filesAndFolders.Count(f => !f.IsFolder),
                    FolderCount = filesAndFolders.Count(f => f.IsFolder),
                    TotalSize = filesAndFolders.Sum(f => f.Size)
                });
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("search")]
        public IActionResult Search(string item, string? path)
        {
            string fullPath = HOME_DIR;
            if (path != null)
            {
                fullPath = Path.Combine(HOME_DIR, path);
            }

            fullPath = Path.Combine(fullPath, item); 
            try
            {
                var filesAndFolders = GetFilesAndFolders(fullPath);
          //      var results = filesAndFolders.Where(f => f.Name.Contains(item, StringComparison.OrdinalIgnoreCase));
         //       var results = filesAndFolders.Where(f => f.Name.Contains(item, StringComparison.OrdinalIgnoreCase));
                var results = filesAndFolders;
                return Ok(new DirectoryResponse
                {
                    Files = results.Where(f => !f.IsFolder).ToList(),
                    Folders = results.Where(f => f.IsFolder).ToList(),
                    Path = item,
                    FileCount = results.Count(f => !f.IsFolder),
                    FolderCount = results.Count(f => f.IsFolder),
                    TotalSize = results.Sum(f => f.Size)
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("upload")]
        public IActionResult UploadFile(IFormFile file, string? path)
        {
            string fullPath = HOME_DIR;
            if (path != null)
            {
                fullPath = Path.Combine(HOME_DIR, path);
            }

            var filePath = Path.Combine(fullPath, file.FileName);
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return Ok();
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("download")]
        public IActionResult DownloadFile(string path)
        {
            var fullPath = Path.Combine(HOME_DIR, path);
            var file = new FileInfo(fullPath);
            var fs = new FileStream(fullPath, FileMode.Open); // convert it to a stream
            // return PhysicalFile(file.FullName, file.Name, true);

            return File(fs, "application/octet-stream", file.Name);
        }

        private IEnumerable<FileFolder> GetFilesAndFolders(string path)
        {
            var filesAndFolders = new List<FileFolder>();
            var files = Directory.GetFiles(path);
            var folders = Directory.GetDirectories(path);

            foreach (var file in files)
            {
                try
                {
                    var fileInfo = new FileInfo(file);
                    filesAndFolders.Add(new FileFolder
                    {
                        Name = fileInfo.Name,
                        Size = fileInfo.Length,
                        IsFolder = false
                    });
                }
                catch (Exception e) { }
                
            }

            foreach (var folder in folders)
            {
                try
                {
                    var folderInfo = new DirectoryInfo(folder);
                    filesAndFolders.Add(new FileFolder
                    {
                        Name = folderInfo.Name,
                        Size = GetFolderSize(folder),
                        IsFolder = true
                    });
                }catch(Exception e) { }
            }

            return filesAndFolders;
        }

        private long GetFolderSize(string folderPath)
        {
            var files = Directory.GetFiles(folderPath);
            var folders = Directory.GetDirectories(folderPath);

            var size = files.Sum(f => new FileInfo(f).Length);

            foreach (var folder in folders)
            {
                size += GetFolderSize(folder);
            }

            return size;
        }
    }
}

