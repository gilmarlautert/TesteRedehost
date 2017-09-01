using AutoMapper;
using ProjetoRedehost.Models;

namespace ProjetoRedehost.ViewModels.Mappings 
{
    public class ViewModelToEntityMappingProfile : Profile
    {
        public ViewModelToEntityMappingProfile()
        {
              CreateMap<RegistrationViewModel,ApplicationUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));
        }
    }
}