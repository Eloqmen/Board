using Board.Application.AppData.Contexts.Users.Repositories;
using Board.Contracts.User;
using Board.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Board.Application.AppData.Contexts.Users.Services
{
    /// <inheritdoc cref="IUserService" />
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _сonfiguration;

        public UserService(
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccesso,
            IConfiguration сonfiguration)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccesso;
            _сonfiguration = сonfiguration;
        }

        /// <inheritdoc />
        public async Task<Guid> RegisterUserAsync(CreateUserDto userDto, CancellationToken cancellation)
        {
            var user = new User
            {
                Name = userDto.Login,
                Login = userDto.Login,
                Password = userDto.Password,
                Created = DateTime.UtcNow
            };

            var existingUser = await _userRepository.FindWhere(user => user.Login == userDto.Login, cancellation);
            if (existingUser != null)
            {
                throw new Exception($"Пользователь с логином '{userDto.Login}' уже зарегистрирован!");
            }

            await _userRepository.AddAsync(user, cancellation);

            return user.Id;
        }

        /// <inheritdoc />
        public async Task<string> LoginAsync(LoginUserDto userDto, CancellationToken cancellation)
        {
            var existingUser = await _userRepository.FindWhere(user => user.Login == userDto.Login, cancellation);
            if (existingUser == null)
            {
                throw new Exception("Пользователь не найден!");
            }

            if (!existingUser.Password.Equals(userDto.Password))
            {
                throw new Exception("Неверный логин или пароль.");
            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
            new Claim(ClaimTypes.Name, existingUser.Login)
        };

            var secretKey = _сonfiguration["Jwt:Key"];

            var token = new JwtSecurityToken
                (
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    SecurityAlgorithms.HmacSha256
                    )
                );

            var result = new JwtSecurityTokenHandler().WriteToken(token);

            return result;
        }

        /// <inheritdoc />
        public async Task<UserDto> GetCurrentAsync(CancellationToken cancellation)
        {
            var claims = _httpContextAccessor.HttpContext.User.Claims;
            var claimId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(claimId))
            {
                return null;
            }

            var id = Guid.Parse(claimId);
            var user = await _userRepository.FindById(id, cancellation);

            if (user == null)
            {
                throw new Exception($"Не найден пользователь с идентификатором '{id}'.");
            }

            //TODO
            var result = new UserDto
            {
                Id = user.Id,
                Login = user.Login
            };

            return result;
        }
        }
    }
