using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Supermercado.Data;
using System.Data;
using System.Linq;

namespace Supermercado.DAO
{
    public class CRUD : ConexionBD
    {
        public void insertarProducto(Producto p)
        {
            try {
                OracleConnection connectionString = GetConnection();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connectionString;
                cmd.CommandText = "insertar_producto_sp";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("id", OracleDbType.Int32).Value = p.id;
                cmd.Parameters.Add("idArea", OracleDbType.Int32).Value = p.idArea;
                cmd.Parameters.Add("ean", OracleDbType.Long).Value = p.ean;
                cmd.Parameters.Add("descripcion", OracleDbType.Varchar2).Value = p.descripcion;
                cmd.Parameters.Add("precio", OracleDbType.BinaryFloat).Value = p.precio;
                cmd.Parameters.Add("cantidad", OracleDbType.Int32).Value = p.cantidad;
                connectionString.Open();
                cmd.ExecuteNonQuery();
                connectionString.Close();
                cmd.Dispose();
            } catch (Exception) {

            }
        }
        public List<Producto> listarProductos()
        {
            try
            {
                List<Producto> productos = new List<Producto>();
                OracleConnection connectionString = GetConnection();
                string sql = "select producto.pk_idproducto, producto.fk_idarea, producto.ean, producto.descripcion, producto.precio, producto.cantidad  from SUPER.producto";
                OracleConnection connection = connectionString;
                connection.Open();
                OracleCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                OracleDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Producto p = new Producto();
                        p.id = dr.GetInt32(0);
                        p.idArea = dr.GetInt32(1);
                        p.ean = dr.GetInt64(2);
                        p.descripcion = dr.GetString(3);
                        p.precio = dr.GetFloat(4);
                        p.cantidad = dr.GetInt32(5);
                        productos.Add(p);
                    }
                }
                connection.Close();
                cmd.Dispose();
                connection.Dispose();
                return productos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Producto productoPorId(int id)
        {
            try
            {
                Producto p = new Producto();
                OracleConnection connectionString = GetConnection();
                string sql = "select producto.fk_idarea, producto.ean, producto.descripcion, producto.precio, producto.cantidad  from SUPER.producto where producto.pk_idproducto=:id";
                OracleConnection connection = connectionString;
                connection.Open();
                OracleCommand cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("id", OracleDbType.Int32).Value = id;
                cmd.ExecuteNonQuery();
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        p.idArea = dr.GetInt32(0);
                        p.ean = dr.GetInt64(1);
                        p.descripcion = dr.GetString(2);
                        p.precio = dr.GetFloat(3);
                        p.cantidad = dr.GetInt32(4);
                    }
                }
                connection.Close();
                cmd.Dispose();
                connection.Dispose();
                return p;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void actualizarProducto(Producto p)
        {
            try
            {
                OracleConnection connectionString = GetConnection();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connectionString;
                cmd.CommandText = "actualizar_producto_sp";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("id", OracleDbType.Int32).Value = p.id;
                cmd.Parameters.Add("idArea", OracleDbType.Int32).Value = p.idArea;
                cmd.Parameters.Add("ean", OracleDbType.Long).Value = p.ean;
                cmd.Parameters.Add("descripcion", OracleDbType.Varchar2).Value = p.descripcion;
                cmd.Parameters.Add("precio", OracleDbType.BinaryFloat).Value = p.precio;
                cmd.Parameters.Add("cantidad", OracleDbType.Int32).Value = p.cantidad;
                connectionString.Open();
                cmd.ExecuteNonQuery();
                connectionString.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        public void eliminarProducto(int id)
        {
            try
            {
                OracleConnection connectionString = GetConnection();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connectionString;
                cmd.CommandText = "eliminar_producto_sp";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("id", OracleDbType.Int32).Value = id;
                connectionString.Open();
                cmd.ExecuteNonQuery();
                connectionString.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        public List<LogMovimiento> logsMovimientos()
        {
            DataSet ds = new DataSet();
            List<LogMovimiento> logs = new List<LogMovimiento>();
            try
            {
                OracleConnection connection = GetConnection();
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "seleccionar_bitacoraMovimientos";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(ds);
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                    logs.Add(new LogMovimiento()
                    {
                        id = Int32.Parse(row["PK_IDBITACORAMOVIMIENTOS"].ToString()),
                        nombreUsuario = row["NOMBREUSUARIO"].ToString(),
                        descripcion = row["DESCRIPCION"].ToString(),
                        tipoTransaccion = row["TIPOTRANSACCION"].ToString(),
                        fecha = row["FECHA"].ToString(),
                        tabla = row["TABLA"].ToString(),
                        idUsuario = Int32.Parse(row["IDUSUARIO"].ToString())
                    });
                return logs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<LogCajero> logsCajero()
        {
            DataSet ds = new DataSet();
            List<LogCajero> logs = new List<LogCajero>();
            try
            {
                OracleConnection connection = GetConnection();
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "seleccionar_bitacoraCajero";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(ds);
                }


                foreach (DataRow row in ds.Tables[0].Rows)
                    logs.Add(new LogCajero()
                    {
                        id = Int32.Parse(row["PK_IDBITACORACAJERO"].ToString()),
                        nombreUsuario = row["NOMBREUSUARIO"].ToString(),
                        fecha = row["FECHA"].ToString(),
                        idUsuario = Int32.Parse(row["IDUSUARIO"].ToString()),
                        idFactura = Int32.Parse(row["IDFACTURA"].ToString()),
                        numeroCaja = Int32.Parse(row["NUMEROCAJA"].ToString())
                    });

                return logs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<LogFactura> logsFactura()
        {
            DataSet ds = new DataSet();
            List<LogFactura> logs = new List<LogFactura>();
            try
            {
                OracleConnection connection = GetConnection();
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "seleccionar_bitacoraFactura";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(ds);
                }

                var dtFactura = ds.Tables[0].AsEnumerable().Distinct();

                foreach (DataRow row in dtFactura)
                {
                    List<DetalleFacturaLog> detalles = new List<DetalleFacturaLog>();
                    foreach (DataRow detalle in ds.Tables[0].AsEnumerable().Where(x => x["PK_IDBITACORAFACTURA"] == row["PK_IDBITACORAFACTURA"]).AsEnumerable())
                    {
                        detalles.Add(new DetalleFacturaLog() 
                        {
                            subTotal = long.Parse(detalle["SUBTOTAL"].ToString()),
                            EAN = long.Parse(detalle["EAN"].ToString()),
                            cantidad = Int32.Parse(detalle["CANTIDAD"].ToString())
                        });
                    }
                    logs.Add(new LogFactura()
                    {
                        id = Int32.Parse(row["PK_IDBITACORAFACTURA"].ToString()),
                        nombreUsuario = row["NOMBREUSUARIO"].ToString(),
                        fecha = row["FECHA"].ToString(),
                        idUsuario = Int32.Parse(row["IDUSUARIO"].ToString()),
                        idFactura = Int32.Parse(row["IDFACTURA"].ToString()),
                        numeroCaja = Int32.Parse(row["IDUSUARIO"].ToString()),
                        total = Int64.Parse(row["TOTAL"].ToString()),
                        detelle = detalles
                    });
                }
                return logs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
