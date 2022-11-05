using DublinWalks.API.Data;
using DublinWalks.API.Modals.Domain;
using Microsoft.EntityFrameworkCore;

namespace DublinWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly DublinDbContext ndublinwalkdb;

        public RegionRepository(DublinDbContext dublinwalkdb)
        {
            this.ndublinwalkdb = dublinwalkdb;
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await ndublinwalkdb.AddAsync(region);
            await ndublinwalkdb.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await ndublinwalkdb.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null)
            {
                return null;
            }

            //Delete the region
            ndublinwalkdb.Regions.Remove(region);
            await ndublinwalkdb.SaveChangesAsync();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await ndublinwalkdb.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
           var region = await ndublinwalkdb.Regions.FirstOrDefaultAsync(x => x.Id == id);
            return  region;
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await ndublinwalkdb.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRegion == null)
            {
                return null;
            }

            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.Area = region.Area;
            existingRegion.Lat = region.Lat;
            existingRegion.Long = region.Long;
            existingRegion.Population = region.Population;

           await  ndublinwalkdb.SaveChangesAsync();
            return existingRegion;  

        }
    }
}
