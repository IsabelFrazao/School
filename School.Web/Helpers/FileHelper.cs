using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Helpers
{
    public class FileHelper : IFileHelper
    {
        public async Task<string> UploadFileAsync(IFormFile uploadFile)
        {
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.jpg";

            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\UploadExcel\\", file);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await uploadFile.CopyToAsync(stream);
            }

            return path = $"~/wwwroot/UploadExcel/{file}";
        }
    }
}
