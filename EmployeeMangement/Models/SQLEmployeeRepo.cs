using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.Models
{
    public class SQLEmployeeRepo : IEmployeeRepository
    {
        private readonly AppDBContext m_Context;

        public SQLEmployeeRepo(AppDBContext context)
        {
            this.m_Context = context;
        }
        public Employee addEmployee(Employee employee)
        {
            this.m_Context.Employees.Add(employee);
            this.m_Context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = this.m_Context.Employees.Find(id);
            if(employee != null)
            {
                this.m_Context.Remove(employee);
                this.m_Context.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return m_Context.Employees;
        }

        public Employee GetEmployee(int id)
        {
           return this.m_Context.Employees.Find(id);
        }

        public Employee Update(Employee employeeChanges)
        {
            // holds the changes to var and than change it state to modified;
            var employee = this.m_Context.Employees.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.m_Context.SaveChanges();
            return employeeChanges;
        }
    }
}
