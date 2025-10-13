using CRM.Application.Commons.Bases.Response;
using CRM.Application.Dtos.User.Request;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Infrastructure.Persistences.Interfaces;
using CRM.Utilities.AppSettings;
using CRM.Utilities.Static;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WatchDog;
using BC = BCrypt.Net.BCrypt;


namespace CRM.Application.Services
{
    public class AuthApplication : IAuthApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly AppSettings _appSettings;
        private readonly IEmailApplication _emailApplication;
        public AuthApplication(IUnitOfWork unitOfWork, IConfiguration configuration, IOptions<AppSettings> appSettings, IEmailApplication emailApplication)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _appSettings = appSettings.Value;
            _emailApplication = emailApplication;


        }
        public async Task<BaseResponse<bool>> ChangePassword(ChangePasswordRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var user = await _unitOfWork.User.UserByEmail(requestDto.Email!);
                if (user == null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_TOKEN_ERROR;
                    return response;
                }
                if (user.PasswordResetToken != requestDto.Token || user.TokenExpiration < DateTime.UtcNow)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_TOKEN_ERROR;
                    return response;
                }

                user.Password = BC.HashPassword(requestDto.NewPassword);
                user.PasswordResetToken = null;
                user.TokenExpiration = null;

                response.Data = await _unitOfWork.User.EditAsync(user);
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_CHANGE_PASSWORD;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }
            return response;
        }

        public async  Task<BaseResponse<string>> Login(TokenRequestDto requestDto, string authType)
        {
            var response = new BaseResponse<string>();

            try
            {
                var user = await _unitOfWork.User.UserByEmail(requestDto.Email);
                if (user == null)
                {

                    var client = await _unitOfWork.Client.UserByEmail(requestDto.Email!);

                    if (client == null)
                    {
                        response.IsSuccess = false;
                        response.Message = ReplyMessage.MESSAGE_TOKEN_ERROR;
                        return response;
                    }
                    //client.PasswordResetToken != requestDto.Toke ||
                    if (client.TokenExpiration < DateTime.UtcNow)
                    {
                        response.IsSuccess = false;
                        response.Message = ReplyMessage.MESSAGE_TOKEN_ERROR;
                        return response;
                    }
                    string passwordHash = BC.HashPassword(requestDto.Password);

                    if (BC.Verify(requestDto.Password, client.Password))
                    {
                        response.IsSuccess = true;
                        response.Data = GenerateTokenClient(client);
                        response.Message = ReplyMessage.MESSAGE_TOKEN;
                        return response;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = ReplyMessage.MESSAGE_LOGIN_FAIL;
                        return response;
                    }


                }
                if (user.AuthType != authType)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_AUTH_TYPE_GOOGLE;
                    return response;
                }

                if (BC.Verify(requestDto.Password, user.Password))
                {
                    response.IsSuccess = true;
                    response.Data = GenerateToken(user);
                    response.Message = ReplyMessage.MESSAGE_TOKEN;

                    return response;
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }
            return response;
        }

        public async  Task<BaseResponse<string>> LoginWithGoogle(string credentials, string authType)
        {
            var response = new BaseResponse<string>();
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>
                    {
                       _appSettings.ClientId!
                    }

                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(credentials, settings);
                var user = await _unitOfWork.User.UserByEmail(payload.Email);
                if (user is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_GOOGLE_ERROR;
                    return response;
                }
                if (user.AuthType != authType)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_AUTH_TYPE;
                    return response;
                }
                response.IsSuccess = true;
                response.Data = GenerateToken(user);
                response.Message = ReplyMessage.MESSAGE_TOKEN;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse<bool>> RequestPasswordReset(ResetPasswordRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var user = await _unitOfWork.User.UserByEmail(requestDto.Email!);
                if (user == null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_TOKEN_ERROR;
                    return response;
                }

                string resetToken = GenerateResetToken();
                user.PasswordResetToken = resetToken;
                user.TokenExpiration = DateTime.UtcNow.AddHours(1);

                await _emailApplication.SendEmailAsync(requestDto.Email!, "Restablecer la contraseña", "El token para restablecer la contraseña : " + requestDto.CurrentUrlClient + "/" + resetToken + "/" + requestDto.Email);
                response.Data = await _unitOfWork.User.EditAsync(user);
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_REQUEST_PASSWORD_RESET;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
                WatchLogger.Log(ex.InnerException!.Message);

            }
            return response;
        }

        private string GenerateToken(User user)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Email!),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.UserName!),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Email!),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, Guid.NewGuid().ToString(),ClaimValueTypes.Integer64)

            };

            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Issuer"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:Expires"]!)),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GenerateTokenClient(Client client)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, client.Email!),
                new Claim(JwtRegisteredClaimNames.FamilyName, client.UserName!),
                new Claim(JwtRegisteredClaimNames.GivenName, client.Email!),
                new Claim(JwtRegisteredClaimNames.UniqueName, client.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, Guid.NewGuid().ToString(),ClaimValueTypes.Integer64)

            };
             
            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Issuer"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:Expires"]!)),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateResetToken()
        {
            return Guid.NewGuid().ToString("N");
        }

        public async Task<BaseResponse<string>> LoginClient(TokenRequestDto requestDto, string authType)
        {
            var response = new BaseResponse<string>();

            try
            {
                var client =  _unitOfWork.Client.UserByEmail(requestDto.Email);
                if (client.Result == null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_TOKEN_ERROR;
                    return response;
                }
                if (client.Result.AuthType != authType)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_AUTH_TYPE_GOOGLE;
                    return response;
                }

                if (BC.Verify(requestDto.Password, client.Result.Password))
                {
                    response.IsSuccess = true;
                    response.Data = GenerateTokenClient(client.Result);
                    response.Message = ReplyMessage.MESSAGE_TOKEN;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse<bool>> ChangePasswordClient(ChangePasswordRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var client = await _unitOfWork.Client.UserByEmail(requestDto.Email!);
                if (client == null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_TOKEN_ERROR;
                    return response;
                }
                if (client.PasswordResetToken != requestDto.Token || client.TokenExpiration < DateTime.UtcNow)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_TOKEN_ERROR;
                    return response;
                }

                client.Password = BC.HashPassword(requestDto.NewPassword);
                client.PasswordResetToken = null;
                client.TokenExpiration = null;

                response.Data = await _unitOfWork.Client.EditAsync(client);
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_CHANGE_PASSWORD;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }
            return response;
        }
    }
}
