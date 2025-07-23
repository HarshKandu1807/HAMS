using Hospital_Management.Models;
using Hospital_Management.Models.DTOS;
using Hospital_Management.Services.Iservice;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Hospital_Management.Services
{
    public class AppointmentService : Iappointment
    {
        private readonly AppDbContext context;
        private readonly Iemail iemail;
        public AppointmentService(AppDbContext context, Iemail iemail)
        {
            this.context = context;
            this.iemail = iemail;
        }

        public async Task<List<GetAppointmentDTO>?> GetAppointments()
        {
            var data = await context.Appointments.Include(x => x.Prescription).Where(x => x.Status == AppointmentStatus.Scheduled ||
            x.Status == AppointmentStatus.Rescheduled).Select(x => new GetAppointmentDTO
            {
                AppointmentDate = x.AppointmentDate,
                ModifiedDate = x.ModifiedDate,
                PatientId = x.PatientId,
                DoctorId = x.DoctorId,
                Status = x.Status,
                Notes = x.Notes,
                Prescription = x.Prescription != null ? new PrescriptionDTO
                {
                    PrescriptionName = x.Prescription.PrescriptionName,
                    AppointmentId = x.Prescription.AppointmentId,
                } : null,
            }).ToListAsync();
            if (data == null)
            {
                return null;
            }
            return data;
        }

        public async Task<List<GetAppointmentDTO>?> GetAppointmentPatient(int id)
        {
            var data = await context.Appointments.Where(x => x.PatientId == id).Select(x => new GetAppointmentDTO { 
                AppointmentDate = x.AppointmentDate, 
                ModifiedDate = x.ModifiedDate, 
                PatientId = x.PatientId, 
                DoctorId = x.DoctorId, 
                Status = x.Status, 
                Notes = x.Notes,
                Prescription = x.Prescription != null ? new PrescriptionDTO
                {
                    PrescriptionName = x.Prescription.PrescriptionName,
                    AppointmentId = x.Prescription.AppointmentId,
                } : null,
            }).ToListAsync();
            if (data == null)
            {
                return null;
            }
            return data;
        }

        public async Task<List<GetAppointmentDTO>?> GetAppointmentDoctor(int id)
        {
            var data = await context.Appointments.Where(x => x.DoctorId == id).Select(x => new GetAppointmentDTO
            {
                AppointmentDate = x.AppointmentDate,
                ModifiedDate = x.ModifiedDate,
                PatientId = x.PatientId,
                DoctorId = x.DoctorId,
                Status = x.Status,
                Notes = x.Notes
            }).ToListAsync();
            if (data == null)
            {
                return null;
            }
            return data;
        }

        public async Task<List<GetAppointmentDTO>?> GetAppointmentDay(DateTime dateTime)
        {
            var data = await context.Appointments.Where(x => x.ModifiedDate.Date == dateTime.Date).Select(x => new GetAppointmentDTO
            {
                AppointmentDate = x.AppointmentDate,
                ModifiedDate = x.ModifiedDate,
                PatientId = x.PatientId,
                DoctorId = x.DoctorId,
                Status = x.Status,
                Notes = x.Notes
            }).ToListAsync();
            if (data == null)
            {
                return null;
            }
            return data;
        }

        public async Task<string> AddAppointment(AppointmentDTO appointmentDTO)
        {
            var check = await context.Appointments.AnyAsync(a => a.DoctorId == appointmentDTO.DoctorId &&
                                                                 a.AppointmentDate == appointmentDTO.AppointmentDate && 
                                                                 (a.Status == AppointmentStatus.Scheduled || a.Status==AppointmentStatus.Rescheduled));


            var leavecheck = await context.DoctorLeaves.AnyAsync(a => a.DoctorId == appointmentDTO.DoctorId &&
                                                                      (appointmentDTO.AppointmentDate >= a.StartDate && appointmentDTO.AppointmentDate<=a.EndDate));

            var patient = await context.Patients.FindAsync(appointmentDTO.PatientId);
            var doctor = await context.Doctors.FindAsync(appointmentDTO.DoctorId);
            if (check)
            {
                return "Slot is already booked";
            }

            if (leavecheck)
            {
                return "Doctor is on Leave";
            }
            var data = new Appointment
            {
                AppointmentDate = appointmentDTO.AppointmentDate,
                ModifiedDate = appointmentDTO.AppointmentDate,
                PatientId = appointmentDTO.PatientId,
                DoctorId = appointmentDTO.DoctorId,
                Status = AppointmentStatus.Scheduled,
                Notes = appointmentDTO.Notes
            };
            await context.AddAsync(data);
            await context.SaveChangesAsync();
            await iemail.SendAppointmentEmailAsync(
                patient.Email,
                patient.Name,
                appointmentDTO.AppointmentDate,
                doctor.Name
            );
            return "Appointment Created Successfully.";
        }

        //public async Task<bool> AddAppointment(AppointmentDTO appointmentDTO)
        //{
        //    var check = await context.Appointments.AnyAsync(a => a.DoctorId == appointmentDTO.DoctorId &&
        //                                                         a.AppointmentDate == appointmentDTO.AppointmentDate && 
        //                                                         a.Status == AppointmentStatus.Scheduled);
        //    if (check)
        //    {
        //        return false;
        //    }
        //    var data = new Appointment
        //    {
        //        AppointmentDate = appointmentDTO.AppointmentDate,
        //        ModifiedDate=appointmentDTO.AppointmentDate,
        //        PatientId=appointmentDTO.PatientId,
        //        DoctorId=appointmentDTO.DoctorId,
        //        Status=AppointmentStatus.Scheduled,
        //        Notes=appointmentDTO.Notes
        //    };
        //    await context.AddAsync(data);
        //    await context.SaveChangesAsync();
        //    await iemail.SendAppointmentEmailAsync(
        //        patient.Email,
        //        patient.Name,
        //        appointmentDTO.AppointmentDate,
        //        doctor.Name
        //    );
        //    return "Appointment Created Successfully.";
        //}

        public async Task<string> UpdateAppointment(UpdateAppointmentDTO appointmentDTO, int id)
        {
            var check = await context.Appointments.AnyAsync(a => a.DoctorId == appointmentDTO.DoctorId &&
                                                                 a.AppointmentDate == appointmentDTO.ModifiedDate &&
                                                                 (a.Status == AppointmentStatus.Scheduled || a.Status==AppointmentStatus.Rescheduled));


            var leavecheck = await context.DoctorLeaves.AnyAsync(a => a.DoctorId == appointmentDTO.DoctorId &&
                                                                      (appointmentDTO.ModifiedDate >= a.StartDate && appointmentDTO.ModifiedDate <= a.EndDate));
            var data = await context.Appointments.FindAsync(id);

            var patient = await context.Patients.FindAsync(appointmentDTO.PatientId);
            var doctor = await context.Doctors.FindAsync(appointmentDTO.DoctorId);

            if (data == null)
            {
                return "Appointment not found";
            }

            if (check)
            {
                return "Appointment already booked";
            }

            if (leavecheck)
            {
                return "Doctor is on Leave";
            }

            data.AppointmentDate = data.AppointmentDate;
            data.ModifiedDate = appointmentDTO.ModifiedDate;
            data.PatientId = appointmentDTO.PatientId;
            data.DoctorId = appointmentDTO.DoctorId;
            data.Status = AppointmentStatus.Rescheduled;
            data.Notes = appointmentDTO.Notes;
            await context.SaveChangesAsync();
            await iemail.SendAppointmentEmailAsync(
                patient.Email,
                patient.Name,
                appointmentDTO.ModifiedDate,
                doctor.Name
            );
            return "Appointment Rescheduled Successfully";
        }

        //public async Task<bool> UpdateAppointment(UpdateAppointmentDTO appointmentDTO, int id)
        //{
        //    var data = await context.Appointments.FindAsync(id);
        //    if (data == null)
        //    {
        //        return false;
        //    }
            
        //}

        public async Task<bool> DeleteAppointment(int id)
        {
            var data = await context.Appointments.FindAsync(id);
            if (data == null)
            {
                return false;
            }
            data.Status = AppointmentStatus.Canceled;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteAppointment(int id)
        {
            var data = await context.Appointments.FindAsync(id);
            if (data == null)
            {
                return false;
            }
            data.Status = AppointmentStatus.Completed;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
