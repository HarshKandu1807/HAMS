namespace Hospital_Management.Models.DTOS
{
    public class GetAppointmentDTO
    {
        public DateTime AppointmentDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public AppointmentStatus Status { get; set; }
        public string Notes { get; set; }
        public PrescriptionDTO Prescription { get; set; }
    }
}
