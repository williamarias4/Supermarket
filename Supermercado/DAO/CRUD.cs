using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Supermercado.Data;

namespace Supermercado.DAO
{
    public class CRUD : ConexionBD
    {
        public void insertarProducto(int _idArea, long _ean, string _descripcion, int _precio, int _cantidad)
        {
            try
            {
                OracleConnection connection = GetConnection();
                string query = string.Format("INSERT INTO producto (pk_idproducto, fk_idarea, ean, descripcion, precio, cantidad) VALUES (producto_sq.nextval,'{0}','{1}','{2}','{3}','{4}');", _idArea, _ean, _descripcion, _precio, _cantidad);
                query = "INSERT INTO super.producto (pk_idproducto, fk_idarea, ean, descripcion, precio, cantidad) VALUES (18, 1, 3693693693693,'aceite',1800, 30)";
                OracleCommand accion = new OracleCommand(query, connection);
                accion.CommandTimeout = 60;
                connection.Open();
                //Ejecuta el query
                accion.ExecuteNonQuery();
                //Cierra conexion
                connection.Close();
            }
            catch (Exception ex)
            {

                
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
