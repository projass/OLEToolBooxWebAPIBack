using OLEToolBoxWebAPIPruebas.Models;

namespace OLEToolBoxWebAPIPruebas.DTOs
    {
    public class DTOProductosAgrupadosDescatalogado
        {
        public bool Descatalogado { get; set; }
        public int Total { get; set; }
        public List<Product> Productos { get; set; }
        }
    }
