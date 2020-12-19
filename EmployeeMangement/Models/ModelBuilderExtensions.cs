using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "momoMOMO",
                    Department = Department.IT,
                    Email = "momoMOMO@momo.om"
                },
                new Employee
                {
                    Id = 2,
                    Name = "m",
                    Department = Department.HR,
                    Email = "mO@momo.om"
                });
        }
    }
}
