using Hospital_Management.Models.DTOS;
using Hospital_Management.Services.Iservice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartment department;
        public DepartmentController(IDepartment department)
        {
            this.department = department;
        }

        [Authorize(Roles = "Admin,Doctor,Receptionist")]
        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var data = await department.GetDepartments();
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [Authorize(Roles = "Admin,Doctor,Receptionist")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var data = await department.GetDepartmentById(id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddDepartment(DepartmentDTO departmentDTO)
        {
            var data = await department.AddDepartment(departmentDTO);
            if (data == null)
            {
                return Conflict();
            }
            return Ok(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateDepartment(DepartmentDTO departmentDTO, int id)
        {
            var data = await department.UpdateDepartment(departmentDTO, id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var data = await department.DeleteDepartment(id);
            if (data == false)
            {
                return NotFound();
            }
            return Ok(data);
        }
    }
}
