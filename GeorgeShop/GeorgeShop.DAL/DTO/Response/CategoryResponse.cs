using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.DAL.DTO.Response
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public List<CategoryTranslationResponse> Translations { get; set; }
    }
}
