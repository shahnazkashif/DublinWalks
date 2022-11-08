using DublinWalks.API.Data;
using DublinWalks.API.Modals.Domain;
using Microsoft.EntityFrameworkCore;

namespace DublinWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        //here DublinDbContext will be used for to talk with the WalkDifficulty table and for using its resources directly
        private readonly DublinDbContext dublinwalkdb;

        public WalkDifficultyRepository(DublinDbContext dublinwalkdb)
        {
            this.dublinwalkdb = dublinwalkdb;                      
        }


        //Add Walkdifficulty
     

        public async Task<Modals.Domain.WalkDifficulty> AddAsync(Modals.Domain.WalkDifficulty walkDifficulty)
        {
            //Assign new id
            walkDifficulty.Id = Guid.NewGuid();
            await dublinwalkdb.WalkDifficulty.AddAsync(walkDifficulty);
            await dublinwalkdb.SaveChangesAsync();

            return walkDifficulty;
        }

        //getting All Walkdifficulties
        public async Task<IEnumerable<Modals.Domain.WalkDifficulty>> GetAllAsync()
        {
            return await dublinwalkdb.WalkDifficulty.ToListAsync();
        }
        
        //Get WalkDifficultyById
       public async Task<Modals.Domain.WalkDifficulty> GetAsync(Guid id)
        {
            return await dublinwalkdb.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Modals.Domain.WalkDifficulty> UpdateAsync(Guid id,
                    Modals.Domain.WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty = await dublinwalkdb.WalkDifficulty.FindAsync(id);

            if (existingWalkDifficulty != null)
            {
                existingWalkDifficulty.Code = walkDifficulty.Code;

                await dublinwalkdb.SaveChangesAsync();
                return existingWalkDifficulty;
            }

            return null;
        }

        public async Task<Modals.Domain.WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingWalkDifficulty = await dublinwalkdb.WalkDifficulty.FindAsync(id);

            if (existingWalkDifficulty == null)
                return null;

            dublinwalkdb.WalkDifficulty.Remove(existingWalkDifficulty);
            await dublinwalkdb.SaveChangesAsync();
            return existingWalkDifficulty;
        }


    }
}
