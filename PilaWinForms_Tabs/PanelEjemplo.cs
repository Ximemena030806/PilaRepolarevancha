using System;
using System.Drawing;
using System.Windows.Forms;
using PilaRepo.Shared;

namespace PilaWinForms_Tabs
{
    /// <summary>
    /// Ejemplo con AMBAS pilas de libros a la vez: ESTANTERÍA (arreglo de tamaño fijo) y
    /// BODEGA (nodos enlazados, sin límite). Los botones de arriba usan las dos pilas juntas
    /// (la lógica está en Shared/EjemploBiblioteca.cs); cada pila además tiene sus propios botones.
    /// </summary>
    internal class PanelEjemplo : TableLayoutPanel, IRefrescable
    {
        private readonly EjemploBiblioteca _ejemplo = new EjemploBiblioteca();
        private readonly PanelPila<Libro> _panelEstanteria;
        private readonly PanelPila<Libro> _panelBodega;
        private readonly TextBox _txtTitulo = new TextBox { Width = 180, Margin = new Padding(3, 5, 3, 3), Text = "Don Quijote de la Mancha" };
        private readonly TextBox _txtAutor = new TextBox { Width = 180, Margin = new Padding(3, 5, 3, 3), Text = "Miguel de Cervantes" };
        private readonly Label _lblEstado = new Label { Dock = DockStyle.Fill, AutoSize = false };

        public PanelEjemplo()
        {
            Dock = DockStyle.Fill;
            Padding = new Padding(8);
            ColumnCount = 1;
            RowCount = 3;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            RowStyles.Add(new RowStyle(SizeType.AutoSize));          // 0 controles conjuntos
            RowStyles.Add(new RowStyle(SizeType.Percent, 100));      // 1 las dos pilas
            RowStyles.Add(new RowStyle(SizeType.Absolute, 40));      // 2 estado

            // Fila 0: libro nuevo + operaciones que usan las DOS pilas
            var controles = UiHelper.NuevoFlow();
            controles.Controls.Add(UiHelper.Etiqueta("Título:"));
            controles.Controls.Add(_txtTitulo);
            controles.Controls.Add(UiHelper.Etiqueta("Autor:"));
            controles.Controls.Add(_txtAutor);
            controles.Controls.Add(UiHelper.Boton("Apilar libro", (s, e) => Ejecutar(() =>
                _ejemplo.Apilar(new Libro(_txtTitulo.Text, _txtAutor.Text))), 110));
            controles.Controls.Add(UiHelper.Boton("Estantería → Bodega", (s, e) => Ejecutar(_ejemplo.MoverEstanteriaABodega), 160));
            controles.Controls.Add(UiHelper.Boton("Bodega → Estantería", (s, e) => Ejecutar(_ejemplo.MoverBodegaAEstanteria), 160));

            // Fila 1: dos columnas, una pila en cada una
            _panelEstanteria = MainForm.NuevoPanelLibros(_ejemplo.Estanteria);
            _panelBodega = MainForm.NuevoPanelLibros(_ejemplo.Bodega);

            var dos = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            dos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            dos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            dos.Controls.Add(Columna($"ESTANTERÍA — arreglo, tamaño {EjemploBiblioteca.CapacidadEstanteria}", _panelEstanteria), 0, 0);
            dos.Controls.Add(Columna("BODEGA — nodos enlazados, sin límite", _panelBodega), 1, 0);

            Controls.Add(controles, 0, 0);
            Controls.Add(dos, 0, 1);
            Controls.Add(_lblEstado, 0, 2);

            UiHelper.MostrarEstado(_lblEstado,
                "Apila libros: cuando la estantería (arreglo) se llena, el siguiente libro se va a la bodega (nodos).", false);
        }

        private static Control Columna(string titulo, Control panel)
        {
            var col = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
            col.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            col.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            col.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var lbl = new Label { Text = titulo, AutoSize = true, Margin = new Padding(3, 4, 3, 2) };
            lbl.Font = new Font(lbl.Font, FontStyle.Bold);

            col.Controls.Add(lbl, 0, 0);
            col.Controls.Add(panel, 0, 1);
            return col;
        }

        private void Ejecutar(Func<string> accion)
        {
            try
            {
                UiHelper.MostrarEstado(_lblEstado, accion(), false);
            }
            catch (InvalidOperationException ex)       // Overflow / Underflow
            {
                UiHelper.MostrarEstado(_lblEstado, "Error: " + ex.Message, true);
            }
            Refrescar();
        }

        public void Refrescar()
        {
            _panelEstanteria.Refrescar();
            _panelBodega.Refrescar();
        }
    }
}
