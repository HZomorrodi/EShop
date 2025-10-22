using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public static Image FromBase64ToImage(this string image)
        {
            byte[] bytes = Convert.FromBase64String(image);
            using MemoryStream ms = new(bytes);
            using Image pic = Image.FromStream(ms);
            return pic;
        }
        public static string SaveBase64Image(this string image, string name, string folderName)
        {
            byte[] bytes = Convert.FromBase64String(image);
            using MemoryStream ms = new(bytes);
            using Image pic = Image.FromStream(ms);
            string imageExtension = $".{pic.RawFormat}";
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folderName, name + imageExtension);
            pic.Save(imagePath);
            return imageExtension;
        }
        public static async Task<string> ConvertToBase64(this IFormFile image)
        {
            MemoryStream ms = new();
            await image.CopyToAsync(ms);
            byte[] fileBytes = ms.ToArray();
            return Convert.ToBase64String(fileBytes);
        }
    }
}
