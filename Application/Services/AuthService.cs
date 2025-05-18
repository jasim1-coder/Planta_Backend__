using Microsoft.AspNetCore.Http;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Common.Helpers;
using System.Security.Cryptography;


namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository authRepo;
        private readonly JWTGenerator jwtgenerator;
        private readonly IHttpContextAccessor httpContextAccessor;
        public AuthService(IAuthRepository _authrepo, JWTGenerator _jwtgenerator, IHttpContextAccessor _httpContextAccessor)
        {
            authRepo = _authrepo;
            jwtgenerator = _jwtgenerator;
            httpContextAccessor = _httpContextAccessor;
           
        }
        public async Task<ResponseDTO<object>> Register(RegisterDTO regdata)
        {
            try
            {
                // Validate uniqueness
                if (await authRepo.GetByUserNameAsync(regdata.UserName) != null)
                    return new ResponseDTO<object> { StatusCode = 409, Message = "Username already exists" };

                if (await authRepo.GetByEmailAsync(regdata.Email) != null)
                    return new ResponseDTO<object> { StatusCode = 409, Message = "Email already exists" };

                // Hash password and set ID
                regdata.Password = HashPassword(regdata.Password);
                regdata.UserId = Guid.NewGuid();

                // Repository handles transaction internally
                await authRepo.RegisterAsync(regdata);

                return new ResponseDTO<object> { StatusCode = 201, Message = "Registration Successful" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };
            }
        }
        public async Task<ResponseDTO<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginDto)
        {
            try
            {
                var user = await authRepo.GetByEmailAsync(loginDto.Email);
                if (user == null)
                    return new ResponseDTO<LoginResponseDTO> { StatusCode = 404, Message = "User not found" };
                if (user.IsBlocked)
                    return new ResponseDTO<LoginResponseDTO> { StatusCode = 403, Message = "You are blocked by admin" };

                var roles = await authRepo.GetUserRolesAsync(user.UserID);
                var accessToken =  jwtgenerator.GenerateToken(user.UserID);

                // set access token as cookie
                var context = httpContextAccessor.HttpContext;
                if (context != null)
                {
                    context.Response.Cookies.Append("accessToken", accessToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false,
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTime.UtcNow.AddDays(7)
                    });
                }

                // generate refresh token and store
                var refreshToken = GenerateRefreshToken();
                await authRepo.UpdateRefreshToken(user.UserID, refreshToken);

                return new ResponseDTO<LoginResponseDTO>
                {
                    StatusCode = 200,
                    Message = "Login successful",
                    Data = new LoginResponseDTO
                    {
                        UserId = user.UserID,
                        Roles = (List<string>)roles,
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<LoginResponseDTO>
                {
                    StatusCode = 500,
                    Error = ex.Message
                };
            }
        }
        public async Task<ResponseDTO<SelectRoleResponseDTO>> SelectRoleAsync(SelectRoleDTO dto, Guid userId)
        {
            var roles = await authRepo.GetUserRolesAsync(userId);
            if (!roles.Contains(dto.SelectedRole))
                return new ResponseDTO<SelectRoleResponseDTO> { StatusCode = 403, Message = "Role not assigned to user" };
            var updatedToken = jwtgenerator.GenerateToken(userId, dto.SelectedRole);
            return new ResponseDTO<SelectRoleResponseDTO>
            {
                StatusCode = 200,
                Message = "Role selected successfully",
                Data = new SelectRoleResponseDTO { Role = dto.SelectedRole, UpdatedToken = updatedToken }
            };
        }

        public async Task<ResponseDTO<object>> RefreshAccessToken(Guid userid)
        {
            try
            {

                var user = await authRepo.CheckRefreshToken(userid);
                if (user == null || user.RefreshTokenExpiry < DateTime.Now)
                {
                    return new ResponseDTO<object> { StatusCode = 401, Message = "Invalid or expired refresh token" };
                }
                var context = httpContextAccessor.HttpContext;

                var accessToken =  jwtgenerator.GenerateToken(user.UserID);
                var newRefreshToken = GenerateRefreshToken();
                if (context != null)
                {
                    context.Response.Cookies.Append("accessToken", accessToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false,
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTime.Now.AddDays(7)
                    });
                }
                await authRepo.UpdateRefreshToken(user.UserID, newRefreshToken);
                return new ResponseDTO<object>
                {
                    StatusCode = 200,
                    Message = "Token Refreshed Successfully",

                };



            }
            catch (Exception ex)
            {
                return new ResponseDTO<object> { StatusCode = 500, Error = ex.Message };
            }

        }

        private string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);


        public bool Verifypassword(string password, string hashedpassword)
        {
            bool verifiedpassword = BCrypt.Net.BCrypt.Verify(password, hashedpassword);
            return verifiedpassword;
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
    }
}
