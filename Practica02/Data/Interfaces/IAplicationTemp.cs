using Practica01.Domain;
using Practica03.Domain;

namespace Practica02Back.Data.Interfaces
{
    public interface IAplicationTemp
    {
        bool Create(Factura oFactura);
        List<Factura> GetAll();
        Factura? GetByParam(DateTime? fec, int? id_forma_pag);
        bool Update(int id, Factura updFactura);
    }
}
