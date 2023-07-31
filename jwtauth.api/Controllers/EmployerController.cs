using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Http;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace jwtauth.api.Controllers
{
    [Authorize]
    [Route("api/employee/")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public EmployeeController()
        {
        }

        [HttpGet]
        [Route("GetAllEmployees")]
        public async Task<List<EmployeeInfo>> GetAllEmployees()
        {
            var employeeInfo = await Task.FromResult(GetEmployeeInfo());
            return employeeInfo;
        }

        [HttpGet]
        public List<EmployeeInfo> Get()
        {
            var employeeInfo = GetEmployeeInfo();
            return employeeInfo;
        }

        [HttpGet]
        [Route("GetAllEmployees/{id}")]
        public async Task<List<EmployeeInfo>> GetAllEmployees(long id)
        {
            var employeeInfo = await Task.FromResult(GetEmployeeInfo().Where(x => x.Id == id).ToList());
            return employeeInfo;
        }

        [HttpPost]
        [Route("AddEmployee")]
        public async Task<ActionResult<EmployeeInfo>> AddEmployee(EmployeeInfo request)
        {
            if(request != null)
            {
                List<EmployeeInfo> insertEmployee = new List<EmployeeInfo>();
                insertEmployee.Add(request);
                return await Task.FromResult(request);
            }
            else
            {
                return await Task.FromResult(BadRequest());
            }
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