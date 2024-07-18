using AutoMapper;
using DublinWalks.API.CustomActionFilters;
using DublinWalks.API.Modals.Domain;
using DublinWalks.API.Modals.DTO;
using DublinWalks.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DublinWalks.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper,
                                  ILogger<RegionsController> logger)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }                

        [HttpGet]
       // [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            try
            {
               // throw new Exception("This is a custome exception");

                var regions = await regionRepository.GetAllAsync();

                var regionsDTO = mapper.Map<List<Modals.DTO.Region>>(regions);

               // logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regionsDTO)}");

                return Ok(regionsDTO);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
            

            //return DTO regions
            //var regionsDTO = new List<Modals.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Modals.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Name = region.Name,
            //        Code = region.Code,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population,
            //    };
            //    regionsDTO.Add(regionDTO);
            //});
                       

            //var regions = new List<Region>()
            //{
            //    new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name="testRegion1",
            //        Code="Region2",
            //        Area=123,
            //        Lat = 123,
            //        Long = 123,
            //        Population = 500000
            //    },
            //    new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name="testRegion2",
            //        Code="Region2",
            //        Area=123,
            //        Lat = 123,
            //        Long = 123,
            //        Population = 500000
            //    }
            //};

            //Ok is 200 Success response

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
      //  [Authorize(Roles = "Reader")]
        //restricting the route to accept only guid type values not int type or any
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);
            if (region == null)
            {
                return NotFound();  
            }
            var regionDTO = mapper.Map<Modals.DTO.Region>(region);
            return Ok(regionDTO);
        }

        [HttpPost]
       // [ValidateModal]
       // [Authorize(Roles = "Writer")]
        public async Task<IActionResult> AddRegionAsync(Modals.DTO.AddRegionRequest addRegionRequest)
        {
            //validate the request 
            // if (!ValidateAddRegionAsync(addRegionRequest))
            // {
            //   return BadRequest(ModelState);    
            // }
            //Request(DTO) to Domain modal           

            var region = new Modals.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                RegionImageUrl = addRegionRequest.RegionImageUrl

            };

            //Pass detail to repository
            region = await regionRepository.AddAsync(region);

            //Convert back to DTO
            var regionDTO = new Modals.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);            
        }

        [HttpDelete]
        [Route("{id:guid}")]
       // [Authorize(Roles = "Writer, Reader")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get region from Database
            var region = await regionRepository.DeleteAsync(id);

            // if Null notFound
            if (region == null)
            {
                return NotFound();
            }

            //Convert reponse back to DTO
            //Convert back to DTO
            var regionDTO = new Modals.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };

            //return OK Response
            return Ok(regionDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModal]
      //  [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id,
            [FromBody] Modals.DTO.UpdateRegionRequest updateRegionRequest )
        {
            
                //Convert DTO to domain modal
                var region = new Modals.Domain.Region()
                {
                    Code = updateRegionRequest.Code,
                    Name = updateRegionRequest.Name,
                    RegionImageUrl = updateRegionRequest.RegionImageUrl
                };

                //update region using Repository
                region = await regionRepository.UpdateAsync(id, region);


                //if null then NotFound
                if (region == null)
                {
                    return NotFound();
                }


                //Convert Domain back to DTO
                var regionDTO = new Modals.DTO.Region()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name                    
                };

                //Return OK Response
                return Ok(regionDTO);           
        }


        #region private methods
        private bool ValidateAddRegionAsync(Modals.DTO.AddRegionRequest addRegionRequest)
        {
            if (addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest),
                   $"Add Region Data is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code),
                    $"{nameof(addRegionRequest.Code)} cannot be null or empty or whitespace.");  
            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name),
                    $"{nameof(addRegionRequest.Name)} cannot be null or empty or whitespace.");
            }

            //if (addRegionRequest.Area <=0 )
            //{
            //    ModelState.AddModelError(nameof(addRegionRequest.Area),
            //        $"{nameof(addRegionRequest.Area)} cannot be less than or equal to zero.");
            //}

            //if (addRegionRequest.Lat <= 0)
            //{
            //    ModelState.AddModelError(nameof(addRegionRequest.Lat),
            //        $"{nameof(addRegionRequest.Lat)} cannot be less than or equal to zero.");
            //}

            //if (addRegionRequest.Long <= 0)
            //{
            //    ModelState.AddModelError(nameof(addRegionRequest.Long),
            //        $"{nameof(addRegionRequest.Long)} cannot be less than or equal to zero.");
            //}
            //if (addRegionRequest.Population < 0)
            //{
            //    ModelState.AddModelError(nameof(addRegionRequest.Population),
            //        $"{nameof(addRegionRequest.Population)} cannot be less than zero.");
            //}

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        #endregion  


    }
}
