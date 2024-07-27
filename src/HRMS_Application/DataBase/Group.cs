using System.ComponentModel.DataAnnotations;

namespace HRMS_Application.DataBase
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public ICollection<User> Admins { get; set; } = [];
        public ICollection<User> Members { get; set; } = [];
    }
}
