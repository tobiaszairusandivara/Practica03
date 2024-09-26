using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica02.Domain
{
    public class Articulo
    {
        private string nombre;
        private int precioUnitario;

        public Articulo()
        {
            nombre = "";
            precioUnitario = 0;
        }

        public Articulo(string nombre, int precioUnitario)
        {
            this.nombre = nombre;
            this.precioUnitario = precioUnitario;
        }

        public string Nombre
        {
            get { return this.nombre; }
            set { this.nombre = value; }
        }
            
        public int PrecioUnitario
        {
            get { return this.precioUnitario; }
            set { this.precioUnitario = value; }
        }

        public override string ToString()
        {
            return nombre + "; $" + precioUnitario;
        }
    }
}
