using OLEToolBoxWebAPIPruebas.Validators;

namespace OLEToolBoxWebAPIPruebas.DTOs
    {
    public class DTOProductoPost
        {

        public int? IdProduct { get; set; }
        public string Nombre { get; set; }
        [PrecioValidacion]
        public decimal Precio { get; set; }

        public bool? Deprecated { get; set; }

        public int? Existances { get; set; }
        public int FamiliaId { get; set; }

        }
    }
