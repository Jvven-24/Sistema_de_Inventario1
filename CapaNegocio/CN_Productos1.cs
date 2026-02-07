using CapaDatos;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class CN_FuncionesProductos
    {
        private Conexion objConexion = new Conexion();

        // Agregar Producto
        public int AgregarProducto(string codigo, string nombre, int cantidad, decimal precio)
        {
            SqlConnection conexion = objConexion.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand("INSERT INTO Productos (Codigo, Nombre, Cantidad, Precio, FechaRegistro, Estado) VALUES (@codigo, @nombre, @cantidad, @precio, @fechaRegistro, @estado)", conexion))
                {
                    comando.Parameters.AddWithValue("@codigo", codigo);
                    comando.Parameters.AddWithValue("@nombre", nombre);
                    comando.Parameters.AddWithValue("@cantidad", cantidad);
                    comando.Parameters.AddWithValue("@precio", precio);
                    comando.Parameters.AddWithValue("@fechaRegistro", DateTime.Now);
                    comando.Parameters.AddWithValue("@estado", true);

                    return comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar producto: " + ex.Message);
            }
            finally
            {
                objConexion.CerrarConexion(conexion);
            }
        }

        // Verificar si existe un código
        public bool ExisteCodigo(string codigo)
        {
            SqlConnection conexion = objConexion.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand("SELECT COUNT(*) FROM Productos WHERE Codigo = @codigo", conexion))
                {
                    comando.Parameters.AddWithValue("@codigo", codigo);
                    int resultado = (int)comando.ExecuteScalar();
                    return resultado > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar código: " + ex.Message);
            }
            finally
            {
                objConexion.CerrarConexion(conexion);
            }
        }

        // Listar todos los productos
        public DataTable ListarProductos()
        {
            SqlConnection conexion = objConexion.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand("SELECT IdProducto, Codigo, Nombre, Cantidad, Precio, FechaRegistro, Estado FROM Productos WHERE Estado = 1 ORDER BY Nombre", conexion))
                {
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    DataTable tabla = new DataTable();
                    adaptador.Fill(tabla);
                    return tabla;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar productos: " + ex.Message);
            }
            finally
            {
                objConexion.CerrarConexion(conexion);
            }
        }

        // Consultar por Código
        public DataTable ConsultarPorCodigo(string codigo)
        {
            SqlConnection conexion = objConexion.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand("SELECT IdProducto, Codigo, Nombre, Cantidad, Precio, FechaRegistro, Estado FROM Productos WHERE Codigo LIKE @codigo AND Estado = 1", conexion))
                {
                    comando.Parameters.AddWithValue("@codigo", "%" + codigo + "%");
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    DataTable tabla = new DataTable();
                    adaptador.Fill(tabla);
                    return tabla;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar por código: " + ex.Message);
            }
            finally
            {
                objConexion.CerrarConexion(conexion);
            }
        }

        // Consultar por Nombre
        public DataTable ConsultarPorNombre(string nombre)
        {
            SqlConnection conexion = objConexion.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand("SELECT IdProducto, Codigo, Nombre, Cantidad, Precio, FechaRegistro, Estado FROM Productos WHERE Nombre LIKE @nombre AND Estado = 1", conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", "%" + nombre + "%");
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    DataTable tabla = new DataTable();
                    adaptador.Fill(tabla);
                    return tabla;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar por nombre: " + ex.Message);
            }
            finally
            {
                objConexion.CerrarConexion(conexion);
            }
        }

        // Actualizar Cantidad
        public bool ActualizarCantidad(int idProducto, int nuevaCantidad)
        {
            SqlConnection conexion = objConexion.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand("UPDATE Productos SET Cantidad = @cantidad WHERE IdProducto = @id AND Estado = 1", conexion))
                {
                    comando.Parameters.AddWithValue("@cantidad", nuevaCantidad);
                    comando.Parameters.AddWithValue("@id", idProducto);

                    int resultado = comando.ExecuteNonQuery();
                    return resultado > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar cantidad: " + ex.Message);
            }
            finally
            {
                objConexion.CerrarConexion(conexion);
            }
        }

        // Eliminar Producto (lógico)
        public bool EliminarProducto(int idProducto)
        {
            SqlConnection conexion = objConexion.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand("UPDATE Productos SET Estado = 0 WHERE IdProducto = @id", conexion))
                {
                    comando.Parameters.AddWithValue("@id", idProducto);

                    int resultado = comando.ExecuteNonQuery();
                    return resultado > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar producto: " + ex.Message);
            }
            finally
            {
                objConexion.CerrarConexion(conexion);
            }
        }
    }
    public class CN_Productos
    {
        private CN_FuncionesProductos objCapaDatos = new CN_FuncionesProductos();

        // Validar datos del producto
        private bool ValidarProducto(string codigo, string nombre, int cantidad, decimal precio, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(codigo))
            {
                mensaje = "El código del producto es obligatorio.";
                return false;
            }

            if (codigo.Length > 20)
            {
                mensaje = "El código no puede superar 20 caracteres.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                mensaje = "El nombre del producto es obligatorio.";
                return false;
            }

            if (nombre.Length > 100)
            {
                mensaje = "El nombre no puede superar 100 caracteres.";
                return false;
            }

            if (cantidad < 0)
            {
                mensaje = "La cantidad no puede ser negativa.";
                return false;
            }

            if (precio <= 0)
            {
                mensaje = "El precio debe ser mayor a 0.";
                return false;
            }

            return true;
        }

        // Agregar Producto
        public bool AgregarProducto(Producto producto, out string mensaje)
        {
            mensaje = string.Empty;

            // Validar datos
            if (!ValidarProducto(producto.Codigo, producto.Nombre, producto.Cantidad, producto.Precio, out mensaje))
            {
                return false;
            }

            // Verificar si el código ya existe
            try
            {
                if (objCapaDatos.ExisteCodigo(producto.Codigo))
                {
                    mensaje = "El código del producto ya existe en el sistema.";
                    return false;
                }

                int resultado = objCapaDatos.AgregarProducto(
                    producto.Codigo,
                    producto.Nombre,
                    producto.Cantidad,
                    producto.Precio
                );

                if (resultado > 0)
                {
                    mensaje = "Producto agregado exitosamente.";
                    return true;
                }
                else
                {
                    mensaje = "No se pudo agregar el producto.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                mensaje = "Error: " + ex.Message;
                return false;
            }
        }

        // Listar Productos
        public DataTable ListarProductos()
        {
            try
            {
                return objCapaDatos.ListarProductos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en capa de negocio: " + ex.Message);
            }
        }

        // Consultar por Código
        public DataTable ConsultarPorCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new Exception("Debe ingresar un código para buscar.");
            }

            return objCapaDatos.ConsultarPorCodigo(codigo.Trim());
        }

        // Consultar por Nombre
        public DataTable ConsultarPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new Exception("Debe ingresar un nombre para buscar.");
            }

            return objCapaDatos.ConsultarPorNombre(nombre.Trim());
        }

        // Actualizar Cantidad
        public bool ActualizarCantidad(int idProducto, int nuevaCantidad, out string mensaje)
        {
            mensaje = string.Empty;

            if (nuevaCantidad < 0)
            {
                mensaje = "La cantidad no puede ser negativa.";
                return false;
            }

            try
            {
                bool resultado = objCapaDatos.ActualizarCantidad(idProducto, nuevaCantidad);

                if (resultado)
                {
                    mensaje = "Cantidad actualizada exitosamente.";
                    return true;
                }
                else
                {
                    mensaje = "No se encontró el producto.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                mensaje = "Error: " + ex.Message;
                return false;
            }
        }

        // Eliminar Producto
        public bool EliminarProducto(int idProducto, out string mensaje)
        {
            mensaje = string.Empty;

            try
            {
                bool resultado = objCapaDatos.EliminarProducto(idProducto);

                if (resultado)
                {
                    mensaje = "Producto descontinuado exitosamente.";
                    return true;
                }
                else
                {
                    mensaje = "No se encontró el producto.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                mensaje = "Error: " + ex.Message;
                return false;
            }
        }
    }
}