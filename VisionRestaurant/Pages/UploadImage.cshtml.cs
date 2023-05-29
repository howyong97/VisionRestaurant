using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VisionRestaurant.Pages
{
    public class UploadImageModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHostEnvironment _environment;

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        public UploadImageModel(ILogger<IndexModel> logger, IHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }
        public void OnGet()
        {
        }
        public async Task OnPostAsync()
        {
            if (UploadedFile == null || UploadedFile.Length == 0)
            {
                return;
            }

            _logger.LogInformation($"Uploading {UploadedFile.FileName}.");
            var id = Request.Query["id"];
            string extension = Path.GetExtension(UploadedFile.FileName);
            string newFileName = id + extension;
            string targetFileName = $"{_environment.ContentRootPath}/wwwroot/Image/{newFileName}";

            using (var stream = new FileStream(targetFileName, FileMode.Create))
            {
                await UploadedFile.CopyToAsync(stream);
            }
        }
    }
}