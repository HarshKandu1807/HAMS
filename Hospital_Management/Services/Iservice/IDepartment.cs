using Hospital_Management.Models.DTOS;

namespace Hospital_Management.Services.Iservice
{
    public interface IDepartment
    {
        Task<List<DepartmentDTO>?> GetDepartments();
        Task<DepartmentDTO?> GetDepartmentById(int id);
        Task<DepartmentDTO?> AddDepartment(DepartmentDTO DepartmentDTO);
        Task<DepartmentDTO?> UpdateDepartment(DepartmentDTO DepartmentDTO, int id);
        Task<bool> DeleteDepartment(int id);
    }
}
