using Practica02.Domain;
using Practica02.Data.DataAccess;
using System.Data.SqlClient;
using System.Data;
using Practica02back.Data.Interfaces;

namespace Practica02Back.Data.Implementations
{
    public class ArticulosRepository : IAplication
    {
        private DataHelper dh;
        private SqlConnection _connection;

        public ArticulosRepository()
        {
            dh = DataHelper.GetInstance();
            _connection = dh.GetConection();
        }

        public bool Create(Articulo oArticulo)
        {
            try
            {
                if(oArticulo ==  null)
                {
                    return false;
                }

                var parametros = new List<SQLParameter>
                {
                    new SQLParameter("@nombre", oArticulo.Nombre),
                    new SQLParameter("@pre_unitario", oArticulo.PrecioUnitario)
                };
                return dh.ExecuteCRUDSPQuery("SP_CREATE_ARTICULO", parametros);
            }
            catch(SqlException ex)
            {
                Console.WriteLine($"SQL Exepction: {ex.Message}");
                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exepction: {ex.Message}");
                return false;
            }
        }

        public List<Articulo> GetAll()
        {
            List<Articulo> lstArt = new List<Articulo>();
            var helper = DataHelper.GetInstance();
            var spHelper = helper.ExecuteSPQuery("SP_GET_ALL_ARTICULO", null);
            foreach (DataRow row in spHelper.Rows) 
            {
                string nombre = Convert.ToString(row["nombre"]);
                int precioUnitario = Convert.ToInt32(row["pre_unitario"]);

                Articulo oArticulo = new Articulo()
                {
                    Nombre = nombre,
                    PrecioUnitario = precioUnitario
                };
                lstArt.Add(oArticulo);
            }
            return lstArt;  
        }

        public bool Update(int id, Articulo updArticulo)
        {
            try
            {
                if (updArticulo == null)
                {
                    return false;
                }

                var parametros = new List<SQLParameter>
                {
                    new SQLParameter("@id_articulo", id),
                    new SQLParameter("@nombre", updArticulo.Nombre),
                    new SQLParameter("@pre_unitario", updArticulo.PrecioUnitario)
                };
                return dh.ExecuteCRUDSPQuery("SP_UPDTE_ARTICULO", parametros);
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Exepction: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exepction: {ex.Message}");
                return false;
            }
        }

        public bool Delete(int id)
        {
            var parametros = new List<SQLParameter>();
            parametros.Add(new SQLParameter("@id_articulo", id));
            bool del = DataHelper.GetInstance().ExecuteCRUDSPQuery("SP_DEL_ARTICULO", parametros);
            return del;
        }
    }
}
