
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OLEToolBoxWebAPIPruebas.Models;
using OLEToolBoxWebAPIPruebas.Services;
using OLEToolBoxWebAPIPruebas.DTOs;
using OLEToolBoxWebAPIPruebas.DTOs.DTOUsers;

namespace OLEToolBoxWebAPIPruebas.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
        {

        /**
        * 
        * Clase controladora sobre el modelo USUARIOS.
        * 
        * Siempre que se refiera en este documento a USUARIOS, 
        * me estaré refiriendo a la tabla en la DATABASE.
        * 
        * */


        private readonly PruebasoletoolboxContext _context;
        private readonly HashService _hashService;
        private readonly TokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        // private readonly OperacionesService _operacionesService;

        public AuthController(PruebasoletoolboxContext context, TokenService tokenService, HashService hashService, IHttpContextAccessor httpContextAccessor)
            {
            _context = context;
            _hashService = hashService;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            //  _operacionesService = operacionesService;
            }

        /**
         * 
         * Método GetUsuarios()
         * 
         * Devuelve todos los usuarios en la base de datos.
         * 
         * */


        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        //    {
        //    // Obtiene todos los mensajes del foro
        //    await _operacionesService.AddOperacion("Get", "Usuarios");
        //    return await _context.Usuarios.Include(x => x.PerfilUsuarios).ToListAsync();
        //    }


        /**
         * 
         * Método Login()
         * 
         * Concede acceso a un cliente a la app.
         * 
         * */

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] DTOUserCredentialsPost usuario)
            {
            // Busca en la base de datos un usuario que coincida con el correo electrónico proporcionado.
            var usuarioDB = await _context.UserCredentials.FirstOrDefaultAsync(x => x.Email == usuario.EmailDTO);
            if (usuarioDB == null)
                {
                return Unauthorized(); // Si no se encuentra el usuario, devuelve un error Unauthorized.
                }

            // Calcula el hash del password proporcionado utilizando el salt del usuario almacenado en la base de datos.
            var resultadoHash = _hashService.Hash(usuario.CredPasswordDTO, usuarioDB.Salt);
            if (usuarioDB.CredPassword == resultadoHash.Hash)
                {
                // Si las contraseñas coinciden, genera un token JWT llamando al método GenerarToken y lo devuelve junto con el email.
                var response = _tokenService.GenerarToken(usuario);
                // await _operacionesService.AddOperacion("Post", "Login");
                return Ok(response);
                }
            else
                {
                return Unauthorized(); // Si las contraseñas no coinciden, devuelve un error Unauthorized.
                }

            }

        /**
         * 
         * Método GetUsuariosId()
         * 
         * Devuelve un usuario en la base de datos seleccionada con id.
         * 
         * */

        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<Usuario>> GetUsuariosId(int id)
        //    {
        //    // Obtiene todos los mensajes del foro
        //    var usuario = await _context.Usuarios.FindAsync(id);

        //    if (usuario == null)
        //        {
        //        return NotFound();
        //        }

        //    await _operacionesService.AddOperacion("Get", "UsuariosPorId");
        //    return usuario;
        //    }

        /**
         * 
         * Método GetUsuariosEmail()
         * 
         * Devuelve un usuario en la base de datos seleccionada con email.
         * 
         * */

        //[HttpGet("{email}")]
        //public async Task<ActionResult<Usuario>> GetUsuariosEmail(string email)
        //    {
        //    // Obtiene todos los mensajes del foro
        //    var usuario = await _context.Usuarios.Where(x => x.Email == email).FirstOrDefaultAsync();

        //    if (usuario == null)
        //        {
        //        return NotFound();
        //        }

        //    await _operacionesService.AddOperacion("Get", "Email");
        //    return usuario;
        //    }

        /**
         * 
         * Método PostNuevoUsuarioHash()
         * 
         * Inserta un nuevo usuario encriptando la contraseña en la base de datos.
         * 
         * */


        //[AllowAnonymous]
        //[HttpPost("register")]
        //public async Task<ActionResult> PostNuevoUsuarioHash([FromBody] DTOUsuario usuario)
        //    {
        //    // Verificar si el usuario ya existe
        //    if (_context.Usuarios.Any(u => u.Email == usuario.Email))
        //        {
        //        // Retorna un error indicando que el usuario ya existe
        //        return BadRequest("El usuario ya existe.");
        //        }

        //    var resultadoHash = _hashService.Hash(usuario.Password);
        //    var newUsuario = new Usuario
        //        {
        //        Email = usuario.Email,
        //        Password = resultadoHash.Hash,
        //        Salt = resultadoHash.Salt
        //        };

        //    await _context.Usuarios.AddAsync(newUsuario);
        //    await _context.SaveChangesAsync();

        //    await _operacionesService.AddOperacion("Post", "Register");
        //    return Ok(newUsuario);
        //    }



        /**
         * 
         * Método LinkChangePasswordHash([FromBody] DTOUsuarioLinkChangePassword usuario)
         * 
         * A partir de un email concede un link con la solicitud de cambio de contraseña.
         * 
         * */

        //[HttpPost("hash/linkchangepassword")]
        //public async Task<ActionResult> LinkChangePasswordHash([FromBody] DTOUsuarioLinkChangePassword usuario)
        //    {
        //    var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Email == usuario.Email);
        //    if (usuarioDB == null)
        //        {
        //        return Unauthorized("Usuario no registrado");
        //        }

        //    // Creamos un string aleatorio 
        //    Guid miGuid = Guid.NewGuid();
        //    string textoEnlace = Convert.ToBase64String(miGuid.ToByteArray());
        //    // Eliminar caracteres que pueden causar problemas
        //    textoEnlace = textoEnlace.Replace("=", "").Replace("+", "").Replace("/", "").Replace("?", "").Replace("&", "").Replace("!", "").Replace("¡", "");
        //    usuarioDB.EnlaceCambioPass = textoEnlace;
        //    usuarioDB.FechaEnvioEnlace = DateTime.UtcNow;

        //    await _context.SaveChangesAsync();
        //    var ruta = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/changepassword/{textoEnlace}";
        //    return Ok(ruta);
        //    }

        /**
        * 
        * Método LinkChangePasswordHash(string textoEnlace)
        * 
        * Comprueba que el enlace de cambio de contraseña es correcto.
        * 
        * */

        //[HttpGet("/changepassword/{textoEnlace}")]
        //public async Task<ActionResult> LinkChangePasswordHash(string textoEnlace)
        //    {
        //    var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(x => x.EnlaceCambioPass == textoEnlace);
        //    if (usuarioDB == null)
        //        {
        //        return Unauthorized("Operación no autorizada.");
        //        }

        //    return Ok("Enlace correcto");
        //    }

        /**
        * 
        * Método LinkChangePasswordHash([FromBody] DTOUsuarioChangePassword infoUsuario)
        * 
        * Cambia la contraseña.
        * 
        * */

        //[HttpPost("usuarios/changepassword")]
        //public async Task<ActionResult> LinkChangePasswordHash([FromBody] DTOUsuarioChangePassword infoUsuario)
        //    {
        //    var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Email == infoUsuario.Email && x.EnlaceCambioPass == infoUsuario.Enlace);
        //    if (usuarioDB == null)
        //        {
        //        return Unauthorized("Operación no autorizada");
        //        }
        //    var duracionEnlaceMinutos = 30; // Duración del enlace en horas
        //    // FechaEnvioEnlace ahora representa la caducidad del enlace
        //    usuarioDB.FechaEnvioEnlace = DateTime.UtcNow.AddMinutes(duracionEnlaceMinutos);

        //    if (usuarioDB.FechaEnvioEnlace < DateTime.UtcNow)
        //        {
        //        return Unauthorized("Enlace de cambio de contraseña caducado"); // Si el enlace ha caducado, devuelve un error Unauthorized.
        //        }

        //    var resultadoHash = _hashService.Hash(infoUsuario.Password);
        //    usuarioDB.Password = resultadoHash.Hash;
        //    usuarioDB.Salt = resultadoHash.Salt;

        //    await _context.SaveChangesAsync();

        //    await _operacionesService.AddOperacion("Post", "CambioPass");
        //    return Ok("Password cambiado con exito");
        //    }
        }
    }
