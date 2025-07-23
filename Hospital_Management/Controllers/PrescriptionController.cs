using Hospital_Management.Models.DTOS;
using Hospital_Management.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hospital_Management.Services.Iservice;
using Microsoft.AspNetCore.Authorization;

namespace Hospital_Management.Controllers
{
    [Route("api/prescription")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly Iprescription iprescription;
        public PrescriptionController(Iprescription iprescription)
        {
            this.iprescription = iprescription;
        }

        [Authorize(Roles =("Admin,Doctor,Receptionist"))]
        [HttpGet]
        public async Task<IActionResult> GetPrescriptions()
        {
            var data = await iprescription.GetPrescription();
            return Ok(data);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPrescriptionById(int id)
        {
            var data = await iprescription.GetPrescriptionById(id);
            return Ok(data);
        }

        [Authorize(Roles = "Admin,Doctor")]
        [HttpPost]
        public async Task<IActionResult> AddPrescription(PrescriptionDTO prescriptionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var data = await iprescription.AddPrescription(prescriptionDTO);
            return Ok(data);
        }
    }
}
