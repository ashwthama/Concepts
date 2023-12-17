using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PdfReaders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFReaderController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        public PDFReaderController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        [HttpPost]
        public async Task<IActionResult> ReadPdf([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");
            var local_path = "D://PdfReaderStorage";
            var uploadsFolder = Path.Combine(local_path, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, file.FileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Ok(new { filePath });


        }

        [HttpGet("{fileName}")]
        public IActionResult DisplayPdf(string fileName)
        {
            var local_path = "D:\\PdfReaderStorage";
            var filePath = Path.Combine(local_path, "uploads", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/pdf");
        }
    }
}
