using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Http;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using jwtauth.dataaccess.IService;
using jwtauth.models;
using Microsoft.EntityFrameworkCore;

namespace jwtauth.api.Controllers
{
    [Authorize]
    [Route("api/employee/")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAllEmployees")]
        public async Task<List<Employee>> GetAllEmployees()
        {
            var employees = await Task.FromResult(_service.GetEmployeeDetails());
            return employees;
        }

        [HttpGet]
        public List<EmployeeInfo> Get()
        {
            var employeeInfo = GetEmployeeInfo();
            return employeeInfo;
        }

        [HttpGet]
        [Route("GetAllEmployees/{id}")]
        public async Task<ActionResult> GetAllEmployees(int id)
        {
            var employee = await Task.FromResult(_service.GetEmployeeDetails(id));
            if (employee == null)
            {
                return NotFound();
            }  
            return Ok(employee);
        }

        [HttpPost]
        [Route("AddEmployee")]
        public async Task<ActionResult<Employee>> AddEmployee(Employee request)
        {
            if(request != null)
            {
                _service.AddEmployee(request);
                return await Task.FromResult(request);
            }
            else
            {
                return await Task.FromResult(BadRequest());
            }
        }

        [HttpPut]
        [Route("UpdateEmployee/{id}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.EmployeeID)
            {
                return BadRequest();
            }
            try
            {
                _service.UpdateEmployee(employee);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return await Task.FromResult(employee);
        }

        [HttpDelete]
        [Route("DeleteEmployee/{id}")]
        public async Task<ActionResult<Employee>> Delete(int id)
        {
            var employee = _service.DeleteEmployee(id);
            return await Task.FromResult(employee);
        }

        private bool EmployeeExists(int id)
        {
            return _service.CheckEmployee(id);
        }


        private List<EmployeeInfo> GetEmployeeInfo()
        {
            List<EmployeeInfo> employees = new List<EmployeeInfo>
            {
                new EmployeeInfo 
                {
                    Id = 55,
                    FirstName = "Bonnie",
                    LastName = "Eaton",
                    Gender = "Female",
                    Email = "bonnieeaton@skyplex.com",
                    Age = 26,
                    IsActive =false,
                },
                new EmployeeInfo
                {
                    Id = 64,
                    FirstName = "Tania",
                    LastName = "Sellers",
                    Gender = "Female",
                    Email = "taniasellers@skyplex.com",
                    Age = 23,
                    IsActive =false
                },
                new EmployeeInfo
                {
                    Id = 63,
                    FirstName = "Esther",
                    LastName = "Mcmahon",
                    Gender = "Female",
                    Email = "esthermcmahon@skyplex.com",
                    Age = 25,
                    IsActive =true
                },
                new EmployeeInfo
                {
                    Id = 67,
                    FirstName = "Sparks",
                    LastName = "Sweet",
                    Gender = "Male",
                    Email = "sparkssweet@skyplex.com",
                    Age = 21,
                    IsActive =true
                },
                new EmployeeInfo
                {
                    Id = 68,
                    FirstName = "Clark",
                    LastName = "Cooley",
                    Gender = "Male",
                    Email = "clarkcooley@skyplex.com",
                    Age = 26,
                    IsActive =false
                },
                new EmployeeInfo
                {
                    Id = 69,
                    FirstName = "Jocelyn",
                    LastName = "Becks",
                    Gender = "Female",
                    Email = "jocelynbeck@skyplex.com",
                    Age = 24,
                    IsActive =false
                },
                new EmployeeInfo
                {
                    Id = 61,
                    FirstName = "Beatrice",
                    LastName = "Sullivan",
                    Gender = "Female",
                    Email = "beatricesullivan@skyplex.com",
                    Age = 23,
                    IsActive =false
                }
            };
            return employees;
        }

        public class EmployeeInfo
        {
            public long Id { get; set;}
            public string FirstName { get; set;}
            public string LastName { get; set;}
            public int Age { get; set;}
            public string Gender { get; set;}
            public string Email { get; set;}
            public Boolean IsActive { get; set;}
        }

    }
}