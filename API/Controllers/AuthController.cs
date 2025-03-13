using API.DTOs;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AuthController(
            IAuthService authService,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _authService = authService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);

                    return BadRequest(new ApiResponse(400, string.Join(", ", errors)));
                }

                if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
                    return BadRequest(new ApiResponse(400, "Email and password are required"));

                var user = await _userRepository.GetByEmailAsync(loginDto.Email);

                if (user == null)
                    return Unauthorized(new ApiResponse(401, "Invalid credentials"));

                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                    return Unauthorized(new ApiResponse(401, "Invalid credentials"));

                user.LastLogin = DateTime.UtcNow;
                await _userRepository.UpdateUserAsync(user);

                var token = await _authService.GenerateJwtToken(user);

                return new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Token = token,
                    Role = user.Role.ToString()
                };
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userRepository.EmailExistsAsync(registerDto.Email))
                return BadRequest(new ApiResponse(400, "Email already exists"));

            var user = new User
            {
                Email = registerDto.Email,
                Name = registerDto.Name,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = registerDto.Role
            };

            var result = await _userRepository.CreateUserAsync(user);

            return new UserDto
            {
                Id = result.Id,
                Email = result.Email,
                Name = result.Name,
                Role = result.Role.ToString()
            };
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("update/{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, UpdateUserDto updateDto)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return NotFound(new ApiResponse(404, "User not found"));

            user.Name = updateDto.Name ?? user.Name;
            user.Role = updateDto.Role ?? user.Role;

            if (!string.IsNullOrEmpty(updateDto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateDto.Password);
            }

            var result = await _userRepository.UpdateUserAsync(user);

            return new UserDto
            {
                Id = result.Id,
                Email = result.Email,
                Name = result.Name,
                Role = result.Role.ToString()
            };
        }
    }
}