using Hospital_Management.Models.DTOS;

namespace Hospital_Management.Services.Iservice
{
    public interface Iappointment
    {
        Task<List<GetAppointmentDTO>?> GetAppointments();
        Task<List<GetAppointmentDTO>?> GetAppointmentPatient(int id);
        Task<List<GetAppointmentDTO>?> GetAppointmentDoctor(int id);
        Task<List<GetAppointmentDTO>?> GetAppointmentDay(DateTime dateTime);
        Task<string> AddAppointment(AppointmentDTO appointmentDTO);
        Task<string> UpdateAppointment(UpdateAppointmentDTO appointmentDTO, int id);
        Task<bool> DeleteAppointment(int id);
        Task<bool> CompleteAppointment(int id);
    }
}
