using AutoMapper;
using HollypocketBackend.Models;
using HollypocketBackend.Models.Product;
using HollypocketBackend.Models.Provider;

namespace HollypocketBackend.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductModel, Product>();
            CreateMap<UpdateProductModel, Product>();
            CreateMap<Provider, UpdateProviderModel>();
            CreateMap<UpdateProviderModel, Provider>();
            CreateMap<CreateProviderModel, Provider>();

            CreateMap<QuestionDto, Question>();
        }
    }
}