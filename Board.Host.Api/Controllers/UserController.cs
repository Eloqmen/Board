﻿using Board.Contracts;
using Board.Contracts.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Board.Application.AppData.Contexts.Users.Services;

namespace Board.Host.Api.Controllers
{
    /// <summary>
    /// Контроллер для работы с аккаунтами.
    /// </summary>
    /// <response code="500">Произошла внутренняя ошибка.</response>
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        /// <summary>
        /// Инициализирует экземпляр <see cref="UserController"/>
        /// </summary>
        /// <param name="logger">Сервис логирования.</param>
        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Зарегистрировать новый аккаунт.
        /// </summary>
        /// <param name="dto">Модель регистрации аккаунта.</param>
        /// <param name="cancellation">Токен отмены.</param>
        /// <response code="201">Аккаунт успешно зарегистрирован.</response>
        /// <response code="400">Модель данных запроса невалидна.</response>
        /// <response code="422">Произошёл конфликт бизнес-логики.</response>
        /// <returns>Модель зарегистрированного аккаунта.</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto dto, CancellationToken cancellation)
        {
            _logger.LogInformation("Регистрация нового аккаунта.");

            var result = await _userService.RegisterUserAsync(dto, cancellation);

            return await Task.Run(() => CreatedAtAction(nameof(Login), result), cancellation);
        }

        /// <summary>
        /// Войти в аккаунт.
        /// </summary>
        /// <param name="dto">Модель входа в аккаунт.</param>
        /// <param name="cancellation">Токен отмены.</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">Модель данных запроса невалидна.</response>
        /// <response code="403">Доступ запрещён (пользователь заблокирован).</response>
        /// <response code="404">Пользователь не найден.</response>
        /// <returns>Модель с данными входа.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto, CancellationToken cancellation)
        {
            _logger.LogInformation("Вход в аккаунт.");

            var result = await _userService.LoginAsync(dto, cancellation);

            return await Task.Run(() => Ok(result), cancellation);
        }

        [HttpPost("logout")]
        public async Task Logout(string token)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpPost("GetUserInfo")]
        public async Task<UserDto> GetUserInfo(CancellationToken cancellation)
        {
            var result = await _userService.GetCurrentAsync(cancellation);

            return result;

            //new UserDto
            //{
            //    Scheme = HttpContext.User.Identity.AuthenticationType,
            //    IsAuthenticated = HttpContext.User.Identity.IsAuthenticated,
            //    Claims = HttpContext.User.Claims.ToList()
            //};
        }
    }
}
