using Practica01.Domain;
using Practica02.Data.DataAccess;
using Practica02Back.Data.Interfaces;
using Practica02.Domain;
using System.Data;
using System.Data.SqlClient;

namespace Practica02Back.Data.Implementations
{
    public class FacturaRepository : IAplicationTemp
    {
        private DataHelper dh;
        private SqlConnection _connection;

        public FacturaRepository()
        {
            dh =DataHelper.GetInstance();
            _connection = dh.GetConection();
        }

        public bool Create(Factura oFactura)
        {
            SqlTransaction transaction = null;

            try
            {
                if (oFactura == null)
                {
                    return false;
                }
                transaction = _connection.BeginTransaction();
                var parametros = new List<SQLParameter>
                {
                    new SQLParameter("@fecha", oFactura.Fecha),
                    new SQLParameter("@id_forma_pago", oFactura.FormaPago),
                    new SQLParameter("@cliente", oFactura.Cliente)
                };
                bool result = dh.ExecuteCRUDSPQuery("SP_CREAR_FACTURA", parametros);
                if (result)
                {
                    transaction.Commit();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }
            }
            catch (SqlException ex)
            {
                transaction?.Rollback();
                Console.WriteLine($"SQL Exception: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public List<Factura> GetAll()
        {
            List<Factura> lstFac = new List<Factura>();
            var helper = DataHelper.GetInstance();
            var spHelper = helper.ExecuteSPQuery("SP_GET_ALL_FACTURA", null);
            foreach (DataRow row in spHelper.Rows)
            {
                string cliente = Convert.ToString(row["cliente"]);
                int nroFactura = Convert.ToInt32(row["nro_factura"]);
                DateTime fecha = Convert.ToDateTime(row["fecha"]);
                int formaPago = Convert.ToInt32(row["id_forma_pago"]); ;

                Factura oFactura = new Factura()
                {
                    Cliente = cliente,
                    NroFactura = nroFactura,
                    Fecha = fecha,
                    FormaPago = formaPago
                };
                lstFac.Add(oFactura);
            }
            return lstFac;
        }

        public Factura? GetByParam(DateTime fec, int id_forma_pago)
        {
            var parametros = new List<SQLParameter>
            {
                    new SQLParameter("fecha", fec),
                    new SQLParameter("id_forma_pago", id_forma_pago)
            };

            DataTable spHelp = DataHelper.GetInstance().ExecuteSPQuery("SP_GET_PARAM_FACTURA", parametros);
            if (spHelp != null && spHelp.Rows.Count == 1)
            {
                DataRow row = spHelp.Rows[0];
                int nroFactura = Convert.ToInt32(row["nro_factura"]);
                DateTime fecha = Convert.ToDateTime(row["fecha"]);
                int formaPago = Convert.ToInt32(row["id_forma_pago"]);
                string cliente = Convert.ToString(row["cliente"]);

                Factura oFactura = new Factura()
                {
                    NroFactura = nroFactura,
                    Fecha = fecha,
                    FormaPago = formaPago,
                    Cliente = cliente
                };
                return oFactura;
            }
            return null;
        }

        public bool Update(int id, Factura updFactura)
        {
            try
            {
                if (updFactura == null)
                {
                    return false;
                }
                var parametros = new List<SQLParameter>
                {
                    new SQLParameter("nro_factura", id),
                    new SQLParameter("cliente", updFactura.Cliente),
                    new SQLParameter("fecha", updFactura.Fecha),
                    new SQLParameter("id_forma_pago", updFactura.FormaPago)
                };
                return dh.ExecuteCRUDSPQuery("SP_UPDTE_FACTURA", parametros);
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
    }
}
