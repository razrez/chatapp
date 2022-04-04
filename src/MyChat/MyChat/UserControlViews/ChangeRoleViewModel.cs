using Microsoft.AspNetCore.Identity;

namespace MyChat.UserControlViews
{
    public class ChangeRoleViewModel
    {
        public string UserId { get; set; }
        public string UserLogin { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }
        public ChangeRoleViewModel()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
        //Эта модель позволит управлять всеми ролями для одного пользователя    
    }
}
