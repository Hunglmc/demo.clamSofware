using demo.clamSofware.Models;
using System.Collections.Generic;

namespace demo.clamSofware.Interface
{
    public interface IEmployeesRepository
    {

        Employees GetEmployees(int id);
        IEnumerable<Employees> GetAllEmployees();
        Employees Add(Employees employees);
        Employees Update(Employees updateEmployees);
        Employees Delete(int id);

    }
}
