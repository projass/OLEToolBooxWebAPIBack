using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OLEToolBoxWebAPIPruebas.DTOs;
using OLEToolBoxWebAPIPruebas.Models;
using OLEToolBoxWebAPIPruebas.Services;

namespace OLEToolBoxWebAPIPruebas.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
        {

        //private readonly FileStorageManager _gestorArchivosLocal;
        private readonly PruebasoletoolboxContext _context;
        private readonly OperationsService _operationsService;

        public ProductsController(PruebasoletoolboxContext context, OperationsService operationsService)
            {
            _context = context;
            _operationsService = operationsService;
            }

        [HttpGet("productos")]
        public async Task<ActionResult<List<Product>>> GetProductos()
            {

            var lista = await _context.Families.ToListAsync();

            if (lista.Count() == 0) return NotFound();

            await _operationsService.AddOperacion("Get", "Families");
            return Ok(lista);

            }

        [HttpGet("productosporfamilianodto/{id:int}")]
        public async Task<ActionResult<List<Product>>> GetProductosPorFamiliaSinDTO(int id)
            {
            // Obtiene un mensaje específico por su ID
            var productosFamilia = await _context.Products.Where(x => x.FamilyId == id).ToListAsync();

            if (productosFamilia == null)
                {
                return NotFound();
                }

            // await _operationsService.AddOperacion("Get", "GetMensajesForoPortema");
            return Ok(productosFamilia);
            }

        [HttpGet("productosagrupadospordescatalogado")]
        public async Task<ActionResult<List<DTOProductosAgrupadosDescatalogado>>> GetProductosAgrupadosPorDescatalogado()
            {
            // Esta consulta devuelve todos los productos agrupados por descatalogado
            // Me salen 2 arrays. Uno con los descatalogados false y el otro con los descatalogados true
            //  var productos = await _context.Productos.GroupBy(g => g.Descatalogado).ToListAsync();
            // Suponemos que nos piden un agrupamiento a medida. Por ejemplo: 
            // * Los valores de los grupos (en este caso serán true/false)
            // * Cuántos productos hay de cada grupo
            // * La lista de productos por grupo

            // Podemos devolverlo así
            //var productos = await _context.Productos.GroupBy(g => g.Descatalogado)
            //    .Select(x => new
            //    {
            //        Descatalogado = x.Key,
            //        Total = x.Count(),
            //        Productos = x.ToList()
            //    }).ToListAsync();

            // O mejor así, utilizando una clase a medida DTO
            var productos = await _context.Products.GroupBy(g => g.Deprecated)
               .Select(x => new DTOProductosAgrupadosDescatalogado
                   {
                   Descatalogado = x.Key,
                   Total = x.Count(),
                   Productos = x.ToList()
                   }).ToListAsync();

            await _operationsService.AddOperacion("Get", "ProductosDescatalogado");

            return Ok(productos);
            }

        [HttpGet("filtrar")]
        public async Task<ActionResult> GetFiltroMultiple([FromQuery] DTOProductosFiltro filtroProductos)
            {
            // AsQueryable nos permite ir construyendo paso a paso el filtrado y ejecutarlo al final.
            // Si lo convertimos a una lista (toListAsync) el resto de filtros los hacemos en memoria
            // porque toListAsync ya trae a la memoria del servidor los datos desde el servidor de base de datos
            // Hacer los filtros en memoria es menos eficiente que hacerlos en una base de datos.
            // Construimos los filtros de forma dinámica y hasta que no hacemos el ToListAsync no vamos a la base de datos
            // para traer la información

            // Versión poco optimizada (no aconsejada)
            //var productos = await _context.Productos.ToListAsync();
            //if (!string.IsNullOrEmpty(filtroProductos.ContieneEnNombre))
            //{
            //    productos = productos.Where(x => x.Nombre.Contains(filtroProductos.ContieneEnNombre)).ToList();
            //}

            //if (filtroProductos.Descatalogado)
            //{
            //    productos = productos.Where(x => x.Descatalogado).ToList();
            //}

            //if (filtroProductos.FamiliaId != 0)
            //{
            //    productos = productos.Where(x => x.FamiliaId == filtroProductos.FamiliaId).ToList();
            //}

            //return Ok(productos);

            // Versión aconsejada porque hace una única consulta al final
            var productosQueryable = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(filtroProductos.ContieneEnNombre))
                {
                productosQueryable = productosQueryable.Where(x => x.Name.Contains(filtroProductos.ContieneEnNombre));
                }

            if (filtroProductos.Descatalogado)
                {
                productosQueryable = productosQueryable.Where(x => x.Deprecated);
                }

            if (filtroProductos.FamiliaId != 0)
                {
                productosQueryable = productosQueryable.Where(x => x.FamilyId == filtroProductos.FamiliaId);
                }

            var productos = await productosQueryable.ToListAsync();

            await _operationsService.AddOperacion("Get", "Productos Filtrar");

            return Ok(productos);
            }

        [HttpPost]
        public async Task<ActionResult> PostProductos([FromBody] DTOProductoPost producto)
            {
            Product newProducto = new Product
                {
                Name = producto.Nombre,
                Price = producto.Precio,
                Deprecated = false,
                DateUp = new DateOnly(),
                FamilyId = producto.FamiliaId,
                Existances = producto.Existances,
                PictureUrl = ""
                };

            await _context.AddAsync(newProducto);
            await _context.SaveChangesAsync();

            await _operationsService.AddOperacion("Post", "Productos");

            return Ok(newProducto);
            }

        [HttpPost("conimagen")]
        public async Task<ActionResult> PostProductos([FromForm] DTOProductoAgregar producto)
            {
            Product newProducto = new Product
                {
                Name = producto.Nombre,
                Price = producto.Precio,
                Deprecated = false,
                DateUp = new DateOnly(),
                Existances = producto.Existances,

                FamilyId = producto.FamiliaId,
                PictureUrl = ""
                };

            if (producto.Foto != null)
                {
                using (var memoryStream = new MemoryStream())
                    {
                    // Extraemos la imagen de la petición
                    await producto.Foto.CopyToAsync(memoryStream);
                    // La convertimos a un array de bytes que es lo que necesita el método de guardar
                    var contenido = memoryStream.ToArray();
                    // La extensión la necesitamos para guardar el archivo
                    var extension = Path.GetExtension(producto.Foto.FileName);
                    // Recibimos el nombre del archivo
                    // El servicio Transient GestorArchivosLocal instancia el servicio y cuando se deja de usar se destruye
                    //newProducto.PictureUrl = await _gestorArchivosLocal.SaveFile(contenido, extension, "imagenes",
                    //  producto.Foto.ContentType);
                    newProducto.PictureUrl = null;
                    }
                }

            await _context.AddAsync(newProducto);
            await _context.SaveChangesAsync();

            await _operationsService.AddOperacion("Post", "Productos");

            return Ok(newProducto);
            }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMensajeForo(int id, DTOProductoAgregar producto)
            {
            if (id != producto.IdProduct)
                {
                return BadRequest();
                }

            var productoUpdate = await _context.Products.FindAsync(id);

            if (productoUpdate == null)
                return NotFound();

            productoUpdate.Name = producto.Nombre;
            productoUpdate.Price = producto.Precio;
            productoUpdate.FamilyId = producto.FamiliaId;

            _context.Update(productoUpdate);

            await _context.SaveChangesAsync();

            await _operationsService.AddOperacion("Put", "ModificarMensaje");
            return Accepted("Datos actualizados");
            }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductos([FromRoute] int id)
            {
            var producto = await _context.Products.FindAsync(id);
            if (producto == null)
                {
                return NotFound();
                }

            //await _gestorArchivosLocal.DeleteFile(producto.PictureUrl, "imagenes");
            _context.Remove(producto);
            await _context.SaveChangesAsync();

            await _operationsService.AddOperacion("Delete", "Productos");

            return Ok();
            }

        }
    }
