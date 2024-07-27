using HRMS_Application.DataBase;

namespace HRMS_Application.Models
{
    public class UserModel
    {
        internal static UserModel? CurrentUser;
        public required int Id { get; init; }
        public required string UserName { get; init; }
        public required string Mac { get; init; }
        public long Time(AppDbContext context)  => context.GetTime(Id);

        public List<ReversoModel> GetReversoList(AppDbContext context)
        {
            var list = new List<ReversoModel>();
            foreach(var i in ManageredGroup)
            {
                list.AddRange(i.GetReverso(context));
            }
            return list;
        }

        public required List<GroupModel> ManageredGroup { get; set;  } 
    }
}
