using Hospital_Management.Models.DTOS;
using Hospital_Management.Services.Iservice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctor doctor;
        public DoctorController(IDoctor doctor)
        {
            this.doctor = doctor;
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpGet]
        public async Task<IActionResult> GetDoctors()
        {
            var data = await doctor.GetDoctors();
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(data);
            }
        }

        [Authorize(Roles = "Admin,Doctor,Receptionist")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var data = await doctor.GetDoctorById(id);
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(data);
            }
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPost]
        public async Task<IActionResult> AddDoctor(DoctorDTO doctorDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var data = await doctor.AddDoctor(doctorDTO);
            if (data == null)
            {
                return BadRequest("Data Already Exist");
            }
            return Ok(data);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPost("leaves")]
        public async Task<IActionResult> AddDoctorLeave(DoctorLeaveDTO doctorLeaveDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var data = await doctor.AddDoctorLeave(doctorLeaveDTO);
            return Ok(data);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPut]
        public async Task<IActionResult> UpdateDoctor(DoctorDTO DoctorDTO, int id)
        {
            var data = await doctor.UpdateDoctor(DoctorDTO, id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpDelete]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var data = await doctor.DeleteDoctor(id);
            if (!data)
            {
                return NotFound();
            }
            return Ok(data);
        }
    }
}
