using EntityFramework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindModel;

namespace EntityFramework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetEmployees()
        {
            NorthwindDbContext context = new NorthwindDbContext();
            IQueryable<EmployeeListModel> query = from e in context.Employees
                                                  select new EmployeeListModel()
                                                  {
                                                      FirstName = e.FirstName,
                                                      LastName = e.LastName,
                                                      HireDate = e.HireDate,
                                                      Title = e.Title
                                                  };
            List<EmployeeListModel> employees = query.ToList();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployee(int id)
        {
            NorthwindDbContext context = new NorthwindDbContext();
            IQueryable<EmployeeGetModel> query = from e in context.Employees
                                                 where e.EmployeeId == id
                                                 select new EmployeeGetModel()
                                                 {
                                                     Address = e.Address,
                                                     City = e.City,
                                                     Region = e.Region,
                                                     PostalCode = e.PostalCode,
                                                     Country = e.Country,
                                                     LastName = e.LastName,
                                                     FirstName = e.FirstName,
                                                     BirthDate = e.BirthDate,
                                                     EmployeeId = e.EmployeeId,
                                                     Title = e.Title,
                                                     Extension = e.Extension,
                                                     HireDate = e.HireDate,
                                                     HomePhone = e.HomePhone,
                                                     Notes = e.Notes,
                                                     Photo = e.Photo,
                                                     PhotoPath = e.PhotoPath,
                                                     ReportsTo = e.ReportsTo,
                                                     TitleOfCourtesy = e.TitleOfCourtesy,

                                                 };
            var employee = query.SingleOrDefault();
            return Ok(employee);
        }


        [HttpPost]
        public IActionResult AddEmployee(EmployeeCreateModel employeeModel)
        {
            Employee employee = new Employee();
            employee.FirstName = employeeModel.FirstName;
            employee.LastName = employeeModel.LastName;
            employee.Title = employeeModel.Title;
            employee.HireDate = employeeModel.HireDate;
            employee.ReportsTo = employeeModel.ReportsTo;
            NorthwindDbContext northwindDbContext = new NorthwindDbContext();
            northwindDbContext.Employees.Add(employee);
            northwindDbContext.SaveChanges();
            return Ok(employee);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                NorthwindDbContext northwindDbContext = new NorthwindDbContext();
                Employee employee = northwindDbContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
                if (employee == null)
                {
                    return NotFound();
                }
                northwindDbContext.Employees.Remove(employee);
                northwindDbContext.SaveChanges();
                return NoContent();
            }
            catch (Exception e)
            {

                throw new Exception("Bir hata oluştu.",e);
            }
            
        }


        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] EmployeeUpdateModel employeeUpdateModel)
        {
            NorthwindDbContext northwindDbContext = new NorthwindDbContext();
            if (employeeUpdateModel == null || id != employeeUpdateModel.EmployeeId)
            {
                return BadRequest();
            }

            var employee = northwindDbContext.Employees.Find(id);

            if (employee == null)
            {
                return NotFound();
            }

            
            employee.LastName = employeeUpdateModel.LastName;
            employee.FirstName = employeeUpdateModel.FirstName;
            employee.Title = employeeUpdateModel.Title;
            employee.TitleOfCourtesy = employeeUpdateModel.TitleOfCourtesy;
            employee.BirthDate = employeeUpdateModel.BirthDate;
            employee.HireDate = employeeUpdateModel.HireDate;
            employee.Address = employeeUpdateModel.Address;
            employee.City = employeeUpdateModel.City;
            employee.Region = employeeUpdateModel.Region;
            employee.PostalCode = employeeUpdateModel.PostalCode;
            employee.Country = employeeUpdateModel.Country;
            employee.HomePhone = employeeUpdateModel.HomePhone;
            employee.Extension = employeeUpdateModel.Extension;
            employee.Photo = employeeUpdateModel.Photo;
            employee.Notes = employeeUpdateModel.Notes;
            employee.ReportsTo = employeeUpdateModel.ReportsTo;
            employee.PhotoPath = employeeUpdateModel.PhotoPath;


            northwindDbContext.SaveChanges();
            return Ok(employee);

        }

        [HttpGet("{id}/orders")]
        public IActionResult GetEmployeeOrders(int id)
        {
            NorthwindDbContext northwindDbContext = new NorthwindDbContext();
            try
            {
                var employee = northwindDbContext.Employees
                    .Include(e => e.Orders)
                    .ThenInclude(o=>o.OrderDetails)
                    .FirstOrDefault(e => e.EmployeeId == id);

                if (employee == null)
                {
                    return NotFound();
                }

                var orders = employee.Orders.Select(o => new
                {
                    OrderId = o.OrderId,
                    CustomerId = o.CustomerId,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.OrderDetails.Sum(od => od.Quantity * od.UnitPrice)
                }).ToList();

                return Ok(orders);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error");
            }
        }

    }
}

