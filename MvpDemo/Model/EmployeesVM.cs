using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class EmployeesVM : IModel<Employees>
    {
        public IEnumerable<Employees> LoadAll()
        {
            using (var entities = new TestEntities())
            {
                return entities.Employees.ToList();
            }
        }

        public void AddEmployee(string name, decimal salary)
        {
            using (var entities = new TestEntities())
            {
                entities.Employees.Add(new Employees { Name = name, Salary = salary });
                entities.SaveChanges();
            }
        }
    }
}
