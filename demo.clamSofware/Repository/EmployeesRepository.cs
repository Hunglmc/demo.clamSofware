using demo.clamSofware.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace demo.clamSofware.Models
{
    public class EmployeesRepository: IEmployeesRepository
    {
        private readonly ILogger logger;
        private readonly AppDbContextContext context;

        public EmployeesRepository(AppDbContextContext context, ILogger<EmployeesRepository> logger)
        {
            this.logger = logger;
            this.context = context;
        }


        public Employees Add(Employees employee)
        {
            context.Employees.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public Employees Delete(int id)
        {
            Employees employee = context.Employees.Find(id);
            if (employee != null)
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
            }
            return employee;

        }

        public IEnumerable<Employees> GetAllEmployees()
        {
            return context.Employees;
        }

        public Employees GetEmployees(int id)
        {
            return context.Employees.Find(id);
        }

        public Employees Update(Employees updateEmployee)
        {

            var employee = context.Employees.Attach(updateEmployee);

            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            context.SaveChanges();

            return updateEmployee;

        }
    }
}
