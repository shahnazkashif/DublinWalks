
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

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
               string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            //return await 
            //     dublinwalkdb.Walks
            //     .Include(x => x.Region)
            //     .Include(x => x. WalkDifficulty)
            //     .ToListAsync();
            // return await dublinwalkdb.Walks.ToListAsync();
            var walks = dublinwalkdb.Walks.Include("Difficulty").Include("Region").AsQueryable();
            //Filtering
           if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase ) )
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }                
            }

            //Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            //pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync(); 
        }
        public async Task<Walk> GetAsync(Guid id)
        {
            return await dublinwalkdb.Walks
                  .Include(x => x.RegionId)
                  .Include(x => x.DifficultyId)
                  .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await dublinwalkdb.Walks.FindAsync(id);

            if (existingWalk != null)
            {
                existingWalk.LengthInKm = walk.LengthInKm;
                existingWalk.Name = walk.Name;
                existingWalk.RegionId = walk.RegionId;
                existingWalk.DifficultyId = walk.DifficultyId;
                await dublinwalkdb.SaveChangesAsync();
                return existingWalk;
            }

            return null;
        }
    }

}
