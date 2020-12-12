using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.Models
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int i_Id);

        IEnumerable<Employee> GetAllEmployee();
    }
}
