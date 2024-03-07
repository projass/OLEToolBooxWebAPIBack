using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OLEToolBoxWebAPIPruebas.DTOs;
using OLEToolBoxWebAPIPruebas.Models;
using OLEToolBoxWebAPIPruebas.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OLEToolBoxWebAPIPruebas.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class FamiliesController : ControllerBase
        {

        private readonly PruebasoletoolboxContext _context;
        private readonly OperationsService _operationsService;

        public FamiliesController(PruebasoletoolboxContext context, OperationsService operationsService)
            {
            _context = context;
            _operationsService = operationsService;
            }

        [HttpGet("familias")]
        public async Task<ActionResult<List<Family>>> GetFamilies()
            {

            var lista = await _context.Families.Include(p => p.Products).ToListAsync();

            if (lista.Count() == 0) return NotFound();

            await _operationsService.AddOperacion("Get", "Families");
            return Ok(lista);

            }

        [HttpGet("ordenadas")]
        public async Task<List<Family>> GetFamiliasOrdenadas()
            {
            // var context = new MiAlmacenContext();
            var listaOrdenada = await _context.Families.OrderBy(x => x.Name).ToListAsync();
            return listaOrdenada;
            }


        [HttpGet("{id:int}")] // api/familias/1 -->Si llamamos a api/familias/moda da 404 por la restricción
        public async Task<ActionResult<Family>> GetFamiliaPorId([FromRoute] int id) // este nombre id es muy importante que coincida con {id} [FromRoute] es opcional y es una manera de decir que el parámetro viene de la ruta
            {
            var familia = await _context.Families.FindAsync(id); // FindAsync busca por campo clave. En este caso, la familia con el id que se le pasa por argumento
                                                                 // var familiaporid = await _context.Familias.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (familia == null)
                {
                return NotFound("La familia " + id + " no existe");
                }
            return Ok(familia);
            }

        [HttpGet("{contiene}/{param2=hogar}")] // api/familias/a/b  --> param2 tiene el valor por defecto hogar
        public async Task<ActionResult<Family>> PrimeraFamiliaPorContiene([FromRoute] string contiene, [FromRoute] string? param2)
        // public async Task<ActionResult<Familia>> GetPrimeraFamiliaPorContiene(string contiene)
            {
            var familia = await _context.Families.FirstOrDefaultAsync(x => x.Name.Contains(contiene));
            if (familia == null)
                {
                return NotFound();
                }
            return Ok(familia);
            }

        [HttpGet("paginacion")]
        public async Task<ActionResult<List<Family>>> GetFamiliasPaginacion()
            {
            var familias = await _context.Families.Take(2).ToListAsync();
            var familias2 = await _context.Families.Skip(1).Take(2).ToListAsync();
            return Ok(new { take = familias, takeSkip = familias2 });
            }

        [HttpPost]
        public async Task<ActionResult> PostFamilia([FromBody] DTOFamilia familia)
            {
            var newFamilia = new Family()
                {
                Name = familia.Name
                };

            await _context.AddAsync(newFamilia);
            await _context.SaveChangesAsync();

            return Created("Familia", new { familia = newFamilia });
            }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutFamilia([FromRoute] int id, DTOFamilia familia)
            {
            if (id != familia.IdFamily)
                {
                return BadRequest("Los ids proporcionados son diferentes");
                }
            var familiaUpdate = await _context.Families.AsTracking().FirstOrDefaultAsync(x => x.IdFamily == id);
            if (familiaUpdate == null)
                {
                return NotFound();
                }
            familiaUpdate.Name = familia.Name;
            _context.Update(familiaUpdate);

            await _context.SaveChangesAsync();
            return NoContent();
            }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
            {
            //var productos = await _context.Productos.CountAsync(x => x.FamiliaId == id);
            //if (productos != 0)
            //{
            //    return BadRequest("Hay productos relacionados");
            //}

            //var listaProductos = await _context.Productos.Where(x => x.FamiliaId == id).ToListAsync();
            //if (listaProductos.Count() != 0)
            //{
            //    return BadRequest("Hay productos relacionados");
            //}

            var hayProductos = await _context.Products.AnyAsync(x => x.FamilyId == id);
            if (hayProductos)
                {
                return BadRequest("Hay productos relacionados");
                }

            var familia = await _context.Families.FindAsync(id);

            if (familia is null)
                {
                return NotFound("La familia no existe");
                }

            _context.Remove(familia);
            await _context.SaveChangesAsync();
            return Ok();
            }

        [HttpDelete("familiayproductos/{id:int}")]
        public async Task<ActionResult> DeleteFamiliaYProductos(int id)
            {
            var familiaConProductos = await _context.Families.Include(x => x.Products).FirstOrDefaultAsync(x => x.IdFamily == id);

            if (familiaConProductos is null)
                {
                return NotFound("La familia no existe");
                }

            _context.Products.RemoveRange(familiaConProductos.Products);
            _context.Families.Remove(familiaConProductos);
            await _context.SaveChangesAsync();
            return Ok();
            }


        }
    }