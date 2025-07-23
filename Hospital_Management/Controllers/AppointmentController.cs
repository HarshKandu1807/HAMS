using Hospital_Management.Models.DTOS;
using Hospital_Management.Services.Iservice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly Iappointment iappointment;
        public AppointmentController(Iappointment iappointment)
        {
            this.iappointment = iappointment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointment()
        {
            var data = await iappointment.GetAppointments();
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpGet("patients/{id}")]
        public async Task<IActionResult> GetAppointmentPatient(int id)
        {
            var data = await iappointment.GetAppointmentPatient(id);
            return Ok(data);
        }

        [HttpGet("docters/{id}")]
        public async Task<IActionResult> GetAppointmentDoctor(int id)
        {
            var data = await iappointment.GetAppointmentDoctor(id);
            return Ok(data);
        }

        [HttpGet("{dateTime}")]
        public async Task<IActionResult> GetAppointmentDay(DateTime dateTime)
        {
            var data = await iappointment.GetAppointmentDay(dateTime);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddAppointment(AppointmentDTO appointmentDTO)
        {
            var data = await iappointment.AddAppointment(appointmentDTO);
            if (data== "Appointment Created Successfully.")
            {
                return Ok(data);
            }
            return BadRequest(data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAppointment(UpdateAppointmentDTO appointmentDTO, int id)
        {
            var data = await iappointment.UpdateAppointment(appointmentDTO,id);
            if(data== "Appointment Rescheduled Successfully")
            {
                return Ok(data);
            }
            return BadRequest(data);
        }

        [HttpDelete("cancel/{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var data = await iappointment.DeleteAppointment(id);
            return Ok(data);
        }

        [HttpDelete("complete/{id}")]
        public async Task<IActionResult> CompleteAppointment(int id)
        {
            var data = await iappointment.CompleteAppointment(id);
            return Ok(data);
        }
    }
}
