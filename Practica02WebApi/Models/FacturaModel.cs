namespace Practica02WebApi.Models
{
    public class FacturaModel
    {
        public int NroFactura { get; set; }
        public DateTime Fecha { get; set; }
        public int FormaPago { get; set; }
        public string Cliente { get; set; }

        public FacturaModel(int nroFactura, DateTime fecha, int formaPago, string cliente)
        {
            NroFactura = nroFactura;
            Fecha = fecha;
            FormaPago = formaPago;
            Cliente = cliente;
        }

        public FacturaModel()
        {

        }
    }
}
