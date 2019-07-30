using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.IdentityModel.Tokens;
using JWTAuth.Entities;
using JWTAuth.Repositories;

namespace JWTAuth.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly SigningConfigurations _signingConfigurations;
        private readonly TokenConfigurations _tokenConfigurations;

        public LoginController(UserRepository userRepository,
            SigningConfigurations signingConfigurations,
            TokenConfigurations tokenConfigurations)
        {
            _userRepository = userRepository;
            _signingConfigurations = signingConfigurations;
            _tokenConfigurations = tokenConfigurations;
        }

        [AllowAnonymous]
        [HttpPost]
        public object Post([FromBody]User userDto)
        {
            bool validCredentials = false;
            if (userDto == null)
            {
                return null;
            }
            
            var user = _userRepository.Find(userDto.UserID);
            validCredentials = (user != null &&
                    userDto.UserID == user.UserID &&
                    userDto.Password == user.Password);
            
            if (validCredentials)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(user.UserID.ToString(), "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, userDto.UserID.ToString())
                    }
                );

                DateTime creationDate = DateTime.Now;
                DateTime expirationDate = creationDate +
                    TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = _tokenConfigurations.Issuer,
                    Audience = _tokenConfigurations.Audience,
                    SigningCredentials = _signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = creationDate,
                    Expires = expirationDate
                });
                var token = handler.WriteToken(securityToken);

                return new
                {
                    authenticated = true,
                    created = creationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = token,
                    message = "OK"
                };
            }
            else
            {
                return new
                {
                    authenticated = false,
                    message = "Failed to authenticate"
                };
            }
        }
    }
}