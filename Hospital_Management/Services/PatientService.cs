using Hospital_Management.Models;
using Hospital_Management.Models.DTOS;
using Hospital_Management.Services.Iservice;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Management.Services
{
    public class PatientService:IPatient
    {
        private readonly AppDbContext context;
        public PatientService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<GetPatientRecordDTO?> GetPatientRecords(int id)
        {
            var patient = await context.Patients
                .Include(p => p.Appointments)
                    .ThenInclude(a => a.Doctor)
                .Include(p => p.Appointments)
                    .ThenInclude(a => a.Prescription)
                .FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null)
                return null;

            var dto = new GetPatientRecordDTO
            {
                Name = patient.Name,
                Email = patient.Email,
                ContactNo = patient.ContactNo,
                Address = patient.Address,
                Gender = patient.Gender,
                CreatedDate = patient.CreatedDate,
                Appointments = patient.Appointments.Select(a => new GetAppointmentDTO
                {
                    AppointmentDate = a.AppointmentDate,
                    ModifiedDate = a.ModifiedDate,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    Status = a.Status,
                    Notes = a.Notes,
                    Prescription = a.Prescription != null
                        ? new PrescriptionDTO
                        {
                            PrescriptionName = a.Prescription.PrescriptionName,
                            AppointmentId = a.Prescription.AppointmentId
                        }
                        : null
                }).ToList(),
            };

            return dto;
        }

        public async Task<List<PatientDTO>?> GetPatients()
        {
            var data = await context.Patients.Select(x => new PatientDTO
            {
                Name = x.Name,
                Email = x.Email,
                ContactNo = x.ContactNo,
                Address = x.Address,
                Gender = x.Gender,
                CreatedDate = x.CreatedDate
            }
            ).ToListAsync();
            if (data == null)
            {
                return null;
            }
            return data;
        }

        public async Task<PatientDTO?> GetPatientById(int id)
        {
            var data = await context.Patients.FindAsync(id);

            if (data == null)
            {
                return null;
            }
            var result = new PatientDTO
            {
                Name = data.Name,
                ContactNo = data.ContactNo,
                Email = data.Email,
                Address = data.Address,
                Gender = data.Address,
                CreatedDate = data.CreatedDate
            };
            return result;
        }

        public async Task<PatientDTO?> AddPatient(PatientDTO patientDTO)
        {
            var data = await context.Patients.AnyAsync(x=>x.Email==patientDTO.Email);
            if (!data)
            {
                var result = new Patient
                {
                    Name = patientDTO.Name,
                    ContactNo=patientDTO.ContactNo,
                    Email=patientDTO.Email,
                    Address=patientDTO.Address,
                    Gender=patientDTO.Gender,
                    CreatedDate=DateTime.UtcNow
                };
                await context.Patients.AddAsync(result);
                await context.SaveChangesAsync();
                return new PatientDTO
                {
                    Name = result.Name,
                    ContactNo = result.ContactNo,
                    Email = result.Email,
                    Address = result.Address,
                    Gender = result.Gender,
                    CreatedDate = result.CreatedDate
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<PatientDTO?> UpdatePatient(PatientDTO patientDTO, int id)
        {
            var data = await context.Patients.FindAsync(id);
            if (data != null)
            {
                data.Name = patientDTO.Name;
                data.ContactNo = patientDTO.ContactNo;
                data.Email = patientDTO.Email;
                data.Address = patientDTO.Address;
                data.Gender = patientDTO.Gender;
                data.CreatedDate = patientDTO.CreatedDate;
                await context.SaveChangesAsync();
            }
            else
            {
                return null;
            }
            return patientDTO;
        }

        public async Task<bool> DeletePatient(int id)
        {
            var data = await context.Patients.FindAsync(id);
            if (data != null)
            {
                context.Remove(data);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
