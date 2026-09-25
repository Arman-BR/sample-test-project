using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace SampleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityManagementController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public IdentityManagementController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        [HttpGet]
        public string Get()
        {
            return "Api running fine.....";
        }

        [HttpGet("GetSecret/{key}")]
        public string GetSecret(string key)
        {
            string? keyData = string.Empty;
            keyData = configuration.GetSection(key).Value;
            return keyData;
        }

        [HttpGet("GetEmployees")]
        public IActionResult GetEmployees()
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                SqlConnection con = new SqlConnection(configuration.GetSection("ConnectionString").Value);
                con.Open();
                SqlCommand command = new SqlCommand("select * from Employees", con);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            Id = reader.GetInt64(0),
                            Name = reader.GetString(1),
                            Mobile = reader.GetString(3),
                            EmpCode = reader.GetString(2),
                        });
                    }
                  
                }
            }
            catch (Exception ex)
            {
                return NotFound("Some error occured - "+ex.ToString());
            }
            return Ok(employees);
        }
    }
}
