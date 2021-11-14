﻿using System;
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
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connectionString;
                cmd.CommandText = "insertar_producto_sp";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("pk_idproducto)", OracleDbType.Int32).Value = p.id;
                cmd.Parameters.Add("fk_idArea", OracleDbType.Int32).Value = p.idArea;
                cmd.Parameters.Add("ean", OracleDbType.Long).Value = p.ean;
                cmd.Parameters.Add("descripcion", OracleDbType.Varchar2).Value = p.descripcion;
                cmd.Parameters.Add("precio", OracleDbType.BinaryFloat).Value = p.precio;
                cmd.Parameters.Add("cantidad", OracleDbType.Int32).Value = p.cantidad;
                connectionString.Open();
                cmd.ExecuteNonQuery();
                connectionString.Close();
                cmd.Dispose();
            } catch (Exception ex) {
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
                cmd.Parameters.Add("pk_idproductofresco", OracleDbType.Int32).Value = f.IdFresco;
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
    }
}
