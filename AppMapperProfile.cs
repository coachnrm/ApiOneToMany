using AutoMapper;
using ApiCrud.Dtos;
using ApiCrud.Data.Entities;

namespace ApiCrud
{
    public class AppMapperProfile: Profile
    {
        public AppMapperProfile()
        {
            CreateMap<CustomerDto, Customer>();
            CreateMap<CustomerAddressesDto, CustomerAddresses>();
        }
    }
}