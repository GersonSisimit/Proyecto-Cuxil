namespace AgroservicioCuxil.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int IdMarca { get; set; }
        public int IdDetalleProducto { get; set; }
        public string RutaImagen { get; set; }
    }
}
