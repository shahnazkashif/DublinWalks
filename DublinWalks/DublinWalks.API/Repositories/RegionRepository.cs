using DublinWalks.API.Data;
using DublinWalks.API.Modals.Domain;
using Microsoft.EntityFrameworkCore;

namespace DublinWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly DublinDbContext dublinwalkdb;

        public RegionRepository(DublinDbContext dublinwalkdb)
        {
            this.dublinwalkdb = dublinwalkdb;
        }


        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await dublinwalkdb.Regions.ToListAsync();
        }
    }
}
