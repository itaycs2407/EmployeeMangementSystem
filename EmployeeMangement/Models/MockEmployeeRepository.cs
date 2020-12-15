using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee>  m_EmployeeList;

        public MockEmployeeRepository()
        {
            m_EmployeeList = new List<Employee>()
            {
                new Employee() {Id=1, Name = "Momo", Email = "momo@itay.com",Department= Department.HR},
                new Employee() {Id=2, Name = "Maya", Email = "maya@itay.com",Department= Department.IT},
                new Employee() {Id=3, Name = "Omri", Email = "omri@itay.com",Department= Department.Payroll}
            };
        }

        public Employee addEmployee(Employee employee)
        {
            employee.Id = m_EmployeeList.Max(employee => employee.Id) + 1;
            m_EmployeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employeeToDelete = this.m_EmployeeList.FirstOrDefault(e => e.Id == id);
            if (employeeToDelete!=null)
            {
                this.m_EmployeeList.Remove(employeeToDelete);
            }
            return employeeToDelete;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return this.m_EmployeeList;
        }

        public Employee GetEmployee(int i_Id)
        {
            return this.m_EmployeeList.FirstOrDefault(element => element.Id == i_Id);
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employeeToChange = this.m_EmployeeList.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (employeeToChange != null)
            {
                employeeToChange.Email = employeeChanges.Email;
                employeeToChange.Department = employeeChanges.Department;
                employeeToChange.Name = employeeChanges.Name;

            }
            return employeeToChange;
        }
    }
}
