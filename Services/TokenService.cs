using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OLEToolBoxWebAPIPruebas.DTOs.DTOUsers;
using OLEToolBoxWebAPIPruebas.DTOs.Token;

namespace OLEToolBoxWebAPIPruebas.Services
    {
    public class TokenService
        {
        private readonly IConfiguration _configuration;
        private readonly HashService _hashService;

        public TokenService(IConfiguration configuration, HashService hashService)
            {
            _configuration = configuration;
            _hashService = hashService;
            }
        // ----------- GENERAR TOKEN --------------

        // Método privado para generar un token JWT basado en las credenciales del usuario.
        public DTOToken GenerarToken(DTOUserCredentialsPost credencialesUsuario)
            {
            // Define los claims (información adicional) que se incluirán en el token.
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, credencialesUsuario.EmailDTO), // Claim con el email del usuario.
                new Claim("lo que yo quiera", "cualquier otro valor") // Ejemplo de otro claim personalizado.
            };

            // Obtiene la clave secreta utilizada para generar el token desde la configuración.
            var clave = _configuration["ClaveJWT"];
            // Crea la clave a partir de la clave secreta obtenida.
            var claveKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave));
            var signinCredentials = new SigningCredentials(claveKey, SecurityAlgorithms.HmacSha256);
            // Define las características del token, como los claims, la fecha de expiración y las credenciales de firma.
            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30), // El token expira en 30 días.
                signingCredentials: signinCredentials
            );

            // Convierte el token en una cadena para poder devolverlo.
            var tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

            // Retorna un DTO que contiene el token JWT y el email del usuario.
            return new DTOToken()
                {
                Token = tokenString,
                Email = credencialesUsuario.EmailDTO
                };
            }
        }
    }

