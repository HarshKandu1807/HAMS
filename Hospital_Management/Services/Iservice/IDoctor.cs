using Hospital_Management.Models.DTOS;

namespace Hospital_Management.Services.Iservice
{
    public interface IDoctor
    {
        Task<List<DoctorDTO>?> GetDoctors();
        Task<DoctorDTO?> GetDoctorById(int id);
        Task<DoctorDTO?> AddDoctor(DoctorDTO doctorDTO);
        Task<DoctorLeaveDTO> AddDoctorLeave(DoctorLeaveDTO doctorLeaveDTO);
        Task<DoctorDTO?> UpdateDoctor(DoctorDTO doctorDTO, int id);
        Task<bool> DeleteDoctor(int id);
    }
}
