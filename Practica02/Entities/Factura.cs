using Practica03.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Practica01.Domain
{
    public class Factura
    {
        private int nroFactura;
        private DateTime fecha;
        private int formaPago;
        private string cliente;
        private DetalleFactura detalleFactura;
        public List<DetalleFactura> DetalleFactura { get; set; }


        public Factura()
        {
            nroFactura = 0;
            fecha = DateTime.Now;
            formaPago = 0;
            cliente = string.Empty;
            DetalleFactura = new List<DetalleFactura>();
        }

        public Factura(int nroFactura, DateTime fecha, int formaPago, string cliente)
        {
            this.nroFactura = nroFactura;
            this.fecha = fecha;
            this.formaPago = formaPago;
            this.cliente = cliente;
            DetalleFactura = new List<DetalleFactura>();
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

        public void AddDetail(DetalleFactura detalle)
        {
            if (detalle != null)
            DetalleFactura.Add(detalle);
        }

        public void RemoveDetail(int index)
        {
            DetalleFactura.RemoveAt(index);
        }
    }
}
