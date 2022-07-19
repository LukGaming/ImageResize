using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Drawing;
using System.Drawing.Drawing2D;
using ImageMagick;
using System.Configuration;
using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace ImageResize.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        [HttpPost]
        [RequestSizeLimit(3333333333)]
        public async Task<ActionResult> PostImages(List<IFormFile> files)
        {
            string dir = "images";
            // If directory does not exist, create it
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.Combine(dir,
                    Path.GetFileName(dir));
                    using (var stream = System.IO.File.Create(dir + "/" + formFile.FileName))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    const int width = 450;
                    const int heigth = 300;
                    var imagePath = dir + "/" + formFile.FileName;
                    using (var image = new MagickImage(dir + "/" + formFile.FileName))
                    {
                        image.Resize(width, heigth);
                        image.Strip();
                        //image.Format = MagickFormat.WebP;
                        image.Write(imagePath);
                    }
                }
            }
            return Ok("Imagem Criada");
        }
        [HttpGet]
        public async Task<IActionResult> GetFile(string image_name)
        {
            var temporaryImage = System.IO.File.OpenRead("images/" + image_name);
            //Replace "image/jpeg" with the mimetype of your image.
            return File(temporaryImage, "image/jpg");
        }

    }
}
