using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica03.Domain
{
    public class FormaPago
    {
        private string nombre;

        public FormaPago()
        {
            nombre = "";
        }

        public FormaPago(string nombre)
        {
            this.nombre = nombre;
        }

        public string Nombre
        {
            get { return this.nombre; }
            set { this.nombre = value; }
        }

        public override string ToString()
        {
            return nombre;
        }
    }
}
