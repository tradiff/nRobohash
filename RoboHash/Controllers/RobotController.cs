using Microsoft.AspNetCore.Mvc;
using RoboHash.Services;
using SixLabors.ImageSharp;
using System.IO;
using System.Threading.Tasks;

namespace RoboHash.Controllers
{
    [Route("api/[controller]")]
    public class RobotController : Controller
    {
        private readonly RobotService _robotService;
        private readonly ShaService _shaService;

        public RobotController()
        {
            _robotService = new RobotService();
            _shaService = new ShaService();
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name, string size = "300x300")
        {
            var sizes = size.Split('x');
            if (sizes.Length != 2)
            {
                return BadRequest("Invalid size");
            }
            int width, height;
            int.TryParse(sizes[0], out width);
            int.TryParse(sizes[1], out height);

            if (width <= 0 || height <= 0 || width > 300 || height > 300)
            {
                return BadRequest("Invalid size");
            }

            var hexDigest = _shaService.Sha512HashStringForUTF8String(name);
            var hashArray = _robotService.CreateHashes(hexDigest, 11);
            var color = _robotService.GetColor(hashArray[0]);

            var assembleResults = _robotService.Assemble(hashArray, color, width, height);
            Stream outputStream = new MemoryStream();
            assembleResults.SaveAsPng(outputStream);
            outputStream.Seek(0, SeekOrigin.Begin);
            return File(outputStream, "image/png");
        }
    }
}
