using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace testVeljkara.DbLayer
{
    public class Link
    {
        [Key]
        public Guid Id { get; set; }
        public string LongLink { get; set; }
        public string ShortLink { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
