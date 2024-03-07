using OLEToolBoxWebAPIPruebas.DTOs;
using OLEToolBoxWebAPIPruebas.Models;
using OLEToolBoxWebAPIPruebas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OLEToolBoxWebAPIPruebas.DTOs.DTOForum;

namespace OLEToolBoxWebAPIPruebas.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class TopicsForumController : ControllerBase
        {

        /**
         * 
         * Clase controladora sobre el modelo TEMA_FORO.
         * 
         * Siempre que se refiera en este documento a TEMA_FORO, 
         * me estaré refiriendo a la tabla en la DATABASE.
         * 
         * */


        //Variable objeto _context
        //Instanciamos el contexto de Entity Framework.Temas

        private readonly PruebasoletoolboxContext _context;
        private readonly OperationsService _operationsService;


        /**
         * 
         * Constructor con un parámetro de inyección de servicio SCOPED
         * 
         * */

        public TopicsForumController(PruebasoletoolboxContext context, OperationsService operationsService)
            {
            _context = context;
            _operationsService = operationsService;
            }

        /**
         * 
         * Método GetTemasForo()
         * 
         * Devuelve todos los temas del foro en la base de datos.
         * 
         * */

        [HttpGet]
        public async Task<ActionResult<List<TopicsForum>>> GetTemasForo()
            {

            var lista = await _context.TopicsForums.ToListAsync();

            if (lista.Count() == 0) return NotFound();

            await _operationsService.AddOperacion("Get", "ListaDeTemas");
            return Ok(lista);

            }

        /**
         * 
         * Método GetTemasForo()
         * 
         * Devuelve todos los temas del foro en la base de datos con sus correspondientes mensajes.
         * 
         * */

        [HttpGet("listTemasConMensajes")]
        public async Task<ActionResult<List<TopicsForum>>> GetTemasForoInclude()
            {

            var lista = await _context.TopicsForums.Include(x => x.MessagesForums).ToListAsync();

            if (lista.Count() == 0) return NoContent();


            await _operationsService.AddOperacion("Get", "ListaDeTemasConMensajes");
            return lista;

            }

        /**
         * 
         * Método GetTemasForoSincrono()
         * 
         * Devuelve todos los perfiles de usuario de forma síncrona.
         * 
         * */

        [HttpGet("sincrono")]
        public ActionResult<List<TopicsForum>> GetTemasForoSincrono()
            {
            // Las operaciones contra una base de datos DEBEN SER SIEMPRE ASÍNCRONAS. Para liberar los hilos de ejecución en cada petición, eso no debe hacerse nunca
            var lista = _context.TopicsForums.ToList();

            if (lista.Count() == 0) return NoContent();


            _operationsService.AddOperacion("Get", "TemasSíncronos");
            return lista;
            }

        /**
        * 
        * Método GetTemasForoOrdenadosAsc()
        * 
        * Devuelve todos los temas del foro ordenados ascendentemente por fecha de creación.
        * 
        * */

        [HttpGet("ordenadosfechaascendente")]
        public async Task<ActionResult<List<TopicsForum>>> GetTemasForoOrdenadosAsc()
            {
            var listaOrdenadaAscendente = await _context.TopicsForums.OrderBy(x => x.DateCreated).ToListAsync();

            if (listaOrdenadaAscendente.Count() == 0) return NoContent();


            await _operationsService.AddOperacion("Get", "TemasOrdenadosAscendentes");
            return listaOrdenadaAscendente;
            }

        /**
         * 
         * Método GetTemasForoOrdenadosDesc()
         * 
         * Devuelve todos los temas del foro ordenados descendentemente por fecha de creación.
         * 
         * */

        [HttpGet("ordenadosfechadescendente")]
        public async Task<ActionResult<List<TopicsForum>>> GetTemasForoOrdenadosDesc()
            {
            var listaOrdenadaDescendente = await _context.TopicsForums.OrderByDescending(x => x.DateCreated).ToListAsync();

            await _operationsService.AddOperacion("Get", "TemasOrdenadosDescendentes");
            return listaOrdenadaDescendente;
            }

        /**
         * 
         * Método GetTemaForoPorId([FromRoute] int id)
         * 
         * Devuelve un tema del foro correspondiente a una id de TEMAS_FORO.
         * 
         * */

        [HttpGet("temasPorId/{id:int}")]
        public async Task<ActionResult<List<TopicsForum>>> GetTemaForoPorId([FromRoute] int id)
            {
            var tema = await _context.TopicsForums.FindAsync(id);

            if (tema == null)
                {
                return NotFound("El tema con " + id + " no existe.");
                }

            await _operationsService.AddOperacion("Get", "TemasPorId");
            return Ok(tema);
            }


        /**
         * 
         * Método GetTemasForoMensajesSelect()
         * 
         * Devuelve todos los temas del foro con sus correspondientes mensajes.
         * 
         * Queda añadir el MensajesController
         * 
         * 
         * */


        //[HttpGet("temasmensajes/{id}")]
        //public async Task<ActionResult<List<DTOTemasMensajes>>> GetTemasForoMensajesSelect()
        //    {

        //    var temas = await (from x in _context.TemasForos.Include(y => y.MensajeForos)
        //                       select new DTOTemasMensajes
        //                           {
        //                           IdTemaDTO = x.IdTema,
        //                           TituloDTO = x.Titulo!,
        //                           MensajesDTO = x.MensajeForos.Select(y => new DTOMensajesItem
        //                               {

        //                               }).ToList(),

        //                           }).ToListAsync();

        //    if (temas.Count() == 0)
        //        {
        //        return NotFound("No hay datos de temas de foros");
        //        }

        //    return Ok(temas);
        //    }

        /**

Método GetTemaForoMensajesSelectId(int id)
Devuelve un tema del foro con sus correspondientes mensajes.
*/

        //[HttpGet("temamensajes/{id:int}")]
        //public async Task<ActionResult<DTOTemasMensajes>> GetTemaForoMensajesSelectId(int id)
        //{

        //    var tema = await (from x in _context.TemasForos.Include(y => y.MensajeForos)
        //                      select new DTOTemasMensajes
        //                      {
        //                          IdTemaDTO = x.IdTema,
        //                          TituloDTO = x.Titulo!,
        //                          MensajesDTO = x.MensajeForos.Select(y => new DTOMensajesItem
        //                          {
        //                              IdUsuariomensajeDTO = y.IdUsuariomensaje,
        //                              IdPerfilUsuariomensajeDTO = y.IdPerfilUsuariomensaje,
        //                              IdTemaDTO = y.IdTema,
        //                              TextoDTO = y.Texto!,
        //                              FechaMensajeDTO = y.FechaMensaje

        //                          }).ToList(),

        //                      }).FirstOrDefaultAsync(z => z.IdTemaDTO == id);

        //    if (tema == null)
        //    {
        //        return NotFound("No hay tema con ese Id");
        //    }

        //    return Ok(tema);
        //}

        [HttpGet("temasmensajes/{id}")]
        public async Task<ActionResult<List<DTOTopicMessages>>> GetTemasForoMensajesSelect(int id)
            {
            try
                {
                var temas = await _context.TopicsForums
                    .Include(t => t.MessagesForums)
                    .Where(t => t.IdTopic == id)
                    .Select(x => new DTOTopicMessages
                        {
                        IdTopicDTO = x.IdTopic,
                        TitleDTO = x.Title!,
                        MensajesDTO = x.MessagesForums.Select(y => new DTOMessagesItem
                            {
                            //IdMensajeDTO = y.IdMensaje,
                            IdUserMessageDTO = y.IdUserMessage,
                            IdUserProfileDTO = y.IdUserProfile,
                            IdTopicDTO = y.IdTopic,
                            TextDTO = y.Text!,
                            DateDTO = y.DateMessage
                            }).ToList()
                        })
                    .ToListAsync();

                if (temas.Count == 0)
                    {
                    return NotFound("No hay datos de temas de foros");
                    }

                await _operationsService.AddOperacion("Get", "TemasMensajes");
                return Ok(temas);
                }
            catch (Exception ex)
                {
                // Manejo de excepciones, registra el error para debugging
                Console.WriteLine($"Error al obtener temas y mensajes: {ex.Message}");
                return StatusCode(500, "Error al obtener temas y mensajes");
                }
            }



        [HttpGet("temasmensajes")]
        public async Task<ActionResult<List<TopicsForum>>> GetTemasForoMensajesSelect()
            {

            var temas = await (from x in _context.TopicsForums.Include(y => y.MessagesForums)
                               select new DTOTopicMessages
                                   {
                                   IdTopicDTO = x.IdTopic,
                                   TitleDTO = x.Title!,
                                   MensajesDTO = x.MessagesForums.Select(y => new DTOMessagesItem
                                       {
                                       IdUserMessageDTO = y.IdUserMessage,
                                       IdUserProfileDTO = y.IdUserProfile,
                                       IdTopicDTO = y.IdTopic,
                                       TextDTO = y.Text!,
                                       DateDTO = y.DateMessage

                                       }).ToList(),

                                   }).ToListAsync();

            if (temas.Count() == 0)
                {
                return NotFound("No hay datos de temas de foros");
                }

            await _operationsService.AddOperacion("Get", "TemasConMensajes");
            return Ok(temas);
            }





        /**
         * 
         * Método PostPerfilUsuario([FromRoute] int id, DTOPerfilUsuarioPost perfil)
         * 
         * Añade un tema a la tabla TEMA_FORO.
         *
         * 
         * */

        [HttpPost("nuevoTema")]
        public async Task<ActionResult> PostTemaForo([FromBody] DTOTopicsForumPost tema)
            {
            var newTema = new TopicsForum()
                {

                IdUserTopic = tema.IdUserProfileTopicDTO,
                Title = tema.TitleDTO,
                DateCreated = tema.DateCreatedDTO,


                };
            await _context.AddAsync(newTema);
            await _context.SaveChangesAsync();

            await _operationsService.AddOperacion("Post", "NuevoMensaje");
            return Created("TemaForo", new { tema = newTema });
            }

        /**
         * 
         * Método PutTemaForo([FromRoute] int id, DTOPerfilUsuarioPut perfil)
         * 
         * Actualiza la información o módifica la información de un registro de PERFIL_USUARIO en la base de datos.
         * 
         * */

        [HttpPut("modificarTema/{id}")]
        public async Task<ActionResult> PutPerfilUsuario([FromRoute] int id, DTOTopicsForumPut tema)
            {
            if (id != tema.IdTopicDTO)
                {
                return BadRequest("Los ids proporcionados son diferentes");
                }
            var perfilUpdate = await _context.TopicsForums.AsTracking().FirstOrDefaultAsync(x => x.IdTopic == id);
            if (perfilUpdate == null)
                {

                return NotFound();
                }



            _context.Update(perfilUpdate);

            await _context.SaveChangesAsync();

            await _operationsService.AddOperacion("Put", "ModificarTema");
            return NoContent();
            }

        /**
         * 
         * Método DeleteTemaForoId(int id)
         * 
         * Elimina un perfil de usuario correspondiente a una id de PERFIL_USUARIO.
         * 
         * */

        [HttpDelete("borrarTema/{id}")]
        public async Task<ActionResult> DeleteTemaForoId([FromRoute] int id)
            {

            var tema = await _context.TopicsForums.FindAsync(id);
            if (tema is null)
                {
                return NotFound($"El tema del foro con id: ${id} no existe.");
                }

            _context.Remove(tema);
            await _context.SaveChangesAsync();

            await _operationsService.AddOperacion("Delete", "DeleteTema");
            return Ok();

            }

        /**
         * 
         * Método DeleteTemaForoSqlId(int id)
         * 
         * Elimina un perfil de usuario correspondiente a una id de PERFIL_USUARIO por SQL.
         * 
         * */

        [HttpDelete("deleteSQL/{id}")]
        public async Task<ActionResult<TopicsForum>> DeleteTemaForoSqlId([FromRoute] int id)
            {

            var tema = await _context.TopicsForums
                        .FromSqlInterpolated($"SELECT * FROM TemasForo WHERE IdTema = {id}")
                        .FirstOrDefaultAsync();
            if (tema is null) return NotFound("No existe ese tema.");


            var tieneMensajes = await _context.MessagesForums
                        .FromSqlInterpolated($"SELECT * FROM MensajeForo WHERE IdTema = {id}")
                        .AnyAsync();
            if (tieneMensajes) return BadRequest("Hay mensajes en ese tema del foro.");


            await _context.Database.ExecuteSqlInterpolatedAsync($@"DELETE FROM TemasForo WHERE IdTema = {id}");

            await _operationsService.AddOperacion("Delete", "DeleteTemaSQL");
            return Ok();
            }





        /**
         * 
         * Método GetTemasMensajesSelect([FromRoute] int id)
         * 
         * Devuelve un tema del foro con sus mensajes seleccionando información.
         * 
         * EN DESARROLLO
         * 
         * 
         * */

        //Método en desarrollo

        //[HttpGet("temasymensajes/{id:int}")]
        //public async Task<ActionResult<TemasForo>> GetTemasMensajesSelect(int id)
        //    {

        //    var casa = await (from x in _context.TemasForos
        //                      select new DTOCasasMonstruo
        //                          {
        //                          IdCasa = x.IdCasa,
        //                          NombreCasaMonstruo = x.NombreCasa,
        //                          ListaMonstruos = x.Monstruos.Select(y => new DTOMonstruoItem
        //                              {
        //                              IdMonstruoItem = y.IdMonstruo,
        //                              NombreMonstruo = y.NombreMonstruo,
        //                              Comportamiento = y.ComportamientoMonstruo,
        //                              }).ToList(),
        //                          }).FirstOrDefaultAsync(x => x.IdCasa == id);

        //    if (casa == null)
        //        {
        //        return NotFound("No hay casa de mosntruos con esa ID.");
        //        }
        //    return Ok(casa);
        //    }





        }
    }
