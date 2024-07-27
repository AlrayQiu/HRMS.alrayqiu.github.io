using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS_Application.DataBase
{
    public class MemberGroup
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public User User { get; set; } = null!;
        public Group Group { get; set; } = null!;
    }
}
