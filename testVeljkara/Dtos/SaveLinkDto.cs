using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testVeljkara.Dtos
{
    public class SaveLinkDto
    {
        public Guid Id { get; set; }
        public string LongLink { get; set; }
        public string ShortLink { get; set; }
        public Guid UserId { get; set; }
    }
}
