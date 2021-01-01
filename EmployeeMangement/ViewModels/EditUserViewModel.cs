using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.ViewModels
{
    public class EditUserViewModel 
    {
        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string City { get; set; }
        [Required][EmailAddress]
        public string Email { get; set; }

        public EditUserViewModel()
        {
            Roles = new List<string>();
            Claims = new List<string>();
        }
    }
}
