using System.ComponentModel.DataAnnotations;

namespace DublinWalks.API.Modals.DTO
{
    public class AddRegionRequest
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be a minimum of 3 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be a maximum of 3 characters")]
        public string Code { get; set; }
        [Required]
        [MinLength(10, ErrorMessage = "Name has to be a minimum of 10 characters")]
        public string Name { get; set; }
        public string RegionImageUrl { get; set; }
    }
}
