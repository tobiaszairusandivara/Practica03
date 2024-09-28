namespace Practica03.Models
{
    public class ArticuloModel
    {
        public string Nombre { get; set; }
        public int PreUnitario { get; set; }

        public ArticuloModel(string nombre, int preUnitario)
        {
            Nombre = nombre;
            PreUnitario = preUnitario;
        }

        public ArticuloModel()
        {

        }
    }
}
