using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace testVeljkara.DbLayer
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
        [Column(TypeName = "nvarchar(350)")]
        public byte[] PasswordHash { get; set; }

        [Column(TypeName = "nvarchar(350)")]
        public byte[] PasswordSalt { get; set; }
    }
}
