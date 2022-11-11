using AutoMapper;
using DublinWalks.API.Modals.DTO;
using DublinWalks.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DublinWalks.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultiessAsync()
        {
            //Fetch data from database - domain walks
            var walksDifficultyDomain = await walkDifficultyRepository.GetAllAsync();

            //convert domain to DTO 
            var walkDifficultyDTO = mapper.Map<List<Modals.DTO.WalkDifficulty>>(walksDifficultyDomain);

            return Ok(walkDifficultyDTO);
        }

        [HttpGet]
        [Route("{id=guid}")]
        [ActionName("GetWalkDifficultyById")]
        public async Task<IActionResult> GetWalkDifficultyById(Guid id)
        {
            //Get domain object from database
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);

            if (walkDifficulty == null) return NotFound();

            //convert domain object to DTO
            var walkDifficultyDTO = mapper.Map<Modals.DTO.WalkDifficulty>(walkDifficulty);

            //return response
            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync(Modals.DTO.AddWalkDifficultyRequest addwalkDifficultiRequest)
        {
            //validate the incoming request 
            if (!ValidateAddWalkDifficultyAsyn(addwalkDifficultiRequest))
            {
                return BadRequest(ModelState);
            }

            //convert DTO to Domain object 
            var walkDifficultyDomain = new Modals.Domain.WalkDifficulty
            {
                Code = addwalkDifficultiRequest.Code                
            };

            //pass domain object to repository to persist this
            await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

            //convert the domain object back to DTO
            var walkDifficultyDTO = mapper.Map<Modals.DTO.WalkDifficulty>(walkDifficultyDomain);

            //send DTO response back to client
            return CreatedAtAction(nameof(GetWalkDifficultyById), 
                new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id,
            [FromBody] Modals.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //validate the incoming request 
            if (!ValidateUpdateWalkDifficultyAsyn(updateWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }

            //convert DTO to Domain object
            var walkDifficultyDomain = new Modals.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code
            };

            //pass details to repository - Get Domain object in response (or null)
            walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);

            //Handle Null (not found)
            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }

            //convert back Domain to DTO
            var walkDifficultyDTO = mapper.Map<Modals.DTO.WalkDifficulty>(walkDifficultyDomain);


            //Return Response
            return Ok(walkDifficultyDTO);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            var walkDifficultyDomain = await walkDifficultyRepository.DeleteAsync(id);

            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = mapper.Map<Modals.DTO.WalkDifficulty>(walkDifficultyDomain);
            return Ok(walkDifficultyDTO);
        }

        #region Private Methods

        private bool ValidateAddWalkDifficultyAsyn(Modals.DTO.AddWalkDifficultyRequest addwalkDifficultiRequest)
        {
            if (addwalkDifficultiRequest == null)
            {
                ModelState.AddModelError(nameof(addwalkDifficultiRequest),
                   $"{nameof(addwalkDifficultiRequest)} cannot be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addwalkDifficultiRequest.Code))
            {
                ModelState.AddModelError(nameof(addwalkDifficultiRequest.Code),
                    $"{nameof(addwalkDifficultiRequest.Code)} is required.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private bool ValidateUpdateWalkDifficultyAsyn(Modals.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest),
                   $"{nameof(updateWalkDifficultyRequest)} cannot be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code),
                    $"{nameof(updateWalkDifficultyRequest.Code)} is required.");
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
