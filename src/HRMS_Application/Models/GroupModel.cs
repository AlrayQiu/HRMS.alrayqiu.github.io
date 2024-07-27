using HRMS_Application.DataBase;

namespace HRMS_Application.Models
{
    public class GroupModel
    {

        public required int Id{ get; init; }
        public required string Name { get; init; }

        public required int[] MemberUserId { get; set; } = [];

        public  void RemoveAdministor(int userId, AppDbContext context) => context.RemoveAdministor(Id, userId);
        public List<ReversoModel> GetReverso(AppDbContext context)
        {
            var list = new List<ReversoModel>();
            foreach (var i in MemberUserId)
            {
                list.Add(new ReversoModel() { Id = i,Time = context .GetTime(i),UserName = context.GetUserName(i)});
            }
            return list;
        }
    }
}
