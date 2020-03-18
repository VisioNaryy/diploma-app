using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DiplomaApp.API.Data;
using DiplomaApp.API.Dtos;
using DiplomaApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DiplomaApp.API.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
         private readonly IConfiguration _config;
        private readonly IAuthRepository _repo;
        public AuthController (IAuthRepository repo, IConfiguration config) 
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            //validate request

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("User already exists");

            //creating an user

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);        

        }

        //jwt token
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            //checking, that user is exists in database
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if(userFromRepo == null)
                return Unauthorized();


            //create token
            //one claim with user id
            //second claim with user name
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()), //id
                new Claim(ClaimTypes.Name, userFromRepo.Username) //username
            };

            //create key to sign a token (into byte array)
            var key = new SymmetricSecurityKey(Encoding.UTF8.
            GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //security token descriptor

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            //create and parse token decryptor

            var token = tokenHandler.CreateToken(tokenDescriptor);


            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}