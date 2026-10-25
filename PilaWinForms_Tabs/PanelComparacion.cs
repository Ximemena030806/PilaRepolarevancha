using System;
using System.Drawing;
using System.Windows.Forms;
using PilaRepo.Shared;

namespace PilaWinForms_Tabs
{
    /// <summary>
    /// Pestaña de comparación: CUATRO pilas numéricas PROPIAS de esta pestaña (Estática y Dinámica de la clase
    /// stack_, Nativa sobre Stack&lt;T&gt; y Lista sobre List&lt;T&gt;), lado a lado, cada una con mini-grilla,
    /// Count y Top. Los botones (Push, Pop, Peek, Contains, Count, Clear, Llenar N) aplican la misma
    /// operación a las cuatro y se muestra cuál falló y por qué (Overflow / Underflow), con el tiempo de cada una.
    /// </summary>
    internal class PanelComparacion : TableLayoutPanel, IRefrescable
    {
        private IPilaVista<int> _estatica;
        private IPilaVista<int> _dinamica;
        private IPilaVista<int> _nativa;
        private IPilaVista<int> _lista;

        private readonly NumericUpDown _numValor = new NumericUpDown { Minimum = -100000, Maximum = 100000, Value = 10, Width = 80 };
        private readonly NumericUpDown _numLlenar = new NumericUpDown { Minimum = 1, Maximum = 1000, Value = 6, Width = 60 };
        private readonly NumericUpDown _numTam = new NumericUpDown { Minimum = 1, Maximum = 50, Value = 4, Width = 60 };
        private readonly Label _lblEstado;

        private DataGridView _gridEst = null!, _gridDin = null!, _gridNat = null!, _gridLis = null!;
        private Label _countEst = null!, _countDin = null!, _countNat = null!, _countLis = null!;
        private Label _errEst = null!, _errDin = null!, _errNat = null!, _errLis = null!;

        public PanelComparacion()
        {
            _estatica = FabricaPilas.Estatica(4);
            _dinamica = FabricaPilas.Dinamica();
            _nativa = FabricaPilas.Nativa();
            _lista = FabricaPilas.ListaEnteros();

            Dock = DockStyle.Fill;
            Padding = new Padding(8);
            ColumnCount = 1;
            RowCount = 3;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            RowStyles.Add(new RowStyle(SizeType.AutoSize));          // controles
            RowStyles.Add(new RowStyle(SizeType.Percent, 100));      // las cuatro pilas
            RowStyles.Add(new RowStyle(SizeType.Absolute, 90));      // estado general

            var controles = UiHelper.NuevoFlow();
            controles.Controls.Add(UiHelper.Etiqueta("Valor:"));
            _numValor.Margin = new Padding(3, 5, 3, 3);
            controles.Controls.Add(_numValor);
            controles.Controls.Add(UiHelper.Boton("Push en las 4", (s, e) => OnPush(), 120));
            controles.Controls.Add(UiHelper.Boton("Pop en las 4", (s, e) => OnPop(), 120));
            controles.Controls.Add(UiHelper.Boton("Peek en las 4", (s, e) => OnPeek(), 120));
            controles.Controls.Add(UiHelper.Boton("Contains en las 4", (s, e) => OnContains(), 130));
            controles.Controls.Add(UiHelper.Boton("Count en las 4", (s, e) => OnCount(), 120));
            controles.Controls.Add(UiHelper.Boton("Clear en las 4", (s, e) => OnClear(), 120));
            controles.Controls.Add(UiHelper.Etiqueta("Llenar con N:"));
            _numLlenar.Margin = new Padding(3, 5, 3, 3);
            controles.Controls.Add(_numLlenar);
            controles.Controls.Add(UiHelper.Boton("Llenar N en las 4", (s, e) => OnLlenar(), 130));
            controles.Controls.Add(UiHelper.Etiqueta("Tamaño de la estática:"));
            _numTam.Margin = new Padding(3, 5, 3, 3);
            controles.Controls.Add(_numTam);
            controles.Controls.Add(UiHelper.Boton("Reiniciar las 4", (s, e) => OnReiniciar(), 120));

            var tres = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 1 };
            for (int i = 0; i < 4; i++) tres.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tres.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            tres.Controls.Add(CrearGrupo("Estática (arreglo + top)", out _gridEst, out _countEst, out _errEst), 0, 0);
            tres.Controls.Add(CrearGrupo("Dinámica (nodos + Prev)", out _gridDin, out _countDin, out _errDin), 1, 0);
            tres.Controls.Add(CrearGrupo("Nativa (Stack<T>)", out _gridNat, out _countNat, out _errNat), 2, 0);
            tres.Controls.Add(CrearGrupo("Lista (List<T>)", out _gridLis, out _countLis, out _errLis), 3, 0);

            _lblEstado = new Label { Dock = DockStyle.Fill, AutoSize = false };

            Controls.Add(controles, 0, 0);
            Controls.Add(tres, 0, 1);
            Controls.Add(_lblEstado, 0, 2);

            Refrescar();
        }

        private static GroupBox CrearGrupo(string titulo, out DataGridView grid, out Label lblCount, out Label lblError)
        {
            var grupo = new GroupBox { Text = titulo, Dock = DockStyle.Fill, Padding = new Padding(6) };
            var t = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            t.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            t.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));

            lblCount = new Label { AutoSize = true, Margin = new Padding(3, 3, 3, 3) };
            lblCount.Font = new Font(lblCount.Font, FontStyle.Bold);
            grid = UiHelper.CrearGrid("Valor");
            lblError = new Label { Dock = DockStyle.Fill, AutoSize = false };

            t.Controls.Add(lblCount, 0, 0);
            t.Controls.Add(grid, 0, 1);
            t.Controls.Add(lblError, 0, 2);
            grupo.Controls.Add(t);
            return grupo;
        }

        public void Refrescar()
        {
            Dibujar(_estatica, _gridEst, _countEst, true);
            Dibujar(_dinamica, _gridDin, _countDin, false);
            Dibujar(_nativa, _gridNat, _countNat, false);
            Dibujar(_lista, _gridLis, _countLis, false);
        }

        private static void Dibujar(IPilaVista<int> pila, DataGridView grid, Label lblCount, bool mostrarLibres)
        {
            UiHelper.LlenarGrid(grid, pila.ObtenerCopia(), mostrarLibres ? pila.Capacidad : 0,
                                v => new[] { v.ToString() }, 1);
            lblCount.Text = mostrarLibres
                ? $"top = {pila.Top}   (tamaño = {pila.Capacidad})"
                : $"top = {pila.Top}";
        }

        /// <summary>Aplica la operación a las tres pilas y reporta cuál falló y por qué.</summary>
        private void EnLasTres(string operacion, Func<IPilaVista<int>, string> accion)
        {
            var objetivos = new (string Nombre, IPilaVista<int> Pila, Label Err)[]
            {
                ("Estática", _estatica, _errEst),
                ("Dinámica", _dinamica, _errDin),
                ("Nativa", _nativa, _errNat),
                ("Lista", _lista, _errLis)
            };

            var partes = new System.Collections.Generic.List<string>();
            bool hayError = false;

            foreach (var o in objetivos)
            {
                var cron = System.Diagnostics.Stopwatch.StartNew();
                try
                {
                    string r = accion(o.Pila);
                    cron.Stop();
                    string t = $"{cron.Elapsed.TotalMilliseconds:0.000} ms";
                    o.Err.ForeColor = Color.DarkGreen;
                    o.Err.Text = $"OK: {r} [{t}]";
                    partes.Add($"{o.Nombre}: OK ({r}, {t})");
                }
                catch (InvalidOperationException ex)       // Overflow / Underflow
                {
                    cron.Stop();
                    hayError = true;
                    o.Err.ForeColor = Color.Red;
                    o.Err.Text = "FALLÓ: " + ex.Message;
                    partes.Add($"{o.Nombre}: FALLÓ -> {ex.Message}");
                }
            }

            Refrescar();
            UiHelper.MostrarEstado(_lblEstado, $"{operacion} en las 4:  " + string.Join("   |   ", partes), hayError);
        }

        private void OnPush()
        {
            int v = (int)_numValor.Value;
            EnLasTres("Push(" + v + ")", p => { p.Push(v); return $"Push({v})"; });
            if (_numValor.Value < _numValor.Maximum) _numValor.Value += 1;   // siguiente valor para la demo
        }

        private void OnPeek() => EnLasTres("Peek()", p => $"Peek() = {p.Peek()}");

        private void OnContains()
        {
            int v = (int)_numValor.Value;
            EnLasTres("Contains(" + v + ")", p => $"Contains({v}) = {p.Contains(v)}");
        }

        private void OnCount() => EnLasTres("Count", p => $"Count = {p.Count()}");

        /// <summary>Hace N Push seguidos en cada pila: la estática se desborda al llegar a su tamaño.</summary>
        private void OnLlenar()
        {
            int n = (int)_numLlenar.Value;
            int inicio = (int)_numValor.Value;
            EnLasTres($"Llenar {n}", p =>
            {
                for (int i = 0; i < n; i++) p.Push(inicio + i);
                return $"{n} Push";
            });
        }

        private void OnPop() => EnLasTres("Pop()", p => $"Pop() = {p.Pop()}");

        private void OnClear() => EnLasTres("Clear()", p => { p.Clear(); return "Clear()"; });

        private void OnReiniciar()
        {
            _estatica = FabricaPilas.Estatica((int)_numTam.Value);
            _dinamica = FabricaPilas.Dinamica();
            _nativa = FabricaPilas.Nativa();
            _lista = FabricaPilas.ListaEnteros();
            _errEst.Text = _errDin.Text = _errNat.Text = _errLis.Text = "";
            Refrescar();
            UiHelper.MostrarEstado(_lblEstado, $"Las 4 pilas se reiniciaron (estática de tamaño {_estatica.Capacidad}).", false);
        }
    }
}
