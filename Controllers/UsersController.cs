using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Newtonsoft.Json;
using OLEToolBoxWebAPIPruebas.DTOs.DTOArchivos.adaptatechwebapibackend.DTOs;
using OLEToolBoxWebAPIPruebas.DTOs.DTOUsers;
using OLEToolBoxWebAPIPruebas.Models;
using OLEToolBoxWebAPIPruebas.Services;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OLEToolBoxWebAPIPruebas.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
        {

        private PruebasoletoolboxContext _context;
        private FileStorageManager _fileStorageManager;
        private HashService _hashService;


        /**
         * 
         * Constructor principal.
         * 
         * Es posible inyectar servicios.
         * 
         * No hay servicios desarrollados aún.
         * 
         * */

        public UsersController(PruebasoletoolboxContext context, FileStorageManager fileStorageManager, HashService hashService)
            {

            _context = context;
            _fileStorageManager = fileStorageManager;
            _hashService = hashService;
            }

        /**
         * 
         * Método GetUserCredentials()
         * 
         * Devuelve una lista con todas las credenciales en la base de datos.
         * 
         * */

        [HttpGet("getcredentials")]

        public async Task<ActionResult<List<UserCredential>>> GetUserCredentials()
            {

            var credentialsList = await _context.UserCredentials.ToListAsync();

            if (credentialsList.Count() == 0) return NotFound();


            return Ok(credentialsList);

            }

        [HttpGet("getcredentialsprofiles")]

        public async Task<ActionResult<List<UserProfile>>> GetUserCredentialsProfiles()
            {

            var profilesList = await _context.UserProfiles.Include(c => c.Credentials).ToListAsync();

            if (profilesList.Count() == 0) return NotFound();


            return Ok(profilesList);

            }

        [HttpGet("getcredentialsadmin")]

        public async Task<ActionResult<List<UserCredential>>> GetUserCredentialsAdmin()
            {

            var credentialsList = await _context.UserCredentials.FirstOrDefaultAsync();

            if (credentialsList is null) return NotFound();


            return Ok(credentialsList);

            }

        /**
         * 
         * Método PostUserCredential([FromBody] DTOUserCredentialsPost credentials)
         * 
         * Agrega una nueva credencial a la base de datos.
         * 
         * */

        [HttpPost("postcredentialsadmin")]
        public async Task<ActionResult> PostUserCredential([FromBody] DTOUserCredentialsPostAdmin credentials)
            {

            var strAdmin = "admin@olesystem";

            var credentialExists = await _context.UserCredentials.Where(c => c.Email.Equals(strAdmin)).AnyAsync();

            if (credentialExists)
                {

                return BadRequest("Ya hay admin user.");

                }


            var resultadoHash = _hashService.Hash(credentials.CredPasswordDTO);

            UserCredential newCredentials = new UserCredential()
                {
                Email = strAdmin,
                CredPassword = resultadoHash.Hash,
                Salt = resultadoHash.Salt,
                };

            await _context.AddAsync(newCredentials);
            await _context.SaveChangesAsync();

            return Created("Credentials", new
                {
                credentials = newCredentials
                });


            }


        [HttpPost("postcredentials")]
        public async Task<ActionResult> PostUserCredential([FromBody] DTOUserCredentialsPost credentials)
            {

            var credentialExists = await _context.UserCredentials.Where(c => c.Email.Equals(credentials.EmailDTO)).AnyAsync();

            if (credentialExists)
                {

                return BadRequest("Ya hay un email igual en la credencial de algún/a usuaria/o.");

                }


            var resultadoHash = _hashService.Hash(credentials.CredPasswordDTO);

            UserCredential newCredentials = new UserCredential()
                {
                Email = credentials.EmailDTO.ToString(),
                CredPassword = resultadoHash.Hash,
                Salt = resultadoHash.Salt,
                };

            await _context.AddAsync(newCredentials);
            await _context.SaveChangesAsync();

            return Created("Credentials", new { credentials = newCredentials });


            }

        /**
         * 
         * Método PutUserCredentials([FromRoute] int id, [FromBody] DTOUserCredentialsPut update)
         * 
         * Actualiza los datos de una de las credenciales de usuaria/o ya existentes.
         * 
         * */

        [HttpPut("putcredentials/{id:int}")]
        public async Task<ActionResult> PutUserCredentials([FromRoute] int id, [FromBody] DTOUserCredentialsPut update)
            {

            if (id != update.CredId) return BadRequest("Los ids proporcionados no son iguales.");

            var credentialUpdate = await _context.UserCredentials.AsTracking().FirstOrDefaultAsync(r => r.CredId == id);
            if (credentialUpdate == null)
                {
                return NotFound("No existen credencales con ese id.");
                }

            credentialUpdate.Email = update.EmailDTO;
            credentialUpdate.CredPassword = update.CredPasswordDTO;

            _context.Update(credentialUpdate);

            await _context.SaveChangesAsync();
            return Accepted("Datos actualizados");
            }

        /**
         * 
         * Método DeleteCredentials(int id)
         * 
         * Elimina unas credenciales de la base de datos con una id proporcionada.
         * 
         * */

        [HttpDelete("deletecredentials/{id:int}")]
        public async Task<ActionResult> DeleteCredentials(int id)
            {

            var credentials = await _context.UserCredentials.FindAsync(id);

            if (credentials is null)
                {
                return NotFound("Las credenciales con ese id no existe.");
                }

            if (credentials.UserProfiles.Count() != 0) return BadRequest("Existe un perfil con estas credenciales");


            _context.Remove(credentials);
            await _context.SaveChangesAsync();
            return Ok();
            }

        [HttpGet("getprofilesbyid/{id:int}")]
        public async Task<ActionResult<List<UserProfile>>> GetUserProfiles([FromRoute] int id)
            {

            var profilesList = await _context.UserProfiles.Include(p => p.Credentials).Where(c => c.ProfileId == id).FirstOrDefaultAsync();

            if (profilesList is null) return NotFound("No se encontró el perfil");


            return Ok(profilesList);

            }

        /**
         * 
         * Método GetUserProfiles()
         * 
         * Devuelve una lista con todas las credenciales en la base de datos.
         * 
         * */

        [HttpGet("getprofilesbyemail/{email}")]
        public async Task<ActionResult<List<UserProfile>>> GetUserProfiles([FromRoute] string email)
            {

            var profilesList = await _context.UserProfiles.Include(p => p.Credentials).Where(c => c.Credentials.Email.Equals(email)).FirstOrDefaultAsync();

            if (profilesList is null) return NotFound("No se encontró el perfil");


            return Ok(profilesList);

            }

        [HttpGet("getprofiles")]
        public async Task<ActionResult<List<UserProfile>>> GetUserProfiles()
            {

            var profilesList = await _context.UserProfiles.ToListAsync();

            if (profilesList.Count() == 0) return NotFound();


            return Ok(profilesList);

            }

        /**
        * 
        * Método PostUserProfiles([FromBody] DTOUserProfilePost profile)
        * 
        * Agrega una nuevo perfil a la base de datos.
        * 
        * 
        * */


        [HttpPost("postprofiles")]
        public async Task<ActionResult> PostUserProfile([FromForm] DTOUserProfilePost profile)
            {

            var profileExists = await _context.UserProfiles.Include(p => p.ProfileRole).Where(c => c.ProfileName.Equals(profile.ProfileNameDTO)
            && c.ProfileRoleNavigation.RoleId == profile.ProfileRoleDTO).AnyAsync();

            if (profileExists)
                {

                return BadRequest("Ya hay un perfil con igual nombre y rol en la base de datos.");

                }

            UserProfile newProfile = new UserProfile()
                {
                ProfileName = profile.ProfileNameDTO,
                ProfileApel = profile.ProfileApelDTO,
                ProfileRole = profile.ProfileRoleDTO,
                ProfileAlias = profile.ProfileAliasDTO,
                ProfileAvatar = "",
                };

            if (profile.ProfileAvatarDTO is not null)
                {

                using (var memoryStream = new MemoryStream())
                    {
                    // Extraemos la imagen de la petición
                    await profile.ProfileAvatarDTO.CopyToAsync(memoryStream);
                    // La convertimos a un array de bytes que es lo que necesita el método de guardar
                    var contenido = memoryStream.ToArray();
                    DTOArchivo archivo = new DTOArchivo
                        {
                        Nombre = profile.ProfileAvatarDTO.FileName,
                        Contenido = contenido,
                        Carpeta = "imagenes",
                        ContentType = profile.ProfileAvatarDTO.ContentType
                        };


                    var nombreArchivo = await _fileStorageManager.SaveFile(archivo.Contenido, archivo.Nombre, archivo.Carpeta, archivo.ContentType);

                    newProfile.ProfileAvatar = nombreArchivo;

                    }

                };

            await _context.AddAsync(newProfile);
            await _context.SaveChangesAsync();

            return Created("Profile", new { profile = newProfile });


            }

        /**
        * 
        * Método PutUserProfiles([FromBody] DTOUserProfilePut profile)
        * 
        * Modifica una nuevo perfil a la base de datos.
        * 
        * */

        [HttpPost("profiles/servicioimageneslocal")]
        public async Task<ActionResult> PostUserProfileImageService([FromForm] DTOUserProfilePost profile)
            {
            UserProfile newProfile = new UserProfile
                {
                ProfileName = profile.ProfileNameDTO,
                ProfileApel = profile.ProfileApelDTO,
                Birthday = profile.BirthdayDTO,
                ProfileAlias = profile.ProfileAliasDTO,
                ProfileRole = profile.ProfileRoleDTO,
                CredentialsId = profile.CredentialsIdDTO,
                ProfileAvatar = ""
                };

            if (profile.ProfileAvatarDTO is not null)
                {

                using (var memoryStream = new MemoryStream())
                    {
                    // Extraemos la imagen de la petición
                    await profile.ProfileAvatarDTO.CopyToAsync(memoryStream);
                    // La convertimos a un array de bytes que es lo que necesita el método de guardar
                    var contenido = memoryStream.ToArray();
                    DTOArchivo archivo = new DTOArchivo
                        {
                        Nombre = profile.ProfileAvatarDTO.FileName,
                        Contenido = contenido,
                        Carpeta = "imagenes",
                        ContentType = profile.ProfileAvatarDTO.ContentType
                        };

                    var fileName = await _fileStorageManager.SaveFile(archivo.Contenido, archivo.Nombre, archivo.Carpeta, archivo.ContentType);

                    newProfile.ProfileAvatar = fileName;

                    }

                }

            //await _operacionesService.AddOperacion("Post", "Dreamers");


            await _context.AddAsync(newProfile);
            await _context.SaveChangesAsync();

            return Ok(newProfile);
            }

        [HttpPut("profiles/modificarconimagenes/{id:int}")]
        public async Task<ActionResult> PutUserProfile([FromRoute] int id, DTOUserProfilesPut profile)
            {
            if (id != profile.IdProfileDTO)
                {
                return BadRequest("Los ids proporcionados son diferentes");
                }
            var profileUpdate = await _context.UserProfiles.AsTracking().FirstOrDefaultAsync(x => x.ProfileId == id);
            if (profileUpdate == null)
                {
                return NotFound("No se encontró perfil con ese id.");
                }
            profileUpdate.ProfileName = profile.ProfileNameDTO;
            profileUpdate.ProfileApel = profile.ProfileApelDTO;
            profileUpdate.ProfileAlias = profile.ProfileAliasDTO;
            profileUpdate.Birthday = profile.BirthdayDTO;
            profileUpdate.ProfileRole = profile.ProfileRoleDTO;

            if (profile.ProfileAvatarDTO != null)
                {
                using (var memoryStream = new MemoryStream())
                    {
                    // Extraemos la imagen de la petición
                    await profile.ProfileAvatarDTO.CopyToAsync(memoryStream);
                    // La convertimos a un array de bytes que es lo que necesita el método de guardar
                    var contenido = memoryStream.ToArray();
                    DTOArchivo archivo = new DTOArchivo
                        {
                        Nombre = profile.ProfileAvatarDTO.FileName,
                        Contenido = contenido,
                        Carpeta = "imagenes",
                        ContentType = profile.ProfileAvatarDTO.ContentType
                        };

                    var fileName = await _fileStorageManager.SaveFile(archivo.Contenido, archivo.Nombre, archivo.Carpeta, archivo.ContentType);

                    profileUpdate.ProfileAvatar = fileName;
                    }
                }

            _context.Update(profileUpdate);

            //await _operacionesService.AddOperacion("Put", "Dreamer");


            await _context.SaveChangesAsync();
            return NoContent();
            }



        [HttpDelete("deleteuserprofile/{id:int}")]
        public async Task<ActionResult> DeleteUserProfile(int id)
            {

            var profile = await _context.UserProfiles.Where(p => p.ProfileId == id).FirstOrDefaultAsync();

            if (profile is null) return NotFound("El id no figura en la base de datos.");


            _context.Remove(profile);
            await _context.SaveChangesAsync();

            return NoContent();

            }

        //[HttpPut("modificarconimagenes/{id:int}")]
        //public async Task<ActionResult> PutDreamer([FromRoute] int id, DTODreamerPut dreamer)
        //    {
        //    if (id != dreamer.IdDreamer)
        //        {
        //        return BadRequest("Los ids proporcionados son diferentes");
        //        }
        //    var dreamerUpdate = await _context.Dreamers.AsTracking().FirstOrDefaultAsync(x => x.IdDreamer == id);
        //    if (dreamerUpdate == null)
        //        {
        //        return NotFound();
        //        }
        //    dreamerUpdate.NameDreamer = dreamer.NameDTO;
        //    dreamerUpdate.AgeDreamer = dreamer.AgeDTO;
        //    dreamerUpdate.IdUser = dreamer.IdUserDTO;
        //    dreamerUpdate.Birthday = dreamer.BirthdayDTO;
        //    dreamerUpdate.Avatar = "";

        //    var client = new HttpClient();
        //    if (dreamer.AvatarDTO != null)
        //        {
        //        using (var memoryStream = new MemoryStream())
        //            {
        //            // Extraemos la imagen de la petición
        //            await dreamer.AvatarDTO.CopyToAsync(memoryStream);
        //            // La convertimos a un array de bytes que es lo que necesita el método de guardar
        //            var contenido = memoryStream.ToArray();
        //            DTOArchivo archivo = new DTOArchivo
        //                {
        //                Nombre = dreamer.AvatarDTO.FileName,
        //                Contenido = contenido,
        //                Carpeta = "imagenes",
        //                ContentType = dreamer.AvatarDTO.ContentType
        //                };
        //            // Comienza la petición
        //            // Es una post que, como toda petición post, va a incluir un body
        //            // El body es el DTOArchivo que hemos llamado archivo
        //            // Debemos convertir el objeto archivo a json (siguiente línea)
        //            var body = JsonConvert.SerializeObject(archivo);
        //            // Pasar ese json a un string (siguiente línea)
        //            var stringContent = new StringContent(body, UnicodeEncoding.UTF8, "application/json");
        //            // Emitimos la petición. response es la respuesta con el nombre del archivo
        //            var response = await client.PostAsync("https://localhost:7296/api/Archivos", stringContent);
        //            // Esa respuesta debemos leerla y convertirla en este caso a un string
        //            var nombreArchivo = await response.Content.ReadAsStringAsync();
        //            // El nombre del archivo lo pasamos al producto
        //            dreamerUpdate.Avatar = nombreArchivo;
        //            }
        //        }

        //    _context.Update(dreamerUpdate);

        //    await _operacionesService.AddOperacion("Put", "Dreamer");


        //    await _context.SaveChangesAsync();
        //    return NoContent();
        //    }

        /**
         * 
         * Método GetUsersData()
         * 
         * Devuelve una lista con todas las credenciales en la base de datos.
         * 
         * */

        [HttpGet("getdata")]
        public async Task<ActionResult<List<UsersDatum>>> GetUsersData()
            {

            var dataList = await _context.UsersData.ToListAsync();

            if (dataList.Count() == 0) return NotFound();

            return Ok(dataList);

            }

        /**
         * 
         * Método PostUserDatum([FromBody] DTOUserDataPost userData)
         * 
         * Agrega un nuevo rol a la base de datos.
         * 
         **/

        [HttpPost("postdata")]
        public async Task<ActionResult> PostData([FromBody] DTOUserDataPost userData)
            {

            var cred = await _context.UserCredentials.FindAsync(userData.CredIdDTO);

            if (cred is null)
                {

                return BadRequest("No existen credenciales con esa ID.");

                }

            var profile = await _context.UserProfiles.FindAsync(userData.ProfileIdDTO);


            if (profile is null)
                {

                return BadRequest("No existen credenciales con esa ID.");

                }

            var existsConnection = cred.CredId == profile.Credentials.CredId;

            if (existsConnection)
                {

                UsersDatum datum = new UsersDatum()
                    {

                    CredId = userData.CredIdDTO,
                    ProfileId = userData.ProfileIdDTO,

                    };

                await _context.AddAsync(datum);
                await _context.SaveChangesAsync();

                return Created("Datum", new { datum = userData });

                }
            else return BadRequest("No se pudo realizar la transacción.");
            }


        /**
         * 
         * Método PutUserDatum([FromRoute] int id, [FromBody] DTORolePut update)
         * 
         * Actualiza los datos de uno de los roles de sistema ya existentes.
         * 
         * */

        [HttpPut("putdata/{id:int}")]
        public async Task<ActionResult> PutUserDatum([FromRoute] int id, [FromBody] DTOUserDataPut userData)
            {

            if (id != userData.IdUserDatumDTO) return BadRequest("Los ids proporcionados no son iguales.");

            var cred = await _context.UserCredentials.FindAsync(userData.CredIdDTO);

            if (cred is null)
                {

                return BadRequest("No existen credenciales con esa ID.");

                }

            var profile = await _context.UserProfiles.FindAsync(userData.ProfileIdDTO);


            if (profile is null)
                {

                return BadRequest("No existen credenciales con esa ID.");

                }

            var existsConnection = cred.CredId == profile.Credentials.CredId;

            var userDataUpdate = await _context.UsersData.AsTracking().FirstOrDefaultAsync(x => x.DataId == id);

            if (userDataUpdate is null)
                {

                return BadRequest("No existen datos de relación con esa ID.");

                }

            userDataUpdate.CredId = userData.CredIdDTO;
            userDataUpdate.ProfileId = userData.CredIdDTO;

            _context.Update(userDataUpdate);
            await _context.SaveChangesAsync();

            return Accepted("Datos de relación actualizados");
            }

        /**
        * 
        * Método DeleteDatum(int id)
        * 
        * Elimina una relación de la base de datos con una id proporcionada.
        * 
        * */

        [HttpDelete("deletedata/{id:int}")]
        public async Task<ActionResult> DeleteDatum(int id)
            {

            var datum = await _context.Roles.FindAsync(id);

            if (datum is null)
                {
                return NotFound("El rol con ese id no existe.");
                }

            _context.Remove(datum);
            await _context.SaveChangesAsync();
            return Ok("Datos de relación eliminados.");
            }


        /**
         * 
         * Método GetRoles()
         * 
         * Devuelve una lista con todos los roles en la base de datos.
         * 
         * */

        [HttpGet("getroles")]
        public async Task<ActionResult<List<Role>>> GetRoles()
            {

            var roleList = await _context.Roles.ToListAsync();

            if (roleList.Count() == 0) return NotFound();

            return Ok(roleList);

            }


        /**
         * 
         * Método PostRole([FromBody] DTORolePost role)
         * 
         * Agrega un nuevo rol a la base de datos.
         * 
         * */

        [HttpPost("postroles")]
        public async Task<ActionResult> PostRole([FromBody] DTORolePost role)
            {

            var roleExists = await _context.Roles.Where(r => r.RoleDescription.Equals(role.RoleDescriptionDTO)).AnyAsync();

            if (roleExists)
                {

                return BadRequest("Ya hay un rol de sistema con esa descripción.");

                }

            Role newRole = new Role()
                {

                RoleDescription = role.RoleDescriptionDTO,

                };

            await _context.AddAsync(newRole);
            await _context.SaveChangesAsync();

            return Created("Role", new { role = newRole });


            }

        /**
         * 
         * Método PutRoles([FromRoute] int id, [FromBody] DTORolePut update)
         * 
         * Actualiza los datos de uno de los roles de sistema ya existentes.
         * 
         * */

        [HttpPut("putroles/{id:int}")]
        public async Task<ActionResult> PutRoles([FromRoute] int id, [FromBody] DTORolePut update)
            {

            if (id != update.RoleIdDTO) return BadRequest("Los ids proporcionados no son iguales.");

            var roleUpdate = await _context.Roles.AsTracking().FirstOrDefaultAsync(r => r.RoleId == id);
            if (roleUpdate == null)
                {
                return NotFound("No existe rol con ese id.");
                }

            roleUpdate.RoleDescription = update.RoleDescriptionDTO;

            _context.Update(roleUpdate);

            await _context.SaveChangesAsync();
            return Accepted("Datos actualizados");
            }

        /**
         * 
         * Método DeleteRole(int id)
         * 
         * Elimina un rol de la base de datos con una id proporcionada.
         * 
         * */

        [HttpDelete("deleterole/{id:int}")]
        public async Task<ActionResult> DeleteRole(int id)
            {

            var role = await _context.Roles.FindAsync(id);

            if (role is null)
                {
                return NotFound("El rol con ese id no existe.");
                }

            _context.Remove(role);
            await _context.SaveChangesAsync();
            return Ok();
            }

        }
    }
