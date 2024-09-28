using Practica01.Domain;
using Practica03.Data.DataAccess;
using Practica02Back.Data.Interfaces;
using Practica03.Domain;
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
                if (oFactura == null || oFactura.DetalleFactura == null || oFactura.DetalleFactura.Count == 0)
                {
                    return false;
                }
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                transaction = _connection.BeginTransaction();
                // Primero, insertar la factura
                var parametrosFactura = new List<SQLParameter>
                {
                    new SQLParameter("@fecha", oFactura.Fecha),
                    new SQLParameter("@id_forma_pago", oFactura.FormaPago),
                    new SQLParameter("@cliente", oFactura.Cliente)
                };
                // Ejecutar el SP para crear la factura
                bool resultFactura = dh.ExecuteCRUDSPQueryWithTransaction("SP_CREATE_FACTURA", parametrosFactura, transaction);
                if (!resultFactura)
                {
                    transaction.Rollback();
                    return false;
                }
                // Ahora, obtener el nro_factura 
                int nroFactura = GetLastInsertedFacturaId(transaction); // ID de la última factura insertada
                // Insertar los detalles de la factura
                foreach (var detalle in oFactura.DetalleFactura)
                {
                    var parametrosDetalle = new List<SQLParameter>
                    {
                        new SQLParameter("@nro_factura", nroFactura),
                        new SQLParameter("@id_articulo", detalle.Id_Articulo), 
                        new SQLParameter("@cantidad", detalle.Cantidad)
                    };
                    // Ejecutar el SP para crear el detalle de la factura
                    bool resultDetalle = dh.ExecuteCRUDSPQueryWithTransaction("SP_CREATE_DETALLE", parametrosDetalle, transaction);
                    if (!resultDetalle)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
                transaction.Commit();
                return true;
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
            Factura? oFactura = null;
            var helper = DataHelper.GetInstance();
            var t = helper.ExecuteSPQuery("SP_GET_ALL_FACTURA", null);

            foreach (DataRow row in t.Rows)
            {
                // Leer la primera fila y tomar datos del maestro y primer detalle
                if (oFactura == null || oFactura.NroFactura != Convert.ToInt32(row["nro_factura"]))
                {
                    oFactura = new Factura
                    {
                        NroFactura = Convert.ToInt32(row["nro_factura"]),
                        Fecha = Convert.ToDateTime(row["fecha"]),
                        FormaPago = Convert.ToInt32(row["id_forma_pago"]),
                        Cliente = row["cliente"].ToString(),
                        DetalleFactura = new List<DetalleFactura>() // Inicializa la lista de detalles
                    };
                    // Agregar el primer detalle
                    oFactura.DetalleFactura.Add(ReadDetalleFactura(row));
                    lstFac.Add(oFactura);
                }
                else
                {
                    // Mientras no cambia el NroFactura, leer datos de detalles.
                    oFactura.DetalleFactura.Add(ReadDetalleFactura(row));
                }
            }
            return lstFac;
        }



        //SI O SI TIENE QUE TENER LOS DOS PARAMETROS, DE LO CONTRARIO RETORNA NULL
        public Factura? GetByParam(DateTime? fec, int? id_forma_pago)
        {
            Factura? oFactura = null;
            var helper = DataHelper.GetInstance();
            var parametros = new List<SQLParameter>();
            // Solo agregar los parámetros si no son nulos
            if (fec.HasValue)
                parametros.Add(new SQLParameter("fecha", fec.Value));

            if (id_forma_pago.HasValue)
                parametros.Add(new SQLParameter("id_forma_pago", id_forma_pago.Value));
            try
            {
                var t = helper.ExecuteSPQuery("SP_GET_PARAM_FACTURA", parametros);
                if (t == null || t.Rows.Count == 0)
                {
                    Console.WriteLine("No se encontró ninguna factura con los parámetros proporcionados.");
                    return null;
                }
                foreach (DataRow row in t.Rows)
                {
                    if (row.Table.Columns.Contains("nro_factura") &&
                        row.Table.Columns.Contains("fecha") &&
                        row.Table.Columns.Contains("id_forma_pago") &&
                        row.Table.Columns.Contains("cliente"))
                    {
                        if (oFactura == null)
                        {
                            oFactura = new Factura
                            {
                                NroFactura = Convert.ToInt32(row["nro_factura"]),
                                Fecha = Convert.ToDateTime(row["fecha"]),
                                FormaPago = Convert.ToInt32(row["id_forma_pago"]),
                                Cliente = row["cliente"].ToString(),
                                DetalleFactura = new List<DetalleFactura>() 
                            };
                            oFactura.DetalleFactura.Add(ReadDetalleFactura(row));
                        }
                        else
                        {
                            oFactura.DetalleFactura.Add(ReadDetalleFactura(row));
                        }
                    }
                    else
                    {
                        Console.WriteLine("Faltan columnas en el resultado de la consulta.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return oFactura;
        }




        //RE HACER PARA QUE SE MOD. DETALLES O SE AGREGUEN SOBRE UNA FACTURA
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
                bool facturaActualizada = dh.ExecuteCRUDSPQuery("SP_UPDATE_FACT", parametros);

                // Ahora, actualizar los detalles
                foreach (var detalle in updFactura.DetalleFactura)
                {
                    if (detalle.Id_Detalle > 0)
                    {
                        var detalleParametros = new List<SQLParameter>
                        {
                           new SQLParameter("id_detalle", detalle.Id_Detalle),
                           new SQLParameter("nro_factura", id), 
                           new SQLParameter("id_articulo", detalle.Id_Articulo),
                           new SQLParameter("cantidad", detalle.Cantidad)
                        };
                        dh.ExecuteCRUDSPQuery("SP_UPDATE_DET", detalleParametros);
                    }
                    else
                    {
                        var nuevoDetalleParametros = new List<SQLParameter>
                        {
                           new SQLParameter("nro_factura", id), 
                           new SQLParameter("id_articulo", detalle.Id_Articulo),
                           new SQLParameter("cantidad", detalle.Cantidad)
                        };
                        dh.ExecuteCRUDSPQuery("SP_CREATE_DETALLE", nuevoDetalleParametros);
                    }
                }
                return facturaActualizada;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Exception: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }


        //METODOS AUXILIARES
        // Función auxiliar para leer un detalle de factura
        private DetalleFactura ReadDetalleFactura(DataRow row)
        {
            DetalleFactura detalle = new DetalleFactura
            {
                Id_Articulo = Convert.ToInt32(row["id_articulo"].ToString()),
                Cantidad = Convert.ToInt32(row["cantidad"].ToString()),
            };
            return detalle;
        }



        private int GetLastInsertedFacturaId(SqlTransaction transaction)
        {
            // ID de la última factura insertada. SCOPE_IDENTITY() o
            var command = new SqlCommand("SELECT SCOPE_IDENTITY()", transaction.Connection, transaction);
            return Convert.ToInt32(command.ExecuteScalar());
        }
    }
}
