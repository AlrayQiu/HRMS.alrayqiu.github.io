using System.ComponentModel.DataAnnotations;

namespace HRMS_Application.DataBase
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Mac { get; set; } = "";
        public long Time { get; set; } =0;


        public ICollection<Group> ManagedGroups { get; set; } = [];
        public ICollection<Group> MemberGroup { get; set; } = [];
    }
}
