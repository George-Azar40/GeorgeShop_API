using GeorgeShop.DAL.DTO.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GeorgeShop.BLL.Extensions
{
    public static class PaginationExtensions
    {
        public static async Task<PaginationResponse<T>> ToPaginationAsync<T>
            (
            this IQueryable<T> query,
            int page, int limit
            )
        {
            var totalCount = await query.CountAsync();
            var data = await query.Skip( (page - 1) * limit ).Take(limit).ToListAsync();
            /*
             page   limit   skip
             1       5       0
             2       5       5
             3       5       10
             */
            return new PaginationResponse<T>
            {
                Data = data,
                TotalCount = totalCount,
                Page = page,
                Limit = limit
            };
        }
            
       


    }
}
