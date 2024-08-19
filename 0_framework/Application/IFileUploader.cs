using Microsoft.AspNetCore.Http;

namespace _0_framework.Application;

public interface IFileUploader
{
    string Upload(IFormFile file, string path);
}