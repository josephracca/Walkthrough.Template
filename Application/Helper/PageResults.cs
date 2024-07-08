using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public class PageResults<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalData { get; private set; }

        public PageResults(IEnumerable<T> items, int count, int pageIndex, int itemsPerPage)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)itemsPerPage);
            TotalData = count;
            this.AddRange(items);
        }

        public static PageResults<T> CreateList(IList<T> source, int pageIndex, int itemsPerPage)
        {
            var count = source.Count;

            var items = source.Skip((pageIndex - 1) * itemsPerPage).Take(itemsPerPage).ToList();
            return new PageResults<T>(items, count, pageIndex, itemsPerPage);
        }
    }
}
