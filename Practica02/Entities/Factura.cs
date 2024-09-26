using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica01.Domain
{
    public class Factura
    {
        private int nroFactura;
        private DateTime fecha;
        //private FormaPago formaPago;
        private int formaPago;
        private string cliente; 

        public Factura()
        {
            nroFactura = 0;
            fecha = DateTime.Now;
            formaPago = 0;
            cliente = string.Empty;
        }

        public Factura(int nroFactura, DateTime fecha, int formaPago, string cliente)
        {
            this.nroFactura = nroFactura;
            this.fecha = fecha;
            this.formaPago = formaPago;
            this.cliente = cliente;
        }
         
        public int NroFactura
        { 
            get { return this.nroFactura; }
            set { this.nroFactura = value; }
        }

        public DateTime Fecha
        {
            get { return this.fecha; }
            set { this.fecha = value; }
        }

        public int FormaPago
        {
            get { return this.formaPago; }
            set { this.formaPago = value; }
        }
     
        public string Cliente
        {
            get { return this.cliente; }
            set { this.cliente = value; }
        }
    }
}
