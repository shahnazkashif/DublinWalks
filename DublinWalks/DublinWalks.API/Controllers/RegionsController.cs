using AutoMapper;
using DublinWalks.API.Modals.Domain;
using DublinWalks.API.Modals.DTO;
using DublinWalks.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DublinWalks.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
                

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllAsync();

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

           var regionsDTO = mapper.Map<List<Modals.DTO.Region>>(regions);
            return Ok(regionsDTO);

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
        public async Task<IActionResult> AddRegionAsync(Modals.DTO.AddRegionRequest addRegionRequest)
        {
            //Request(DTO) to Domain modal
            var region = new Modals.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population
            };

            //Pass detail to repository
            region = await regionRepository.AddAsync(region);

            //Convert back to DTO
            var regionDTO = new Modals.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
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
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };

            //return OK Response
            return Ok(regionDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id,
            [FromBody] Modals.DTO.UpdateRegionRequest updateRegionRequest )
        {

            //Convert DTO to domain modal
            var region = new Modals.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population
            };

            //update region using Repository
            region = await regionRepository.UpdateAsync(id,region);


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
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };

            //Return OK Response
            return Ok(regionDTO);
        }

        

    }
}
