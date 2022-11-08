
using DublinWalks.API.Data;
using DublinWalks.API.Modals.Domain;
using Microsoft.EntityFrameworkCore;

namespace DublinWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        //here DublinDbContext will be used for to talk with the Walks table and for using its resources directly
        private readonly DublinDbContext dublinwalkdb;

        public WalkRepository(DublinDbContext dublinwalkdb)
        {
            this.dublinwalkdb = dublinwalkdb;
        }

        public async Task<Walk> AddAsync(Walk walk) 
        {
            //Assign new id
            walk.Id = Guid.NewGuid();
            await dublinwalkdb.Walks.AddAsync(walk);
            await dublinwalkdb.SaveChangesAsync();

            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await dublinwalkdb.Walks.FindAsync(id);

            if (existingWalk == null)
            {
                return null;
            }

            dublinwalkdb.Walks.Remove(existingWalk);
            await dublinwalkdb.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
           return await 
                dublinwalkdb.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync(); 
        }
        public async Task<Walk> GetAsync(Guid id)
        {
          return await  dublinwalkdb.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);  
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
           var existingWalk = await dublinwalkdb.Walks.FindAsync(id);

            if (existingWalk != null)
            { 
                existingWalk.Length = walk.Length;
                existingWalk.Name = walk.Name;
                existingWalk.RegionId = walk.RegionId;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                await dublinwalkdb.SaveChangesAsync();
                return existingWalk;    
            }

            return null;
        }
    }

}
