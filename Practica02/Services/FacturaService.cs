using Practica01.Domain;
using Practica02Back.Data.Implementations;
using Practica02Back.Data.Interfaces;
using Practica02.Domain;
using Practica02back.Data.Interfaces;

namespace Practica02Back.Services
{
    public class FacturaService : IFacturaService
    {
        private IAplication repository;
        private IAplicationTemp repositoryTemp;

        public FacturaService()
        {
            repository = new ArticulosRepository();
            repositoryTemp = new FacturaRepository();
        }

        //Articulos
        public bool CreateArt(Articulo oArticulo)
        {
            return repository.Create(oArticulo);
        }

        public List<Articulo> GetAllArt()
        {
            return repository.GetAll();
        }

        public bool DeleteArt(int id)
        {
            return repository.Delete(id);
        }

        public bool UpdateArt(int id, Articulo updArticulo)
        {
            return repository.Update(id, updArticulo);
        }


        //Facturas
        public bool CreateFact(Factura oFactura)
        {
            return repositoryTemp.Create(oFactura);
        }

        public List<Factura> GetAllFact()
        {
            return repositoryTemp.GetAll();
        }

        public Factura? GetFactParam(DateTime fec, int id_forma_pag)
        {
            return repositoryTemp.GetByParam(fec, id_forma_pag);
        }

        public bool UpdateFact(int id, Factura updFactura)
        {
            return repositoryTemp.Update(id, updFactura);
        }
    }
}
