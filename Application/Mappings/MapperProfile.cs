using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<PlantRequestDTO, PlantRequest>();
            CreateMap<PlantRequest, PlantRequestViewDTO>();
            CreateMap<CreateSubscriptionDTO, Subscription>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "active"))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<PlantRequestDTO, PlantRequest>();

            CreateMap<PlantRequest, PlantRequestViewDTO>();

            CreateMap<PlantRequest, Plant>();

            CreateMap<Plant, PlantDTO>();
            CreateMap<Address, AddressDTO>();
            CreateMap<CreateAddressDTO, Address>();
            CreateMap<PlaceRentalRequestDto, Rental>();
            CreateMap<PlaceRentalItemDto, RentalItem>();

        }
    }
}
