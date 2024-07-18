using DublinWalks.API.Modals.Domain;

namespace DublinWalks.API.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
