using OLEToolBoxWebAPIPruebas.DTOs.DTOForum;
using OLEToolBoxWebAPIPruebas.Models;
using OLEToolBoxWebAPIPruebas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using adaptatechwebapibackend.DTOs.MensajesForo;

namespace OLEToolBoxWebAPIPruebas.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class MessagesForumController : Controller
        {

        /**
        * 
        * Clase controladora sobre el modelo MENSAJES.
        * 
        * Siempre que se refiera en este documento a MENSAJES, 
        * me estaré refiriendo a la tabla en la DATABASE.
        * 
        * */


        private readonly PruebasoletoolboxContext _context;
        private readonly OperationsService _operationsService;

        public MessagesForumController(PruebasoletoolboxContext context, OperationsService operationsService)
            {
            _context = context;
            _operationsService = operationsService;
            }

        /**
         * 
         * Método GetMensajeForos()
         * 
         * Devuelve todos los mensajes en la base de datos.
         * 
         * */
        [HttpGet]
        public async Task<ActionResult<List<MessagesForum>>> GetMensajeForos()
            {
            // Obtiene todos los mensajes del foro
            await _operationsService.AddOperacion("Get", "Mensajes");
            return await _context.MessagesForums.ToListAsync();
            }

        /**
         * 
         * Método GetMensajeForo(int id)
         * 
         * Devuelve un mensaje por ID en la base de datos.
         * 
         * */

        [HttpGet("{id}")]
        public async Task<ActionResult<MessagesForum>> GetMensajeForo(int id)
            {
            // Obtiene un mensaje específico por su ID
            var mensajeForo = await _context.MessagesForums.FindAsync(id);

            if (mensajeForo == null)
                {
                return NotFound();
                }

            await _operationsService.AddOperacion("Get", "MensajesPorId");
            return mensajeForo;
            }

        /**
        * 
        * Método GetMensajeForo(int id)
        * 
        * Devuelve un mensaje por ID en la base de datos.
        * 
        * */

        [HttpGet("mensajesPorTema/{idTemaForo:int}")]
        public async Task<ActionResult<List<DTOMessagesForum>>> GetMensajeForoPorTema(int idTemaForo)
            {
            // Obtiene un mensaje específico por su ID
            var mensajesTemaForo = await _context.MessagesForums.Where(x => x.IdTopic == idTemaForo).Select(x =>
                new DTOMessagesForum
                    {

                    AliasMessageDTO = x.IdUserMessageNavigation.ProfileAlias,
                    IdMessageDTO = x.IdMessage,
                    DateMessageDTO = x.DateMessage,
                    IdUserProfileDTO = x.IdUserProfile,
                    IdTopicDTO = x.IdTopic,
                    TextDTO = x.Text,

                    }).ToListAsync();

            if (mensajesTemaForo == null)
                {
                return NotFound();
                }

            await _operationsService.AddOperacion("Get", "GetMensajesForoPortema");
            return Ok(mensajesTemaForo);
            }

        [HttpGet("mensajesPorTemaNoDTO/{idTemaForo:int}")]
        public async Task<ActionResult<List<MessagesForum>>> GetMensajesForoPorTemaSinDTO(int idTemaForo)
            {
            // Obtiene un mensaje específico por su ID
            var mensajesTemaForo = await _context.MessagesForums.Where(x => x.IdTopic == idTemaForo).ToListAsync();

            if (mensajesTemaForo == null)
                {
                return NotFound();
                }

           // await _operationsService.AddOperacion("Get", "GetMensajesForoPortema");
            return Ok(mensajesTemaForo);
            }

        // Obtener mensajes por alias
        //[HttpGet("{alias}")]
        //public async Task<ActionResult<MensajeForo>> GetMensajeForoAias(string alias)
        //{
        //    // Obtiene un mensaje específico por su alias
        //    var mensajeForo = await _context.MensajeForos.FindAsync(alias);

        //    if (mensajeForo == null)
        //    {
        //        return NotFound();
        //    }

        //    return mensajeForo;
        //}

        /**
         * 
         * Método GetMensajeForoAias(string alias)
         * 
         * Devuelve un mensaje por ALIAS en la base de datos.
         * 
         * */
        [HttpGet("alias/{alias}")]
        public async Task<ActionResult<MessagesForum>> GetMensajeForoAias(string alias)
            {
            // Obtiene un mensaje específico por su alias
            var mensajeForo = await _context.MessagesForums.Include(x => x.IdUserMessageNavigation)
                .Where(y => y.IdUserMessageNavigation.ProfileAlias.Equals(alias)).FirstOrDefaultAsync();


            if (mensajeForo == null)
                {
                return NotFound();
                }

            await _operationsService.AddOperacion("Get", "MensajesPorAlias");
            return mensajeForo;
            }


        /**
         * 
         * Método PostMensajeForo(MensajeForo mensajeForo)
         * 
         * Inserta un mensaje en la base de datos.
         * 
         * */
        [HttpPost("agregarmensaje")]
        public async Task<ActionResult<MessagesForum>> PostMensajeForo(MessagesForum mensajeForo)
            {
            // Crea un nuevo mensaje en el foro
            _context.MessagesForums.Add(mensajeForo);
            await _context.SaveChangesAsync();

            //await _operationsService.AddOperacion("Post", "NuevoMensaje");
            return CreatedAtAction("GetMensajeForo", new { id = mensajeForo.IdMessage }, mensajeForo);
            }

        /**
         * 
         * Método AgregarMensaje([FromBody] DTOMensajeForo mensaje)
         * 
         * Inserta un mensaje en la base de datos validando ciertas cuestiones(Usuario, Perfil, Tema).
         * 
         * */

        [HttpPost("postmensajes")]
        public async Task<ActionResult<List<MessagesForum>>> AgregarMensaje([FromBody] DTOMessagesForum mensaje)
            {
            try
                {
                // Verificar si el usuario que envía el mensaje existe en la base de datos


                //Verificar si el perfil de usuario asociado al mensaje existe en la base de datos
                var perfilUsuarioExistente = await _context.UserProfiles.FirstOrDefaultAsync(p => p.ProfileId == mensaje.IdUserProfileDTO);
                if (perfilUsuarioExistente == null)
                    {
                    return BadRequest("El perfil de usuario especificado no existe.");
                    }

                //Verificar si el tema al que pertenece el mensaje existe en la base de datos
                var temaExistente = await _context.TopicsForums.FirstOrDefaultAsync(t => t.IdTopic == mensaje.IdTopicDTO);
                if (temaExistente == null)
                    {
                    return BadRequest("El tema especificado no existe.");
                    }

                // Si todos los elementos existen, se crea el nuevo mensaje
                var nuevoMensaje = new MessagesForum
                    {
                    IdUserProfile = mensaje.IdUserProfileDTO,
                    IdTopic = mensaje.IdTopicDTO,
                    Text = mensaje.TextDTO,
                    DateMessage = DateTime.Now,
                    AliasMessage = mensaje.AliasMessageDTO,


                    };

                // Agregar el mensaje a la base de datos
                _context.MessagesForums.Add(nuevoMensaje);
                await _context.SaveChangesAsync();

                //   await _operationsService.AddOperacion("Post", "AgregarMensaje");
                return Created("MensajeForo", new { mensaje = nuevoMensaje });
                }
            catch (Exception ex)
                {
                return StatusCode(500, $"Error al agregar el mensaje: {ex.Message}");
                }
            }


        // PUT: api/MensajeForo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMensajeForo(int id, MessagesForum mensajeForo)
            {
            if (id != mensajeForo.IdMessage)
                {
                return BadRequest();
                }

            var mensajeUpdate = await _context.MessagesForums.FindAsync(id);

            if (mensajeUpdate == null)
                return NotFound();

            mensajeUpdate.IdTopic = mensajeForo.IdTopic;
            mensajeUpdate.Text = mensajeForo.Text;

            _context.Update(mensajeUpdate);

            await _context.SaveChangesAsync();

            await _operationsService.AddOperacion("Put", "ModificarMensaje");
            return Accepted("Datos actualizados");
            }

        // DELETE: api/MensajeForo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMensajeForo(int id)
            {
            // Elimina un mensaje del foro por su ID
            var mensajeForo = await _context.MessagesForums.FindAsync(id);
            if (mensajeForo == null)
                {
                return NotFound();
                }

            _context.MessagesForums.Remove(mensajeForo);
            await _context.SaveChangesAsync();

            await _operationsService.AddOperacion("Delete", "DeleteMEnsajePorId");
            return NoContent();
            }

        // GET: api/MensajeForo/ByPerfilUsuario/5
        [HttpGet("ByPerfilUsuario/{perfilUsuarioId}")]
        public async Task<ActionResult<IEnumerable<MessagesForum>>> GetMensajesByPerfilUsuario(int perfilUsuarioId)
            {
            // Obtiene todos los mensajes asociados a un perfil de usuario específico
            var mensajesByPerfil = await _context.MessagesForums
                .Where(m => m.IdUserProfile == perfilUsuarioId)
                .ToListAsync();

            if (mensajesByPerfil == null || mensajesByPerfil.Count == 0)
                {
                return NotFound($"No se encontraron mensajes para el perfil de usuario con ID {perfilUsuarioId}.");
                }

            await _operationsService.AddOperacion("Get", "MensajesPorPerfilDeUsuario");
            return mensajesByPerfil;
            }

        private bool MensajeForoExists(int id)
            {
            // Verifica si un mensaje del foro con el ID proporcionado existe en la base de datos
            return _context.MessagesForums.Any(e => e.IdUserProfile == id);
            }

        }
    }
