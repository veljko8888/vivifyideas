using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testVeljkara.Helpers;

namespace testVeljkara.Dtos
{
    public class DiffDto
    {
        public string DiffResultType { get; set; }
        public List<Difference> Differences { get; set; }
    }
}
