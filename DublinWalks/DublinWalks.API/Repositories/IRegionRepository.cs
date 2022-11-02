using DublinWalks.API.Modals.Domain;

namespace DublinWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();


    }
}
