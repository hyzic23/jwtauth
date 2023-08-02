using System;
using jwtauth.dataaccess.Data;
using jwtauth.dataaccess.IService;
using jwtauth.models;

namespace jwtauth.dataaccess.Service
{
	public class EmployeeService : IEmployeeService
	{
        private readonly DatabaseContext _dbContext;
		public EmployeeService(DatabaseContext dbContext)
		{
            _dbContext = dbContext;
		}

        public void AddEmployee(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges();
        }

        public bool CheckEmployee(int id)
        {
            return _dbContext.Employees.Any(e => e.EmployeeID == id);
        }

        public Employee DeleteEmployee(int id)
        {
            Employee employee = _dbContext.Employees.Find(id);
            if (employee != null)
            {
                _dbContext.Employees.Remove(employee);
                _dbContext.SaveChanges();
                return employee;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public List<Employee> GetEmployeeDetails()
        {
            return _dbContext.Employees.ToList();
        }

        public Employee GetEmployeeDetails(int id)
        {
            return _dbContext.Employees.Find(id);
        }

        public void UpdateEmployee(Employee employee)
        {
            _dbContext.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}

