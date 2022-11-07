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

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
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

    }
}
