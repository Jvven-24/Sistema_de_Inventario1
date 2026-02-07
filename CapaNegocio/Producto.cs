using System;

namespace CapaNegocio
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Estado { get; set; }

        public Producto() { }

        public Producto(string codigo, string nombre, int cantidad, decimal precio)
        {
            Codigo = codigo;
            Nombre = nombre;
            Cantidad = cantidad;
            Precio = precio;
        }
    }
}