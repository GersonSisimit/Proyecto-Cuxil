namespace AgroservicioCuxil.Models
{
    public class DetallePresentacionProducto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public TipoPresentacion Tipo { get; set; }
        public enum TipoPresentacion
        {
            Liquido,
            Polvo,
            Otros
        }
    }
}
