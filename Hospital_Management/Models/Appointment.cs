using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Management.Models
{
    public enum AppointmentStatus{
        Scheduled,
        Completed,
        Rescheduled,
        Canceled
    }
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        public AppointmentStatus Status { get; set; }
        public string Notes { get; set; }

        public int? PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public Prescription Prescription { get; set; }
    }
}
