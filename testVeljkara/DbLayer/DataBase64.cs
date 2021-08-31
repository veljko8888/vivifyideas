using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace testVeljkara.DbLayer
{
    public class DataBase64
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public string Side { get; set; }
    }
}
