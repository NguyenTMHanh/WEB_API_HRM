using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API_HRM.Models;
using WEB_API_HRM.Data;
using Microsoft.EntityFrameworkCore;

namespace WEB_API_HRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly HRMContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploadController(HRMContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            Console.WriteLine("Received request for UploadAvatar");
            Console.WriteLine($"Content-Type: {Request.ContentType}");
            Console.WriteLine($"Received file: {file?.FileName}, Size: {file?.Length}");
            Console.WriteLine($"WebRootPath: {_webHostEnvironment.WebRootPath}");
            Console.WriteLine($"ContentRootPath: {_webHostEnvironment.ContentRootPath}");

            if (file == null || file.Length == 0)
            {
                Console.WriteLine("No file received");
                return BadRequest(new { message = "No file uploaded" });
            }

            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".doc" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                Console.WriteLine("Invalid file type");
                return BadRequest(new { message = "Only image files are allowed" });
            }

            // Validate file size
            if (file.Length > 5 * 1024 * 1024)
            {
                Console.WriteLine("File size too large");
                return BadRequest(new { message = "File size must be less than 5MB" });
            }

            try
            {
                // Use ContentRootPath as fallback if WebRootPath is null
                var rootPath = !string.IsNullOrEmpty(_webHostEnvironment.WebRootPath)
                    ? _webHostEnvironment.WebRootPath
                    : _webHostEnvironment.ContentRootPath;

                // Create uploads directory if it doesn't exist
                var uploadsFolder = Path.Combine(rootPath, "uploads", "avatars");
                if (!Directory.Exists(uploadsFolder))
                {
                    Console.WriteLine("Creating directory: " + uploadsFolder);
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate unique filename
                var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save file to disk
                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        Console.WriteLine("File saved to: " + filePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error saving file: " + ex.Message);
                    throw;
                }

                // Save file info to database
                var fileInfo = new FileUpload
                {
                    Id = Guid.NewGuid().ToString(),
                    FileName = file.FileName,
                    UniqueFileName = uniqueFileName,
                    FilePath = $"/uploads/avatars/{uniqueFileName}",
                    ContentType = file.ContentType,
                    FileSize = file.Length,
                    UploadDate = DateTime.UtcNow
                };

                try
                {
                    _context.FileUploads.Add(fileInfo);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("File info saved to database: " + fileInfo.Id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error saving to database: " + ex.Message);
                    // Delete the file if it was saved
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                        Console.WriteLine("Deleted orphaned file: " + filePath);
                    }
                    throw;
                }

                return Ok(new
                {
                    id = fileInfo.Id,
                    fileName = fileInfo.FileName,
                    filePath = fileInfo.FilePath,
                    fileSize = fileInfo.FileSize
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error uploading file: " + ex.Message + ", StackTrace: " + ex.StackTrace);
                return StatusCode(500, new { message = "Error uploading file", error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("GetFile/{id}")]
        public async Task<IActionResult> GetAvatar(string id)
        {
            var fileInfo = await _context.FileUploads.FindAsync(id);
            if (fileInfo == null)
            {
                return NotFound(new { message = "File not found" });
            }

            // Use ContentRootPath as fallback if WebRootPath is null
            var rootPath = !string.IsNullOrEmpty(_webHostEnvironment.WebRootPath)
                ? _webHostEnvironment.WebRootPath
                : _webHostEnvironment.ContentRootPath;

            var filePath = Path.Combine(rootPath, "uploads", "avatars", fileInfo.UniqueFileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { message = "Physical file not found" });
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, fileInfo.ContentType, fileInfo.FileName);
        }

        [HttpDelete("DeleteFile/{id}")]
        public async Task<IActionResult> DeleteAvatar(string id)
        {
            var fileInfo = await _context.FileUploads.FindAsync(id);
            if (fileInfo == null)
            {
                return NotFound(new { message = "File not found" });
            }

            try
            {
                // Use ContentRootPath as fallback if WebRootPath is null
                var rootPath = !string.IsNullOrEmpty(_webHostEnvironment.WebRootPath)
                    ? _webHostEnvironment.WebRootPath
                    : _webHostEnvironment.ContentRootPath;

                // Delete physical file
                var filePath = Path.Combine(rootPath, "uploads", "avatars", fileInfo.UniqueFileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Delete from database
                _context.FileUploads.Remove(fileInfo);
                await _context.SaveChangesAsync();

                return Ok(new { message = "File deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting file", error = ex.Message });
            }
        }
    }
}