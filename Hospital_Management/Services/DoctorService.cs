using Hospital_Management.Models.DTOS;
using Hospital_Management.Models;
using Hospital_Management.Services.Iservice;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Hospital_Management.Services
{
    public class DoctorService:IDoctor
    {
        private readonly AppDbContext context;
        public DoctorService(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<List<DoctorDTO>?> GetDoctors()
        {
            var data = await context.Doctors.Select(x => new DoctorDTO
            {
                Name = x.Name,
                DepartmentId = x.DepartmentId,
                Specialization = x.Specialization,
                CreatedAt = x.CreatedAt
            }
            ).ToListAsync();
            if (data == null)
            {
                return null;
            }
            return data;
        }
        public async Task<DoctorDTO?> GetDoctorById(int id)
        {
            var data = await context.Doctors.FindAsync(id);
            
            if(data== null)
            {
                return null;
            }

            var result = new DoctorDTO
            {
                Name = data.Name,
                DepartmentId = data.DepartmentId,
                Specialization = data.Specialization,
                CreatedAt = data.CreatedAt
            };

            return result;
        }
        public async Task<DoctorDTO?> AddDoctor(DoctorDTO DoctorDTO)
        {
            //if (!data)
            //{
                var result = new Doctor
                {
                    Name = DoctorDTO.Name,
                    DepartmentId=DoctorDTO.DepartmentId,
                    Specialization=DoctorDTO.Specialization,
                    CreatedAt=DateTime.UtcNow
                };
                await context.Doctors.AddAsync(result);
                await context.SaveChangesAsync();
                return new DoctorDTO
                {
                    Name = result.Name,
                    DepartmentId = result.DepartmentId,
                    Specialization = result.Specialization,
                    CreatedAt = result.CreatedAt
                };
            //}
            //else
            //{
            //    return null;
            //}
        }

        public async Task<DoctorLeaveDTO> AddDoctorLeave(DoctorLeaveDTO doctorLeaveDTO)
        {
            var data = new DoctorLeave
            {
                DoctorId = doctorLeaveDTO.DoctorId,
                StartDate = doctorLeaveDTO.StartDate,
                EndDate = doctorLeaveDTO.EndDate,
                Reason = doctorLeaveDTO.Reason
            };
            await context.DoctorLeaves.AddAsync(data);
            await context.SaveChangesAsync();
            return new DoctorLeaveDTO
            {
                DoctorId = data.DoctorId,
                StartDate=data.StartDate,
                EndDate=data.EndDate,
                Reason=data.Reason
            };

        }

        public async Task<DoctorDTO?> UpdateDoctor(DoctorDTO DoctorDTO, int id)
        {
            var data = await context.Doctors.FindAsync(id);
            if (data != null)
            {
                data.Name = DoctorDTO.Name;
                data.DepartmentId = DoctorDTO.DepartmentId;
                data.Specialization = DoctorDTO.Specialization;
                data.CreatedAt = data.CreatedAt;
                await context.SaveChangesAsync();
            }
            else
            {
                return null;
            }
            return new DoctorDTO
            {
                Name=data.Name,
                DepartmentId=data.DepartmentId,
                Specialization=data.Specialization,
                CreatedAt=data.CreatedAt
            };
        }
        public async Task<bool> DeleteDoctor(int id)
        {
            var data = await context.Doctors.FindAsync(id);
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
