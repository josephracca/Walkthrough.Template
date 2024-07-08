using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Common
{
    public abstract class PagingDto
    {
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
