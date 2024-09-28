namespace Practica03WebApi.Models
{
    public class FacturaModel
    {
        public int NroFactura { get; set; }
        public DateTime Fecha { get; set; }
        public int FormaPago { get; set; }
        public string Cliente { get; set; }
        public List<DetalleFacturaModel> DetalleFactura { get; set; }

        public FacturaModel(int nroFactura, DateTime fecha, int formaPago, string cliente, List<DetalleFacturaModel> detalleFactura)
        {
            NroFactura = nroFactura;
            Fecha = fecha;
            FormaPago = formaPago;
            Cliente = cliente;
            DetalleFactura = detalleFactura;
        }

        public FacturaModel()
        {
            DetalleFactura = new List<DetalleFacturaModel>(); // Inicializar la lista
        }   
    }
}
