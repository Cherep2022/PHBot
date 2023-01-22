using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZennoPosterProject1.DataBase.Tables
{
    [Table("FriendNotAccepted")]
    public class FriendNotAccepted
    {
        [Key]
        [Index]
        public int Id { get; set; }
        [MaxLength(50)]
        public string UserName { get; set; }
    }
}
