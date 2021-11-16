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
        public int rol;
        public void insertarProducto(Producto p)
        {
            try {
                OracleConnection connectionString = GetConnection();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connectionString;
                cmd.CommandText = "insertar_producto_sp";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("fk_idArea", OracleDbType.Int32).Value = p.idArea;
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

        public void insertarArea(Area a)
        {
            try
            {
                OracleConnection connectionString = GetConnection();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connectionString;
                cmd.CommandText = "insertar_area_sp";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("descripcion", OracleDbType.Varchar2).Value = a.descripcion;
                connectionString.Open();
                cmd.ExecuteNonQuery();
                connectionString.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        public void insertarProductoFresco(ProductoFresco f)
        {
            try
            {
                OracleConnection connectionString = GetConnection();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connectionString;
                cmd.CommandText = "insertar_productofresco_sp";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("fk_idproducto", OracleDbType.Int32).Value = f.id;
                cmd.Parameters.Add("plu", OracleDbType.Int32).Value = f.PLU;
                cmd.Parameters.Add("peso", OracleDbType.BinaryFloat).Value = f.Peso;
                connectionString.Open();
                cmd.ExecuteNonQuery();
                connectionString.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        public List<ProductoFresco> listarProductosFrescos()
        {
            try
            {
                List<ProductoFresco> productos = new List<ProductoFresco>();
                OracleConnection connectionString = GetConnection();
                string sql = "select p.pk_idproducto, p.fk_idarea, p.ean, p.descripcion, p.precio, p.cantidad, f.plu, f.peso  from producto p, productofresco f where p.pk_idproducto = f.fk_idproducto";
                OracleConnection connection = connectionString;
                connection.Open();
                OracleCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ProductoFresco p = new ProductoFresco();
                        p.id = dr.GetInt32(0);
                        p.idArea = dr.GetInt32(1);
                        p.ean = dr.GetInt64(2);
                        p.descripcion = dr.GetString(3);
                        p.precio = dr.GetFloat(4);
                        p.cantidad = dr.GetInt32(5);
                        p.PLU = dr.GetInt32(6);
                        p.Peso = dr.GetFloat(7);
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


        public Rol buscarRol(int idRol)
        {
            Rol rol = new Rol();
            OracleConnection connectionString = GetConnection();
            string sql = "select pk_idrol, descripcion from SUPER.rol where pk_idrol =:id";
            OracleConnection connection = connectionString;
            connection.Open();
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add("id", OracleDbType.Int32).Value = idRol;
            cmd.ExecuteNonQuery();
            OracleDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    rol.IdRol = dr.GetInt32(0);
                    rol.Descripcion = dr.GetString(1);
                }
            }
            connection.Close();
            cmd.Dispose();
            connection.Dispose();
            return rol;
        }


        public Usuario validateCredentials(string username, string password)
        {
            Usuario user = new Usuario();
            OracleConnection connectionString = GetConnection();
            string sql = "select pk_idusuario,fk_idrol,nombreusuario,contrasena,nombre,apellido1,apellido2 from SUPER.usuario where nombreusuario =:usern and contrasena =: pass";
            //string sql = "select verificar_usuario_sp('"+username+"','"+ password+"')"+" from dual";
            OracleConnection connection = connectionString;
            connection.Open();
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add("nombreusuario", OracleDbType.Varchar2).Value = username;
            cmd.Parameters.Add("contrasena", OracleDbType.Varchar2).Value = user.CreateMD5(password);
            cmd.ExecuteNonQuery();
            OracleDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    
                    user.IdUsuario = dr.GetInt32(0);
                    Rol rol = buscarRol(dr.GetInt32(1));
                    user.Rol = rol;
                    user.NombreUsuario= username;
                    user.Contrasena= password;
                    user.Nombre = dr.GetString(4);
                    user.Apellido1 = dr.GetString(5);
                    user.Apellido2 = dr.GetString(6);
                }
            }
            connection.Close();
            cmd.Dispose();
            connection.Dispose();
            return user;
        }

        public List<Producto> listarProductos()
        {
            try
            {
                List<Producto> productos = new List<Producto>();
                OracleConnection connectionString = GetConnection();
                string sql = "select producto.pk_idproducto, producto.fk_idarea, producto.ean, producto.descripcion, producto.precio, producto.cantidad from SUPER.producto order by 1 DESC";
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

        public Area areaPorId(int id)
        {
            try
            {
                Area a = new Area();
                OracleConnection connectionString = GetConnection();
                string sql = "select area.descripcion from SUPER.area where area.pk_idarea=:id";
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
                        a.descripcion = dr.GetString(0);
                    }
                }
                connection.Close();
                cmd.Dispose();
                connection.Dispose();
                return a;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ProductoFresco productoFrescoPorId(int id)
        {
            try
            {
                ProductoFresco f = new ProductoFresco();
                OracleConnection connectionString = GetConnection();
                string sql = "select pk_idproductofresco, fk_idproducto, PLU, peso from productofresco where fk_idproducto = :id";
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
                        f.IdFresco = dr.GetInt32(0);
                        f.id = dr.GetInt32(1);
                        f.PLU = dr.GetInt32(2);
                        f.Peso = dr.GetFloat(3);
                    }
                }
                connection.Close();
                cmd.Dispose();
                connection.Dispose();
                return f;
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
                cmd.Parameters.Add("pk_idproducto", OracleDbType.Int32).Value = p.id;
                cmd.Parameters.Add("fk_idArea", OracleDbType.Int32).Value = p.idArea;
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

        public void actualizarArea(Area a)
        {
            try
            {
                OracleConnection connectionString = GetConnection();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connectionString;
                cmd.CommandText = "actualizar_area_sp";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("pk_idarea", OracleDbType.Int32).Value = a.idArea;
                cmd.Parameters.Add("descripcion", OracleDbType.Varchar2).Value = a.descripcion;
                connectionString.Open();
                cmd.ExecuteNonQuery();
                connectionString.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        public void actualizarProductoFresco(ProductoFresco f)
        {
            try
            {
                OracleConnection connectionString = GetConnection();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connectionString;
                cmd.CommandText = "actualizar_productofresco_sp";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_pk_idproducto", OracleDbType.Int32).Value = f.id;
                cmd.Parameters.Add("plu", OracleDbType.Long).Value = f.PLU;
                cmd.Parameters.Add("peso", OracleDbType.Varchar2).Value = f.Peso;
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
                cmd.Parameters.Add("pk_idproducto", OracleDbType.Int32).Value = id;
                connectionString.Open();
                cmd.ExecuteNonQuery();
                connectionString.Close();
                cmd.Dispose();
                eliminarProductoFresco(id);
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

                var dtFactura = ds.Tables[0].AsEnumerable().GroupBy(x => x["IDFACTURA"].ToString()).Select(x => x.First()).ToList();

                foreach (DataRow row in dtFactura)
                {
                    List<DetalleFacturaLog> detalles = new List<DetalleFacturaLog>();
                    foreach (DataRow detalle in ds.Tables[0].AsEnumerable().Where(x => x["IDFACTURA"].ToString() == row["IDFACTURA"].ToString()).AsEnumerable())
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

        public void eliminarArea(int id)
        {
            try
            {
                OracleConnection connectionString = GetConnection();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connectionString;
                cmd.CommandText = "eliminar_area_sp";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_pk_idarea", OracleDbType.Int32).Value = id;
                connectionString.Open();
                cmd.ExecuteNonQuery();
                connectionString.Close();
                cmd.Dispose();
                eliminarProductoFresco(id);
            }
            catch (Exception ex)
            {
            }
        }

        public void eliminarProductoFresco(int id)
        {
            try
            {
                OracleConnection connectionString = GetConnection();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connectionString;
                cmd.CommandText = "eliminar_productofresco_sp";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_pk_idproducto", OracleDbType.Int32).Value = id;
                connectionString.Open();
                cmd.ExecuteNonQuery();
                connectionString.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        public List<Area> listarAreas()
        {
            try
            {
                List<Area> a = new List<Area>();
                OracleConnection connectionString = GetConnection();
                string sql = "select pk_idarea, descripcion from area";
                OracleConnection connection = connectionString;
                connection.Open();
                OracleCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Area area = new Area();
                        area.idArea = dr.GetInt32(0);
                        area.descripcion = dr.GetString(1);
                        a.Add(area);
                    }
                }
                connection.Close();
                cmd.Dispose();
                connection.Dispose();
                return a;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Area areaPorProducto(int idArea)
        {
            try
            {
                Area a = new Area();
                OracleConnection connectionString = GetConnection();
                string sql = "select pk_idarea, descripcion from area where pk_idarea = :idArea";
                OracleConnection connection = connectionString;
                connection.Open();
                OracleCommand cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("idArea", OracleDbType.Int32).Value = idArea;
                cmd.ExecuteNonQuery();
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        a.idArea = dr.GetInt32(0);
                        a.descripcion = dr.GetString(1);
                    }
                }
                connection.Close();
                cmd.Dispose();
                connection.Dispose();
                return a;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
