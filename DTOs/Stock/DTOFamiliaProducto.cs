namespace OLEToolBoxWebAPIPruebas.DTOs
    {
    public class DTOFamiliaProducto
        {
        public int IdFamily { get; set; }
        public string Name { get; set; }
        public int TotalProductos { get; set; }
        public List<DTOProductoItem> Productos { get; set; }
        }

    public class DTOProductoItem
        {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        }
    }
