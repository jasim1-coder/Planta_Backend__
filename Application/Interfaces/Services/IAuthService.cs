using Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ResponseDTO<object>> Register(RegisterDTO regdata);
        Task<ResponseDTO<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginDto);
        Task<ResponseDTO<SelectRoleResponseDTO>> SelectRoleAsync(SelectRoleDTO selectRoleDto, Guid userId);

        Task<ResponseDTO<object>> RefreshAccessToken(Guid userid);
    }
}
