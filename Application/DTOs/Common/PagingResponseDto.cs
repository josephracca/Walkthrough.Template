using Application.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Common
{
    public class PagingResponseDto<T>
    {
        public PageResults<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalData { get; set; }

        public PagingResponseDto(PageResults<T> items)
        {
            Items = items;
            PageIndex = items.PageIndex;
            TotalPages = items.TotalPages;
            TotalData = items.TotalData;
        }
    }
}
