using OLEToolBoxWebAPIPruebas.Validators;

namespace OLEToolBoxWebAPIPruebas.DTOs
    {
    public class DTOProductoAgregar
        {

        public int? IdProduct { get; set; }
        public string Nombre { get; set; }
        [PrecioValidacion]
        public decimal Precio { get; set; }

        public int? Existances { get; set; }
        public bool? Deprecated { get; set; }

        // Validadores
        // Los validadores nos van a permitir validar la información que nos llega
        [PesoArchivoValidacion(PesoMaximoEnMegaBytes: 5)]
        [TipoArchivoValidacion(grupoTipoArchivo: GrupoTipoArchivo.Imagen)]
        public IFormFile? Foto { get; set; }
        public int FamiliaId { get; set; }

        }
    }
