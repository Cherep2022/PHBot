using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZennoPosterProject1.DataBase.Tables
{
    [Table("FriendsInContest")]
    public class FriendInContest
    {
        [Key]
        [Index]
        public int Id { get; set; }
        [MaxLength(150)]
        public string UserName { get; set; }
    }
}
