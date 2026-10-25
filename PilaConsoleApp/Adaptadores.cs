using System;
using PilaRepo.Shared;

namespace PilaConsoleApp
{
    /// <summary>Operaciones del Menú 2, iguales para cualquier modo de pila.</summary>
    internal interface IMenuPila
    {
        string Nombre { get; }
        void Push();
        void Pop();
        void Peek();
        void Count();
        void Clear();
        void Contains();
        void Ver();
    }

    /// <summary>Adaptador para cualquier pila vista como IPilaVista&lt;T&gt; (estática, dinámica, nativa o de libros).</summary>
    internal sealed class AdaptadorPila<T> : IMenuPila
    {
        private readonly IPilaVista<T> _pila;
        private readonly Func<T> _leerParaPush;
        private readonly Func<T> _leerParaBuscar;
        private readonly Func<T, string> _formato;

        public string Nombre { get; }

        public AdaptadorPila(string nombre, IPilaVista<T> pila, Func<T> leerParaPush, Func<T> leerParaBuscar, Func<T, string> formato)
        {
            Nombre = nombre;
            _pila = pila;
            _leerParaPush = leerParaPush;
            _leerParaBuscar = leerParaBuscar;
            _formato = formato;
        }

        public void Push()
        {
            T v = _leerParaPush();
            _pila.Push(v);   // en la estática puede lanzar el Overflow (pila llena)
            Console.WriteLine($"Push({_formato(v)}) -> apilado.");
        }

        public void Pop()
        {
            T v = _pila.Pop();   // puede lanzar el error de pila vacía
            Console.WriteLine($"Pop() -> {_formato(v)}.");
        }

        public void Peek()
        {
            T v = _pila.Peek();
            Console.WriteLine($"Peek() -> {_formato(v)} (no se retira)");
        }

        public void Count() => Console.WriteLine($"Count = {_pila.Count()}");

        public void Clear()
        {
            _pila.Clear();
            Console.WriteLine("Clear() -> pila vacía.");
        }

        public void Contains()
        {
            T v = _leerParaBuscar();
            Console.WriteLine($"Contains({_formato(v)}) -> {_pila.Contains(v)}");
        }

        public void Ver()
        {
            var copia = _pila.ObtenerCopia();   // base -> tope
            int n = copia.Count;
            int inicio = Math.Max(_pila.EsEstatica ? _pila.Capacidad : 0, n) - 1;

            Console.WriteLine($"--- {Nombre} | top = {_pila.Top} ---");
            if (inicio < 0) Console.WriteLine("(vacía)");
            for (int i = inicio; i >= 0; i--)
            {
                string marca = (i == n - 1) ? "Tope ->" : "       ";
                string dato = i < n ? _formato(copia[i]) : "(libre)";
                Console.WriteLine($"{marca} [{i}] {dato}");
            }
        }
    }
}
