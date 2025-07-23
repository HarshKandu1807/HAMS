using Hospital_Management.Models;
using Hospital_Management.Models.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Services.Iservice
{
    public interface IPatient
    {
        Task<List<PatientDTO>?> GetPatients();
        Task<PatientDTO?> GetPatientById(int id);
        Task<GetPatientRecordDTO?> GetPatientRecords(int id);
        Task<PatientDTO?> AddPatient(PatientDTO patientDTO);
        Task<PatientDTO?> UpdatePatient(PatientDTO patientDTO,int id);
        Task<bool> DeletePatient(int id);
    }
}
