using Hospital_Management.Models.DTOS;

namespace Hospital_Management.Services.Iservice
{
    public interface Iprescription
    {
        Task<PrescriptionDTO?> AddPrescription(PrescriptionDTO prescriptionDTO);
        Task<List<PrescriptionDTO>?> GetPrescription();
        Task<PrescriptionDTO> GetPrescriptionById(int id);
    }
}
