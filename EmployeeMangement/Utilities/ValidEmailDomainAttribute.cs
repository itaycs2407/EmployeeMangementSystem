using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.Utilities
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private string allowedDomain;

        public ValidEmailDomainAttribute(string allowedDomain)
        {
            this.allowedDomain = allowedDomain;
        }
        public override bool IsValid(object value)
        {
            return (value as string).ToLower().Contains(this.allowedDomain.ToLower());
        }
    }
}
