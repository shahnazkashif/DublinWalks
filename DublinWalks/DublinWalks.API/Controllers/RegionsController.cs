using AutoMapper;
using DublinWalks.API.Modals.Domain;
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
        public async Task<IActionResult> GetAllRegions()
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
    }
}
