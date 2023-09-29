using Application.Spellen.Commands.FinishedSpel;
using AutoMapper;

namespace WebUI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<FinishedSpelResultsDTO, FinishedSpelDTO>()
                .ForMember(dest => dest.WinnerName,
                    opt => opt.MapFrom(src => src.GameWonBy))
                .ForMember(dest => dest.LoserName,
                    opt => opt.MapFrom(src => src.GameLostBy))
                .ForMember(dest => dest.IsDraw,
                    opt => opt.MapFrom(src => src.IsDraw))
                .ForMember(dest => dest.IsWinner,
                    opt => opt.MapFrom(src => src.IsWinner))
                .ForMember(dest => dest.AmountOfWitFichesTurned,
                    opt => opt.MapFrom(src => src.AmountOfFichesFlippedByPlayer1))
                .ForMember(dest => dest.AmountOfZwartFichesTurned,
                    opt => opt.MapFrom(src => src.AmountOfFichesFlippedByPlayer2))
                .ForMember(dest => dest.AmountOfGeenFichesTurned,
                    opt => opt.MapFrom(src => 64 - src.AmountOfFichesFlippedByPlayer1 + src.AmountOfFichesFlippedByPlayer2));
        }
    }
}
