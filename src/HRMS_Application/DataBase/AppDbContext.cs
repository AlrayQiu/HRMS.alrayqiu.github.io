using Azure;
using HRMS_Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace HRMS_Application.DataBase
{
    public class AppDbContext : DbContext
    {
        DbSet<User> HRMS_Users { get; set; }
        DbSet<Group> HRMS_Group { get; set; }
        DbSet<ManagerGroup> HRMS_ManagerGroupUser { get; set; }
        DbSet<MemberGroup> HRMS_MemberGroupUser { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            for (var j = 0; j < HRMS_ManagerGroupUser.Count(); j++)
            {
                var i = HRMS_ManagerGroupUser.ElementAt(j);
                i.User = HRMS_Users.FirstOrDefault(u => u.Id == i.UserId);
                i.Group = HRMS_Group.FirstOrDefault(u => u.Id == i.GroupId);
            }
            for (var j = 0; j < HRMS_MemberGroupUser.Count(); j++)
            {
                var i = HRMS_MemberGroupUser.ElementAt(j);
                i.User = HRMS_Users.FirstOrDefault(u => u.Id == i.UserId);
                i.Group = HRMS_Group.FirstOrDefault(u => u.Id == i.GroupId);
            }

            /*
              

            Database.Migrate();
            HRMS_Users.First().ManagedGroups.Add(HRMS_Group.First());
            var user = new User() {Id = 1, Password = "123456", Username = "alray" ,Mac = [0xF4,0x7B ,0x09 ,0x5D ,0x90 ,0x8A] };
            var group = new Group() { };
            group.Admins.Add(user);

            if(!HRMS_Users.Any(u=>u.Id == user.Id))
                HRMS_Users.Add(user);
            if (!HRMS_Group.Any(g => g.Id == group.Id))
                HRMS_Group.Add(group);
            */
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
          => options.UseSqlServer("Server=localhost;Database=app;Trusted_Connection=True;TrustServerCertificate=true;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().HasMany(s => s.Admins).WithMany(c => c.ManagedGroups).UsingEntity<ManagerGroup>(
                sc => sc.HasOne(e => e.User).WithMany(),
                sc => sc.HasOne(e => e.Group).WithMany()
                );

            modelBuilder.Entity<Group>().HasMany(s => s.Members).WithMany(c => c.MemberGroup).UsingEntity<MemberGroup>(
                sc => sc.HasOne(e => e.User).WithMany(),
                sc => sc.HasOne(e => e.Group).WithMany()
                );

            base.OnModelCreating(modelBuilder);
        }

        public bool VerifyUser(string userName, string passWord) => HRMS_Users.Any(u => u.Username == userName && u.Password == passWord);
        public string GetUserMac(string userName) => HRMS_Users.FirstOrDefault(u => u.Username == userName)?.Mac;
        public string[] GetMacs
        {
            get
            {
                List<string> strings = [];
                foreach (var i in HRMS_Users)
                    strings.Add(i.Mac);
                return [.. strings];
            }
        }

        public void AddTime(string mac,long timeInMinutes) => HRMS_Users.FirstOrDefault(u => u.Mac == mac).Time += 1;

        public void RemoveAdministor(int groupId,int userId) => HRMS_Group.FirstOrDefault(u => u.Id == groupId).Admins.Remove (HRMS_Users.FirstOrDefault(u=>u.Id == userId));

        public UserModel CreateUserModel(string name)
        {
            var tmp = HRMS_Users.FirstOrDefault(u => u.Username == name);
            List<GroupModel> groups = [];
            foreach(var g in tmp.ManagedGroups)
            {
                List<int> ms = [];
                foreach(var m in g.Members)
                    ms.Add(m.Id);
                groups.Add(new GroupModel() { Id = g.Id, Name = g.Name ,MemberUserId = ms.ToArray()});
            }
            return new UserModel() { Id = tmp.Id ,UserName = name,Mac = tmp.Mac,ManageredGroup  = groups};
        }

        public long GetTime(int id) => (long)HRMS_Users.FirstOrDefault(u => u.Id == id)?.Time;
        public string GetUserName(int id) => HRMS_Users.FirstOrDefault(u => u.Id == id)?.Username;
    }
}
