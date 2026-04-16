using GeorgeShop.DAL.DTO.Request;
using GeorgeShop.DAL.DTO.Response;
using GeorgeShop.DAL.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.BLL.Mapping
{
    public static class MapsterConfig
    {
        public static void MapsterConfigRegister()
        {
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.Category_id, source => source.Id)
                .Map(dest => dest.UserCreated, source => source.CreatedBy.UserName)
                .Map(dest => dest.Name, source => source.Translations
                .Where(t => t.Language == CultureInfo.CurrentCulture.Name).Select(t=> t.Name).FirstOrDefault()
                );



            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
             .Map(dest => dest.UserCreated, source => source.CreatedBy.UserName)
             .Map(dest => dest.Name, source => source.Translations
             .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
                .Select(t => t.Name).FirstOrDefault()
             ).Map(dest => dest.MainImage, source => $"https://localhost:7053/images/{source.MainImage}")

             //Mapping/MapsterCongig
             .Map(dest => dest.BrandName, source => source.Brand.Name)
             .Map(dest => dest.BrandImage, source => $"https://localhost:7053/images/{source.Brand.BrandImage}");


            TypeAdapterConfig<ProductUpdateRequest, Product>.NewConfig()
                .IgnoreNullValues(true);

            TypeAdapterConfig<Brand, BrandResponse>.NewConfig()
                .Map(dest => dest.BrandImage, source => $"https://localhost:7053/images/{source.BrandImage}");


            TypeAdapterConfig<Cart, CartResponse>.NewConfig()
                .Map(dest => dest.ProductName, source => source.Product.Translations
                    .Where(t => t.Language == CultureInfo.CurrentCulture.Name).Select(t=>t.Name)
                        .FirstOrDefault())
                .Map(dest => dest.Price, source => source.Product.Price)
                .Map(dest => dest.ProductImage, source => $"https://localhost:7053/images/{source.Product.MainImage}")

                ;

        }
    }
}
