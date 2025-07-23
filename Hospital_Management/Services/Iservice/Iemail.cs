namespace Hospital_Management.Services.Iservice
{
    public interface Iemail
    {
        Task SendAppointmentEmailAsync(string toEmail, string patientName, DateTime appointmentDate, 
                                       string doctorName);
    }
}
