using System;
using System.Drawing;
using System.Windows.Forms;
using PilaRepo.Shared;

namespace PilaWinForms_Tabs
{
    /// <summary>Ventana principal: TabControl con 6 pestañas (Libros trae 5 sub-pestañas).</summary>
    internal class MainForm : Form
    {
        private readonly TabControl _tabs = new TabControl { Dock = DockStyle.Fill };

        public MainForm()
        {
            Text = "Pila: estática, dinámica, nativa, libros (5 formas), ejemplo con ambas pilas y comparación";
            ClientSize = new Size(1000, 660);
            MinimumSize = new Size(820, 560);
            StartPosition = FormStartPosition.CenterScreen;

            _tabs.TabPages.Add(CrearTabEstatica());
            _tabs.TabPages.Add(CrearTabDinamica());
            _tabs.TabPages.Add(CrearTabNativa());
            _tabs.TabPages.Add(CrearTabLibros());
            _tabs.TabPages.Add(CrearTabEjemplo());
            _tabs.TabPages.Add(CrearTabComparacion());

            // Al cambiar de pestaña se refresca la grilla visible.
            _tabs.SelectedIndexChanged += (s, e) =>
            {
                if (_tabs.SelectedTab != null)
                    foreach (Control c in _tabs.SelectedTab.Controls)
                        if (c is IRefrescable r) r.Refrescar();
            };

            Controls.Add(_tabs);
        }

        private static TabPage NuevaPagina(string titulo, Control contenido)
        {
            var tp = new TabPage(titulo);
            tp.Controls.Add(contenido);
            return tp;
        }

        private static int LeerEntero(string[] campos)
        {
            if (!int.TryParse(campos[0].Trim(), out int v))
                throw new FormatException("escribe un número entero válido.");
            return v;
        }

        // Panel de una pila numérica (Estática, Dinámica o Nativa).
        private static PanelPila<int> NuevoPanelEnteros(IPilaVista<int> pila) =>
            new PanelPila<int>(pila, new[] { "Valor" }, new[] { "Valor (int):" },
                               new[] { "10" }, LeerEntero, LeerEntero, v => new[] { v.ToString() });

        // 1) Pila Estática (clase stack_ del semestre pasado): arreglo de tamaño fijo.
        //    Por defecto 4 (índices 0-3), vacía con top = -1.
        private TabPage CrearTabEstatica()
        {
            var panel = NuevoPanelEnteros(FabricaPilas.Estatica(4));

            var num = new NumericUpDown { Minimum = 1, Maximum = 50, Value = 4, Width = 60, Margin = new Padding(3, 5, 3, 3) };
            panel.AgregarEncabezado(UiHelper.Etiqueta("Tamaño de la pila:"));
            panel.AgregarEncabezado(num);
            panel.AgregarEncabezado(UiHelper.Boton("Crear / Reiniciar Pila", (s, e) =>
            {
                panel.PilaActual = FabricaPilas.Estatica((int)num.Value);
                panel.Estado($"Pila estática creada: tamaño {(int)num.Value} (índices 0-{(int)num.Value - 1}), top = -1");
            }, 160));

            return NuevaPagina("Pila Estática", panel);
        }

        // 2) Pila Dinámica (clase stack_ del semestre pasado): nodos enlazados con Prev, sin límite.
        private TabPage CrearTabDinamica() =>
            NuevaPagina("Pila Dinámica", NuevoPanelEnteros(FabricaPilas.Dinamica()));

        // 3) Pila Nativa: respaldada por Stack<T>, vacía.
        private TabPage CrearTabNativa() =>
            NuevaPagina("Pila Nativa (Stack<T>)", NuevoPanelEnteros(FabricaPilas.Nativa()));

        // ------------------------------------------------------------------
        // Paneles reutilizables para los datos de ejemplo
        // ------------------------------------------------------------------

        internal static PanelPila<Libro> NuevoPanelLibros(IPilaVista<Libro> pila) =>
            new PanelPila<Libro>(pila,
                new[] { "Título", "Autor" }, new[] { "Título:", "Autor:" },
                new[] { "Don Quijote de la Mancha", "Miguel de Cervantes" },
                c => new Libro(c[0], c[1]),
                c => new Libro(c[0], ""),                  // Contains busca solo por título
                l => new[] { l.Titulo, l.Autor },
                "Contains (por título)");

        /// <summary>Agrega al panel de arreglo el selector de tamaño y el botón "Crear / Reiniciar".</summary>
        private static PanelPila<T> ConTamaño<T>(PanelPila<T> panel, Func<int, IPilaVista<T>> crear)
        {
            // Mínimo 3: la pila de ejemplo ya viene precargada con 3 datos.
            var num = new NumericUpDown { Minimum = 3, Maximum = 50, Value = 5, Width = 60, Margin = new Padding(3, 5, 3, 3) };
            panel.AgregarEncabezado(UiHelper.Etiqueta("Tamaño del arreglo:"));
            panel.AgregarEncabezado(num);
            panel.AgregarEncabezado(UiHelper.Boton("Crear / Reiniciar Pila", (s, e) =>
            {
                panel.PilaActual = crear((int)num.Value);
                panel.Estado($"Pila de arreglo creada: tamaño {(int)num.Value} (índices 0-{(int)num.Value - 1}), con 3 datos precargados");
            }, 160));
            return panel;
        }

        /// <summary>TabControl interno: una sub-pestaña por implementación de la misma pila.</summary>
        private static TabControl CrearGrupo(params (string Titulo, Control Panel)[] paginas)
        {
            var grupo = new TabControl { Dock = DockStyle.Fill };
            foreach (var (titulo, panel) in paginas)
                grupo.TabPages.Add(NuevaPagina(titulo, panel));

            grupo.SelectedIndexChanged += (s, e) =>
            {
                if (grupo.SelectedTab != null)
                    foreach (Control c in grupo.SelectedTab.Controls)
                        if (c is IRefrescable r) r.Refrescar();
            };
            return grupo;
        }

        // 4) Pila de LIBROS en 5 formas, todas PRECARGADAS con los mismos 3 libros.
        //    CON arreglo: T[] propio, Stack<T>, List<T>.   SIN arreglo: nodos propios, LinkedList<T>.
        private TabPage CrearTabLibros() =>
            NuevaPagina("Pila de Libros", CrearGrupo(
                ("Arreglo propio T[]", ConTamaño(NuevoPanelLibros(FabricaPilas.LibrosEstatica()), FabricaPilas.LibrosEstatica)),
                ("Stack<T> (con arreglo)", NuevoPanelLibros(FabricaPilas.LibrosNativa())),
                ("List<T> (con arreglo)", NuevoPanelLibros(FabricaPilas.LibrosListaPrecargada())),
                ("Nodos propios (sin arreglo)", NuevoPanelLibros(FabricaPilas.LibrosPrecargada())),
                ("LinkedList<T> (sin arreglo)", NuevoPanelLibros(FabricaPilas.LibrosLinkedList()))));

        // 5) Ejemplo con AMBAS pilas: estantería (arreglo) + bodega (nodos).
        private TabPage CrearTabEjemplo() => NuevaPagina("Ejemplo: ambas pilas", new PanelEjemplo());

        // 6) Comparación de las tres pilas numéricas propias de la pestaña.
        private TabPage CrearTabComparacion() => NuevaPagina("Comparación", new PanelComparacion());
    }
}
