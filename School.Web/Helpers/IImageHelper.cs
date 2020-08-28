using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace School.Web.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imagefile, string folder);
    }
}
