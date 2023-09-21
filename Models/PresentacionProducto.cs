namespace AgroservicioCuxil.Models
{
    public class PresentacionProducto
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public string RutaImagen { get; set; }
        public int IdDetallePresentacion { get; set; }
        public double Existencia { get; set; }
        public double Precio { get; set; }
    }
}
