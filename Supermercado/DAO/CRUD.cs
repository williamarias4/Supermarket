using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Supermercado.Data;

namespace Supermercado.DAO
{
    public class CRUD : ConexionBD
    {
        public void insertarProducto(Producto p)
        {
            try {
                OracleConnection connectionString = GetConnection();
                string sql = "insert into super.producto(pk_idproducto, fk_idarea, ean, descripcion, precio, cantidad) values (:id, :idArea, :ean, :descripcion, :precio, :cantidad)";
                OracleConnection connection = connectionString;
                connection.Open();
                OracleCommand cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("id", OracleDbType.Int32).Value = p.id;
                cmd.Parameters.Add("idArea", OracleDbType.Int64).Value = p.idArea;
                cmd.Parameters.Add("ean", OracleDbType.Long).Value = p.ean;
                cmd.Parameters.Add("descripcion", OracleDbType.Varchar2).Value = p.descripcion;
                cmd.Parameters.Add("precio", OracleDbType.BinaryFloat).Value = p.precio;
                cmd.Parameters.Add("cantidad", OracleDbType.Int64).Value = p.cantidad;
                cmd.ExecuteNonQuery();
                connection.Close();
                cmd.Dispose();
                connection.Dispose();
            } catch (Exception ex) {
            }
        }

        public DataSet ListaClientes()
        {
            DataSet ds = new DataSet();
            try
            {
                OracleConnection connection = GetConnection();
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "seleccionar_productos";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(ds);
                }
                return ds;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
