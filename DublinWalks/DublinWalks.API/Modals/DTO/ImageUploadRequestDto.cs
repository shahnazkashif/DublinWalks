using System.ComponentModel.DataAnnotations;

namespace DublinWalks.API.Modals.DTO
{
    public class ImageUploadRequestDto
    {
        [Required]
        public IFormFile File  { get; set; }

        [Required]
        public string FileName { get; set; }

        public string? FileDescription { get; set; }

    }
}
