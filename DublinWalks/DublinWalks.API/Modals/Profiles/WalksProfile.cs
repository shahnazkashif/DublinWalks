using AutoMapper;

namespace DublinWalks.API.Modals.Profiles
{
    public class WalksProfile : Profile
    {
        public WalksProfile()
        {
            CreateMap<Modals.Domain.Walk, Modals.DTO.Walk>()
              .ReverseMap();

            CreateMap<Modals.Domain.WalkDifficulty, Modals.DTO.WalkDifficulty>()
              .ReverseMap();
        }
    }
}
