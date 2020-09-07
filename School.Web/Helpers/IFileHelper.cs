using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace School.Web.Helpers
{
    public interface IFileHelper
    {
        Task<string> UploadFileAsync(IFormFile uploadFile);
    }
}
