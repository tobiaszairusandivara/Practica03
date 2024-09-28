using Microsoft.AspNetCore.Components.Forms;
using Practica01.Domain;
using Practica03.Domain;

namespace Practica02back.Data.Interfaces
{
    public interface IAplication
    {
        bool Create(Articulo oArticulo);//se hace todo o no se hace nada
        List<Articulo> GetAll();
        bool Delete(int id);
        bool Update(int id, Articulo updArticulo);
    }
}
