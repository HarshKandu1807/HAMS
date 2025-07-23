using Hospital_Management.Models;
using Hospital_Management.Models.DTOS;
using Hospital_Management.Services.Iservice;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Management.Services
{
    public class PrescriptionService:Iprescription
    {
        private readonly AppDbContext context;
        public PrescriptionService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<PrescriptionDTO?> AddPrescription(PrescriptionDTO prescriptionDTO)
        {
            var data = new Prescription
            {
                PrescriptionName = prescriptionDTO.PrescriptionName,
                AppointmentId = prescriptionDTO.AppointmentId
            };
            await context.Prescriptions.AddAsync(data);
            await context.SaveChangesAsync();
            return new PrescriptionDTO
            {
                PrescriptionName = data.PrescriptionName,
                AppointmentId = data.AppointmentId
            };
        }

        public async Task<List<PrescriptionDTO>?> GetPrescription()
        {
            var data = await context.Prescriptions.Select(x => new PrescriptionDTO
            {
                PrescriptionName=x.PrescriptionName,
                AppointmentId=x.AppointmentId,
            }).ToListAsync();
            return data;
        }
        public async Task<PrescriptionDTO> GetPrescriptionById(int id)
        {
            var data = await context.Prescriptions.FindAsync(id);
            return new PrescriptionDTO
            {
                PrescriptionName=data.PrescriptionName,
                AppointmentId=data.AppointmentId
            };
        }
    }
}
