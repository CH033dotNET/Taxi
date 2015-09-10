using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class Pager<T> where T : class
    {
        public List<T> Data { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }
}
