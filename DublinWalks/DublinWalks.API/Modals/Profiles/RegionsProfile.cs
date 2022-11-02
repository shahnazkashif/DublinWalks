using AutoMapper;

namespace DublinWalks.API.Modals.Profiles
{
    public class RegionsProfile: Profile
    {
        public RegionsProfile()
        {
            CreateMap<Modals.Domain.Region, Modals.DTO.Region>()
                .ReverseMap();
        }
    }
}
