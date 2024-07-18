using System.ComponentModel.DataAnnotations.Schema;

namespace DublinWalks.API.Modals.Domain
{
    public class Image
    {
        public  Guid Id { get; set; }

        [NotMapped]
        public   IFormFile File { get; set; }
        public string FileName { get; set; }
        //? shows nullable property (means its optional)
        public  string? FileDescription{ get; set; }
        public string FileExtension { get; set; }
        public  long FileSizeInBytes { get; set; }
        public string FilePath { get; set; }
    }
}
