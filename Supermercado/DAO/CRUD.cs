using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Supermercado.Data;
using System.Data;
using System.Linq;
using System.Text;

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
            DataSet ds = new DataSet();
            List<ProductoFresco> p = new List<ProductoFresco>();
            try
            {
                OracleConnection connection = GetConnection();
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "seleccionar_productosfrescos";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(ds);
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                    p.Add(new ProductoFresco()
                    {
                        id = Int32.Parse(row["PK_IDPRODUCTO"].ToString()),
                        idArea = Int32.Parse(row["FK_IDAREA"].ToString()),
                        ean = Int32.Parse(row["EAN"].ToString()),
                        descripcion = row["DESCRIPCION"].ToString(),
                        precio = Int64.Parse(row["PRECIO"].ToString()),
                        cantidad = Int32.Parse(row["CANTIDAD"].ToString()),
                        PLU = Int32.Parse(row["PLU"].ToString()),
                        Peso = Int64.Parse(row["PESO"].ToString())
                    });
                return p;
            }
            catch (Exception e)
            {
                throw e;
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
            //string sql = "select pk_idusuario,fk_idrol,nombreusuario,contrasena,nombre,apellido1,apellido2 from SUPER.usuario where nombreusuario =: " + username + " and contrasena =: " + password;
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
            DataSet ds = new DataSet();
            List<Producto> p = new List<Producto>();
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
                foreach (DataRow row in ds.Tables[0].Rows)
                    p.Add(new Producto()
                    {
                        id = Int32.Parse(row["PK_IDPRODUCTO"].ToString()),
                        idArea = Int32.Parse(row["FK_IDAREA"].ToString()),
                        ean = Int64.Parse(row["EAN"].ToString()),
                        descripcion = row["DESCRIPCION"].ToString(),
                        precio = Int64.Parse(row["PRECIO"].ToString()),
                        cantidad = Int32.Parse(row["CANTIDAD"].ToString())
                    });
                return p;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Producto> listarProductosDisponibles()
        {
            DataSet ds = new DataSet();
            List<Producto> p = new List<Producto>();
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
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int cantidad = 0;
                    if (row["FK_IDAREA"].ToString() != "4")
                    {
                        cantidad = Int32.Parse(row["CANTIDAD"].ToString());
                    }
                    else
                    {
                        cantidad = Int32.Parse(row["peso"].ToString());
                    }
                    p.Add(new Producto()
                    {
                        id = Int32.Parse(row["PK_IDPRODUCTO"].ToString()),
                        idArea = Int32.Parse(row["FK_IDAREA"].ToString()),
                        ean = Int64.Parse(row["EAN"].ToString()),
                        descripcion = row["DESCRIPCION"].ToString(),
                        precio = Int64.Parse(row["PRECIO"].ToString()),
                        cantidad = cantidad
                    });
                }
                return p;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Producto productoPorId(int id)
        {
            Producto p = new Producto();
            try
            {
                OracleConnection connection = GetConnection();
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "seleccionar_productoporid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
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
                }
                return p;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Area areaPorId(int id)
        {
            Area a = new Area();
            try
            {
                OracleConnection connection = GetConnection();
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "seleccionar_areaporid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
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
                }
                return a;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ProductoFresco productoFrescoPorId(int id)
        {
            ProductoFresco p = new ProductoFresco();
            try
            {
                OracleConnection connection = GetConnection();
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "seleccionar_productofrescoporid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("id", OracleDbType.Int32).Value = id;
                    cmd.ExecuteNonQuery();
                    OracleDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            p.IdFresco = dr.GetInt32(0);
                            p.id = dr.GetInt32(1);
                            p.PLU = dr.GetInt32(2);
                            p.Peso = dr.GetFloat(3);
                        }
                    }
                    connection.Close();
                    cmd.Dispose();
                    connection.Dispose();
                }
                return p;
            }
            catch (Exception e)
            {
                throw e;
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
                        id = Int64.Parse(row["PK_IDBITACORAMOVIMIENTOS"].ToString()),
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
                        id = Int64.Parse(row["PK_IDBITACORACAJERO"].ToString()),
                        nombreUsuario = row["NOMBREUSUARIO"].ToString(),
                        fecha = row["FECHA"].ToString(),
                        idUsuario = Int32.Parse(row["IDUSUARIO"].ToString()),
                        idFactura = Int64.Parse(row["IDFACTURA"].ToString()),
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
                        id = Int64.Parse(row["PK_IDBITACORAFACTURA"].ToString()),
                        nombreUsuario = row["NOMBREUSUARIO"].ToString(),
                        fecha = row["FECHA"].ToString(),
                        idUsuario = Int32.Parse(row["IDUSUARIO"].ToString()),
                        idFactura = Int64.Parse(row["IDFACTURA"].ToString()),
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

        public void agregarLogCajero(Factura factura)
        {
            try
            {
                OracleConnection connectionString = GetConnection();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connectionString;
                cmd.CommandText = "insertar_bitacoracajero_sp";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_pk_idbitacoracajero", OracleDbType.Int32).Value = factura.idFactura;
                cmd.Parameters.Add("p_idusuario", OracleDbType.Int32).Value = factura.idUsuario;
                cmd.Parameters.Add("p_nombreusuario", OracleDbType.Varchar2).Value = factura.nombreUsuario;
                cmd.Parameters.Add("p_numerocaja", OracleDbType.Int32).Value = factura.numeroCaja;
                cmd.Parameters.Add("p_idfactura", OracleDbType.Int32).Value = factura.idFactura;
                cmd.Parameters.Add("p_fecha", OracleDbType.Date).Value = DateTime.Now;
                connectionString.Open();
                cmd.ExecuteNonQuery();
                connectionString.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        public void actualizarCantidad(int cantidad, int idProducto, int idArea)
        {
            try
            {
                string sproc = "actualizar_cantidad_producto_sp";

                if (idArea == 4)
                    sproc = "actualizar_cantidad_productofresco_sp";
                OracleConnection connectionString = GetConnection();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connectionString;
                cmd.CommandText = sproc;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = idProducto;
                cmd.Parameters.Add("p_cantidad", OracleDbType.Int32).Value = cantidad;
                connectionString.Open();
                cmd.ExecuteNonQuery();
                connectionString.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                
            }
            
        }

        public void agregarLogFactura(Factura factura)
        {
            try
            {
                int autoIdentity = 0;
                foreach (var detalle in factura.detalle)
                {
                    OracleConnection connectionString = GetConnection();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = connectionString;
                    cmd.CommandText = "insertar_bitacorafactura_sp";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_pk_idbitacorafactura", OracleDbType.Int64).Value = factura.idFactura + autoIdentity;
                    cmd.Parameters.Add("p_idfactura", OracleDbType.Int64).Value = factura.idFactura;
                    cmd.Parameters.Add("p_ean", OracleDbType.Int64).Value = detalle.ean;
                    cmd.Parameters.Add("p_cantidad", OracleDbType.Int32).Value = detalle.cantidad;
                    cmd.Parameters.Add("p_subtotal", OracleDbType.Int32).Value = detalle.subtotal;
                    cmd.Parameters.Add("p_total", OracleDbType.Int32).Value = factura.total;
                    cmd.Parameters.Add("p_idusuario", OracleDbType.Int32).Value = factura.idUsuario;
                    cmd.Parameters.Add("p_nombreusuario", OracleDbType.Varchar2).Value = factura.nombreUsuario;
                    cmd.Parameters.Add("p_fecha", OracleDbType.Date).Value = DateTime.Now;
                    connectionString.Open();
                    cmd.ExecuteNonQuery();
                    connectionString.Close();
                    cmd.Dispose();

                    autoIdentity++;
                    actualizarCantidad(detalle.cantidad, detalle.id, detalle.idArea);
                }
                
            }
            catch (Exception ex)
            {
            }
        }

        public void agregarLogMovimiento(int idUsuario, string nombreUsuario, string tabla, string tipoTransaccion, string descripcion)
        {
            try
            {
                string idMovimiento = DateTime.Now.Ticks.ToString();
                OracleConnection connectionString = GetConnection();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connectionString;
                cmd.CommandText = "insertar_bitacoramovimientos_sp";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_pk_idbitacoramovimientos", OracleDbType.Int64).Value = Int64.Parse(idMovimiento.Substring(idMovimiento.Length - 10));
                cmd.Parameters.Add("p_idusuario", OracleDbType.Int32).Value = idUsuario;
                cmd.Parameters.Add("p_nombreusuario", OracleDbType.Varchar2).Value = nombreUsuario;
                cmd.Parameters.Add("p_tabla", OracleDbType.Varchar2).Value = tabla;
                cmd.Parameters.Add("p_tipotransaccion", OracleDbType.Varchar2).Value = tipoTransaccion;
                cmd.Parameters.Add("p_descripcion", OracleDbType.Varchar2).Value = descripcion;
                cmd.Parameters.Add("p_fecha", OracleDbType.Date).Value = DateTime.Now;
                connectionString.Open();
                cmd.ExecuteNonQuery();
                connectionString.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
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
            DataSet ds = new DataSet();
            List<Area> a = new List<Area>();
            try
            {
                OracleConnection connection = GetConnection();
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "seleccionar_areas";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(ds);
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                    a.Add(new Area()
                    {
                        idArea = Int32.Parse(row["PK_IDAREA"].ToString()),
                        descripcion = row["DESCRIPCION"].ToString()
                    });
                return a;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Area areaPorProducto(int idArea)
        {
            Area a = new Area();
            try
            {
                OracleConnection connection = GetConnection();
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "seleccionar_areaporproducto";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("id", OracleDbType.Int32).Value = idArea;
                    cmd.ExecuteNonQuery();
                    OracleDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            a.idArea = Int32.Parse(dr.GetString(0));
                            a.descripcion = dr.GetString(1);
                        }
                    }
                    connection.Close();
                    cmd.Dispose();
                    connection.Dispose();
                }
                return a;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
