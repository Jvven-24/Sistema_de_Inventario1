using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;

namespace Sistema_de_Inventario
{
    public partial class Form1 : Form
    {
        private CN_Productos objNegocio = new CN_Productos();

        public Form1()
        {
            InitializeComponent();
        }

        // Botón: Agregar Producto
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                string codigo = PromptInput("Ingrese el código del producto:");
                if (string.IsNullOrWhiteSpace(codigo)) return;

                string nombre = PromptInput("Ingrese el nombre del producto:");
                if (string.IsNullOrWhiteSpace(nombre)) return;

                if (!int.TryParse(PromptInput("Ingrese la cantidad:"), out int cantidad))
                {
                    MessageBox.Show("La cantidad debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(PromptInput("Ingrese el precio:"), out decimal precio))
                {
                    MessageBox.Show("El precio debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Producto producto = new Producto(codigo, nombre, cantidad, precio);

                if (objNegocio.AgregarProducto(producto, out string mensaje))
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProductos();
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Botón: Consultar Producto
        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                string opcion = PromptInput("¿Desea buscar por (1) Código o (2) Nombre?");

                if (opcion == "1")
                {
                    string codigo = PromptInput("Ingrese el código a buscar:");
                    if (string.IsNullOrWhiteSpace(codigo)) return;

                    DataTable resultado = objNegocio.ConsultarPorCodigo(codigo);
                    if (resultado.Rows.Count > 0)
                    {
                        dgvProductos.DataSource = resultado;
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron productos con ese código.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (opcion == "2")
                {
                    string nombre = PromptInput("Ingrese el nombre a buscar:");
                    if (string.IsNullOrWhiteSpace(nombre)) return;

                    DataTable resultado = objNegocio.ConsultarPorNombre(nombre);
                    if (resultado.Rows.Count > 0)
                    {
                        dgvProductos.DataSource = resultado;
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron productos con ese nombre.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Botón: Actualizar Cantidad
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(PromptInput("Ingrese el ID del producto a actualizar:"), out int idProducto))
                {
                    MessageBox.Show("El ID debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(PromptInput("Ingrese la nueva cantidad:"), out int nuevaCantidad))
                {
                    MessageBox.Show("La cantidad debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (objNegocio.ActualizarCantidad(idProducto, nuevaCantidad, out string mensaje))
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProductos();
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Botón: Eliminar Producto
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(PromptInput("Ingrese el ID del producto a eliminar:"), out int idProducto))
                {
                    MessageBox.Show("El ID debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult resultado = MessageBox.Show("¿Está seguro de que desea eliminar este producto?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    if (objNegocio.EliminarProducto(idProducto, out string mensaje))
                    {
                        MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarProductos();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Botón: Listar Productos
        private void btnListar_Click(object sender, EventArgs e)
        {
            try
            {
                CargarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método auxiliar para cargar productos
        private void CargarProductos()
        {
            try
            {
                DataTable productos = objNegocio.ListarProductos();
                dgvProductos.DataSource = productos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método auxiliar para mostrar cuadro de entrada
        private string PromptInput(string mensaje)
        {
            Form prompt = new Form()
            {
                Text = "Entrada",
                Width = 300,
                Height = 150,
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label label = new Label() { Left = 20, Top = 20, Text = mensaje, Width = 250, Height = 40 };
            TextBox textBox = new TextBox() { Left = 20, Top = 60, Width = 250 };
            Button okBtn = new Button() { Text = "OK", Left = 50, Width = 100, Top = 85, DialogResult = DialogResult.OK };
            Button cancelBtn = new Button() { Text = "Cancelar", Left = 170, Width = 100, Top = 85, DialogResult = DialogResult.Cancel };

            prompt.Controls.Add(label);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(okBtn);
            prompt.Controls.Add(cancelBtn);
            prompt.AcceptButton = okBtn;
            prompt.CancelButton = cancelBtn;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty;
        }
    }
}
