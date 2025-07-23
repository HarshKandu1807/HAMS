using Hospital_Management.Models.DTOS;
using Hospital_Management.Services.Iservice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatient patient;
        public PatientController(IPatient patient)
        {
            this.patient = patient;
        }

        [Authorize(Roles = "Admin,Doctor,Receptionist")]
        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            var data = await patient.GetPatients();
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(data);
            }
        }

        [Authorize(Roles = "Admin,Doctor,Receptionist,Patient")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var data = await patient.GetPatientById(id);
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(data);
;            }
        }

        [Authorize(Roles = "Admin,Doctor,Receptionist,Patient")]
        [HttpGet("record/{id}")]
        public async Task<IActionResult> GetPatientRecord(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var data = await patient.GetPatientRecords(id);
            if (data == null)
            {
                return BadRequest("Data Does Not Exist");
            }
            return Ok(data);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPost]
        public async Task<IActionResult> AddPatient(PatientDTO patientDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var data = await patient.AddPatient(patientDTO);
            if (data == null)
            {
                return BadRequest("Data Already Exist");
            }
            return Ok(data);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPut]
        public async Task<IActionResult> UpdatePatient(PatientDTO patientDTO,int id)
        {
            var data = await patient.UpdatePatient(patientDTO, id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpDelete]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var data = await patient.DeletePatient(id);
            if (!data)
            {
                return NotFound();
            }
            return Ok(data);
        }
    }
}
