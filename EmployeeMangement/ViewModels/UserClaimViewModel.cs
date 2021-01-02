using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.ViewModels
{
    public class UserClaimViewModel
    {
        public string UserId { get; set; }
        public List<UserClaim> Claims { get; set; }
        public UserClaimViewModel()
        {
            this.Claims = new List<UserClaim>();
        }
    }
}
