using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica03.Domain
{
    public class DetalleFactura
    {
        private int nroFactura;
        private int id_detalle;
        private int id_articulo;
        private int cantidad;

        public DetalleFactura()
        {
            nroFactura = 0;
            id_articulo = 0;
            cantidad = 0;
        }

        public DetalleFactura(int nroFactura,int id_articulo, int cantidad)
        {
            this.id_articulo = id_articulo;
            this.cantidad = cantidad;
        }

        public int NroFactura
        {
            get { return this.nroFactura; }
            set { this.nroFactura = value; }
        }

        public int Id_Detalle
        {
            get { return this.id_detalle; }
            set { id_detalle = value; }
        }

        public int Id_Articulo
        {
            get { return this.id_articulo; }
            set { this.id_articulo = value; }
        }

        public int Cantidad
        {
            get { return this.cantidad; }
            set { this.cantidad = value; }
        }

        public override string ToString()
        {
            return cantidad + "x" + id_articulo.ToString();
        }
    }
}
