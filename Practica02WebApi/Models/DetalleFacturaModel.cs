namespace Practica03WebApi.Models
{
    public class DetalleFacturaModel
    {
        public int Id_Articulo { get; set; } // ID del artículo
        public string NombreArticulo { get; set; } // Nombre del artículo
        public int Cantidad { get; set; } // Cantidad de artículos
        public float Precio { get; set; } // Precio por artículo

        public DetalleFacturaModel(int idArticulo, string nombreArticulo, int cantidad, float precio)
        {
                Id_Articulo = idArticulo;
                NombreArticulo = nombreArticulo;
                Cantidad = cantidad;
                Precio = precio;
        }

        public DetalleFacturaModel()
        {

        }
    }
}
