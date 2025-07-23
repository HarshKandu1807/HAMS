using Hospital_Management.Models;
using Hospital_Management.Models.DTOS;
using Hospital_Management.Services.Iservice;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Hospital_Management.Services
{
    public class DepartmentService:IDepartment
    {
        private readonly AppDbContext context;
        public DepartmentService(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<List<DepartmentDTO>?> GetDepartments()
        {
            var data = await context.Departments.Select(x => new DepartmentDTO
            {
                DepartmentName=x.DepartmentName,
                Description=x.Description
            }).ToListAsync();
            if (data.IsNullOrEmpty())
            {
                return null;
            }
            return data;

        }
        public async Task<DepartmentDTO?> GetDepartmentById(int id)
        {
            var data = await context.Departments.FindAsync(id);
            if (data == null)
            {
                return null;
            }
            var result = new DepartmentDTO
            {
                DepartmentName = data.DepartmentName,
                Description=data.Description
            };
            return result;
        }
        public async Task<DepartmentDTO?> AddDepartment(DepartmentDTO DepartmentDTO)
        {
            var data = new Department
            {
                DepartmentName = DepartmentDTO.DepartmentName,
                Description=DepartmentDTO.Description
            };
            await context.Departments.AddAsync(data);
            await context.SaveChangesAsync();
            return new DepartmentDTO
            {
                DepartmentName=data.DepartmentName,
                Description=data.Description
            };

        }
        public async Task<DepartmentDTO?> UpdateDepartment(DepartmentDTO DepartmentDTO, int id)
        {
            var data = await context.Departments.FindAsync(id);
            if (data == null)
            {
                return null;
            }
            data.DepartmentName = DepartmentDTO.DepartmentName;
            data.Description = DepartmentDTO.Description;
            await context.SaveChangesAsync();
            return new DepartmentDTO
            {
                DepartmentName=data.DepartmentName,
                Description=data.Description
            };
        }

        public async Task<bool> DeleteDepartment(int id)
        {
            var data = await context.Departments.FindAsync(id);
            if (data == null)
            {
                return false;
            }
            else
            {
                context.Departments.Remove(data);
                await context.SaveChangesAsync();
                return true;
            }
        }
    }
}
