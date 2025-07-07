using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Common.Extensions
{
    public static class WorkWithImages
    {
        public static void SaveImage(this IFormFile image, string name, string imageExtension, string folderName)
        {
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folderName, name + imageExtension);
            using FileStream fileStream = new(imagePath, FileMode.Create);
            image.CopyTo(fileStream);
        }
        public static void RemoveImage(string imageName, string folderName)
        {
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folderName, imageName);
            File.Delete(imagePath);
        }
    }
}
