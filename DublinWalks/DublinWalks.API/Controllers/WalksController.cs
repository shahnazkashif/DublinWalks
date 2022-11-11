using AutoMapper;
using DublinWalks.API.Modals.Domain;
using DublinWalks.API.Modals.DTO;
using DublinWalks.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DublinWalks.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //Fetch data from database - domain walks
            var walksDomain = await walkRepository.GetAllAsync();

            //convert domain walk to DTO walks
            var walksDTO = mapper.Map<List<Modals.DTO.Walk>>(walksDomain);

            //return reponse
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id=guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //Get walk domain object from database
            var walkDomain = await walkRepository.GetAsync(id);

            //convert domain object to DTO
            var walkDTO = mapper.Map<Modals.DTO.Walk>(walkDomain);


            //return response
            return Ok(walkDTO);
        }


        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Modals.DTO.AddWalkRequest addwalkrequest)
        {
            //validate the incoming request 
            if (!(await ValidateAddWalkAsync(addwalkrequest)))
            {
                return BadRequest(ModelState);
            }

            //convert DTO to Domain object 
            var walkDomain = new Modals.Domain.Walk
            {
                Name = addwalkrequest.Name,
                Length = addwalkrequest.Length,
                RegionId = addwalkrequest.RegionId,
                WalkDifficultyId = addwalkrequest.WalkDifficultyId
            };

            //pass domain object to repository to persist this
            await walkRepository.AddAsync(walkDomain);

            //convert the domain object back to DTO
            var walkDTO = new Modals.DTO.Walk
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            //send DTO response back to client
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id,
            [FromBody] Modals.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //validate the incoming request 
            if (!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }

            //convert DTO to Domain object
            var walkDomain = new Modals.Domain.Walk
            {
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };

            //pass details to repository - Get Domain object in response (or null)
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            //Handle Null (not found)
            if (walkDomain == null)
            {
                return NotFound();
            }

            //convert back Domain to DTO
            var walkDTO = new Modals.DTO.Walk
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };


            //Return Response
            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
           var walkDomain = await  walkRepository.DeleteAsync (id);    

            if (walkDomain == null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<Modals.DTO.Walk>(walkDomain);
            return Ok(walkDTO); 
        }

        #region Private Methods
        private async Task<bool> ValidateAddWalkAsync(Modals.DTO.AddWalkRequest addwalkrequest)
        {
            if (addwalkrequest == null)
            {
                ModelState.AddModelError(nameof(addwalkrequest),
                   $"{nameof(addwalkrequest)} cannot be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addwalkrequest.Name))
            {
                ModelState.AddModelError(nameof(addwalkrequest.Name),
                    $"{nameof(addwalkrequest.Name)} cannot be null or empty or whitespace.");
            }

            if (addwalkrequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(addwalkrequest.Length),
                    $"{nameof(addwalkrequest.Length)} should be greater than zero.");
            }

            var region = await regionRepository.GetAsync(addwalkrequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addwalkrequest.RegionId),
                   $"{nameof(addwalkrequest.RegionId)} is invalid.");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(addwalkrequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addwalkrequest.WalkDifficultyId),
                  $"{nameof(addwalkrequest.WalkDifficultyId)} is invalid.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(Modals.DTO.UpdateWalkRequest updatewalkrequest)
        {
            if (updatewalkrequest == null)
            {
                ModelState.AddModelError(nameof(updatewalkrequest),
                   $"{nameof(updatewalkrequest)} cannot be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updatewalkrequest.Name))
            {
                ModelState.AddModelError(nameof(updatewalkrequest.Name),
                    $"{nameof(updatewalkrequest.Name)} cannot be null or empty or whitespace.");
            }

            if (updatewalkrequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updatewalkrequest.Length),
                    $"{nameof(updatewalkrequest.Length)} should be greater than zero.");
            }

            var region = await regionRepository.GetAsync(updatewalkrequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updatewalkrequest.RegionId),
                   $"{nameof(updatewalkrequest.RegionId)} is invalid.");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(updatewalkrequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updatewalkrequest.WalkDifficultyId),
                  $"{nameof(updatewalkrequest.WalkDifficultyId)} is invalid.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        #endregion


    }
}
