using Practica01.Domain;
using Practica02.Domain;

namespace Practica02Back.Services
{
    public interface IFacturaService
    {
        bool CreateArt(Articulo oArticulo);
        List<Articulo> GetAllArt();
        bool DeleteArt(int id);
        bool UpdateArt(int id, Articulo updArticulo);


        bool CreateFact(Factura oFactura);
        List<Factura> GetAllFact();
        Factura? GetFactParam(DateTime fec, int id_forma_pag);
        bool UpdateFact(int id, Factura updFactura);
    }
}
