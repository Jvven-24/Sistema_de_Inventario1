using System;
using System.Data;
using CapaDatos;

namespace CapaNegocio
{
    public class CN_Productos
    {
        private CD_Productos objCapaDatos = new CD_Productos();

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