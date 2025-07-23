namespace Hospital_Management.Models.DTOS
{
    public class UpdateAppointmentDTO
    {
        public DateTime ModifiedDate { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public string Notes { get; set; }
    }
}
