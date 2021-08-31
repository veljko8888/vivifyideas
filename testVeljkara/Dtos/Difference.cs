using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testVeljkara.Dtos
{
    public class Difference
    {
        public Difference(int offset, int length)
        {
            this.Offset = offset;
            this.Length = length;
        }
        public int Offset { get; set; }
        public int Length { get; set; }
    }
}
