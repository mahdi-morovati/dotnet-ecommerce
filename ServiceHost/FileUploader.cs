using _0_framework.Application;

namespace ServiceHost
{
    public class FileUploader : IFileUploader
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploader(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string Upload(IFormFile file, string path)
        {
            if (file == null || file.Length == 0)
            {
                return string.Empty; // Consider throwing an exception or returning a failure result here
            }

            var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "ProductPictures", path);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fileName = $"{DateTime.Now.ToFileName()}-{file.FileName}";
            var filePath = Path.Combine(directoryPath, fileName);

            try
            {
                using var output = new FileStream(filePath, FileMode.Create);
                file.CopyTo(output);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new InvalidOperationException("Error uploading file", ex);
            }

            return Path.Combine(path, fileName).Replace("\\", "/"); // Return the path in a consistent format
        }
    }
}