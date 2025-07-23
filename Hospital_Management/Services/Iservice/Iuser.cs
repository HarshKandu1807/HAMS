using Hospital_Management.Models.DTOS;

namespace Hospital_Management.Services.Iservice
{
    public interface Iuser
    {
        Task<string> Register(UserDTO userDTO);
        Task<string> Login(LoginDTO loginDTO);
    }
}
