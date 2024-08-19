using _0_framework.Application;

namespace ServiceHost;

public class FileUploader : IFileUploader
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileUploader(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public string Upload(IFormFile file, string path)
    {
        if (file == null) return "";
        var directoryPath = $"{_webHostEnvironment.WebRootPath}//ProductPictures//{path}";

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var fileName = $"{DateTime.Now.ToFileName()}-{file.FileName}";
        var filePath = $"{directoryPath}//{fileName}";

        using var output = System.IO.File.Create(filePath); // Create stream for path
        file.CopyTo(output);
        /*
         * Create stream for path
         * equals above code
         */
        // using (var output = System.IO.File.Create(path))
        // {
        //     file.CopyToAsync(output);
        // }

        return $"{path}/{fileName}";
    }
}