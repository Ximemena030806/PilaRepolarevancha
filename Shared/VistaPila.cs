using System;
using System.Collections.Generic;

namespace PilaRepo.Shared
{
    /// <summary>
    /// Lo que la interfaz (WinForms y consola) necesita de una pila, sin importar si es la
    /// estática, la dinámica, la nativa o la de libros. Los adaptadores de abajo solo
    /// traducen hacia las clases reales (stack_, PilaNativa, PilaLibros) sin cambiar su lógica.
    /// </summary>
    internal interface IPilaVista<T>
    {
        void Push(T item);
        T Pop();
        T Peek();
        int Count();
        bool Contains(T item);
        void Clear();

        /// <summary>Copia de los elementos en orden base -> tope (el último es el tope).</summary>
        List<T> ObtenerCopia();

        /// <summary>true solo en la pila de arreglo de tamaño fijo.</summary>
        bool EsEstatica { get; }

        /// <summary>Tamaño fijo del arreglo (estática); 0 en las demás.</summary>
        int Capacidad { get; }

        /// <summary>Índice del tope (estática: topIndex, -1 si está vacía; demás: Count - 1).</summary>
        int Top { get; }
    }

    /// <summary>Vista de las pilas numéricas: stack_ (estática/dinámica) y PilaNativa.</summary>
    internal sealed class VistaEnteros : IPilaVista<int>
    {
        private readonly stack_.IPilaEnteros _pila;

        public VistaEnteros(stack_.IPilaEnteros pila)
        {
            _pila = pila;
        }

        // Push crea el Node y se lo manda a la pila, igual que el Program.cs original de consola.
        public void Push(int item) => _pila.Push(new stack_.Node(item));
        public int Pop() => _pila.Pop().Value;
        public int Peek() => _pila.Peek().Value;
        public int Count() => _pila.Count();
        public bool Contains(int item) => _pila.Contains(item);
        public void Clear() => _pila.Clear();
        public List<int> ObtenerCopia() => _pila.ToList();
        public bool EsEstatica => _pila.EsEstatica;
        public int Capacidad => _pila.Capacidad;
        public int Top => _pila.TopIndex;
    }

    /// <summary>Vista de la pila dinámica de libros.</summary>
    internal sealed class VistaLibros : IPilaVista<Libro>
    {
        private readonly PilaLibros _pila;

        public VistaLibros(PilaLibros pila)
        {
            _pila = pila;
        }

        public void Push(Libro item) => _pila.Push(item);
        public Libro Pop() => _pila.Pop();
        public Libro Peek() => _pila.Peek();
        public int Count() => _pila.Count();
        public bool Contains(Libro item) => _pila.Contains(item);
        public void Clear() => _pila.Clear();
        public List<Libro> ObtenerCopia() => _pila.ToList();
        public bool EsEstatica => false;
        public int Capacidad => 0;
        public int Top => _pila.Count() - 1;         // solo informativo: en la dinámica el tope es una referencia
    }

    /// <summary>Vista de la pila respaldada por List&lt;T&gt; (libros o enteros).</summary>
    internal sealed class VistaLista<T> : IPilaVista<T>
    {
        private readonly PilaLista<T> _pila;

        public VistaLista(PilaLista<T> pila)
        {
            _pila = pila;
        }

        public void Push(T item) => _pila.Push(item);
        public T Pop() => _pila.Pop();
        public T Peek() => _pila.Peek();
        public int Count() => _pila.Count();
        public bool Contains(T item) => _pila.Contains(item);
        public void Clear() => _pila.Clear();
        public List<T> ObtenerCopia() => _pila.ObtenerCopia();
        public bool EsEstatica => false;
        public int Capacidad => 0;
        public int Top => _pila.Count() - 1;
    }

    /// <summary>Vista de la pila ESTÁTICA genérica (arreglo T[] de tamaño fijo).</summary>
    internal sealed class VistaArreglo<T> : IPilaVista<T> where T : class
    {
        private readonly PilaArreglo<T> _pila;

        public VistaArreglo(PilaArreglo<T> pila)
        {
            _pila = pila;
        }

        public void Push(T item) => _pila.Push(item);
        public T Pop() => _pila.Pop();
        public T Peek() => _pila.Peek();
        public int Count() => _pila.Count();
        public bool Contains(T item) => _pila.Contains(item);
        public void Clear() => _pila.Clear();
        public List<T> ObtenerCopia() => _pila.ObtenerCopia();
        public bool EsEstatica => true;
        public int Capacidad => _pila.Capacidad;
        public int Top => _pila.TopIndex;
    }

    /// <summary>Vista de la pila NATIVA SIN ARREGLOS (LinkedList&lt;T&gt;).</summary>
    internal sealed class VistaLinkedList<T> : IPilaVista<T>
    {
        private readonly PilaLinkedList<T> _pila;

        public VistaLinkedList(PilaLinkedList<T> pila)
        {
            _pila = pila;
        }

        public void Push(T item) => _pila.Push(item);
        public T Pop() => _pila.Pop();
        public T Peek() => _pila.Peek();
        public int Count() => _pila.Count();
        public bool Contains(T item) => _pila.Contains(item);
        public void Clear() => _pila.Clear();
        public List<T> ObtenerCopia() => _pila.ObtenerCopia();
        public bool EsEstatica => false;
        public int Capacidad => 0;
        public int Top => _pila.Count() - 1;
    }

    /// <summary>Vista de la pila NATIVA genérica (Stack&lt;T&gt;).</summary>
    internal sealed class VistaStack<T> : IPilaVista<T>
    {
        private readonly PilaStack<T> _pila;

        public VistaStack(PilaStack<T> pila)
        {
            _pila = pila;
        }

        public void Push(T item) => _pila.Push(item);
        public T Pop() => _pila.Pop();
        public T Peek() => _pila.Peek();
        public int Count() => _pila.Count();
        public bool Contains(T item) => _pila.Contains(item);
        public void Clear() => _pila.Clear();
        public List<T> ObtenerCopia() => _pila.ObtenerCopia();
        public bool EsEstatica => false;
        public int Capacidad => 0;
        public int Top => _pila.Count() - 1;
    }

    /// <summary>
    /// Punto único donde se crean las pilas. Aquí (y solo aquí) se nombra la clase stack_ del
    /// semestre pasado, que vive en el namespace stack_ (se escribe stack_.stack_).
    /// </summary>
    internal static class FabricaPilas
    {
        /// <summary>Pila ESTÁTICA (arreglo de tamaño fijo) = constructor stack_(tamaño).</summary>
        public static IPilaVista<int> Estatica(int tamaño) => new VistaEnteros(new stack_.stack_(tamaño));

        /// <summary>Pila DINÁMICA (nodos con Prev, sin límite) = constructor stack_().</summary>
        public static IPilaVista<int> Dinamica() => new VistaEnteros(new stack_.stack_());

        /// <summary>Pila NATIVA respaldada por Stack&lt;T&gt;.</summary>
        public static IPilaVista<int> Nativa() => new VistaEnteros(new stack_.PilaNativa());

        /// <summary>Pila dinámica de LIBROS, precargada con 3 libros.</summary>
        public static IPilaVista<Libro> LibrosPrecargada() => new VistaLibros(PilaLibros.CrearPrecargada());

        /// <summary>Pila de enteros respaldada por List&lt;int&gt; (solo para comparar).</summary>
        public static IPilaVista<int> ListaEnteros() => new VistaLista<int>(new PilaLista<int>());

        /// <summary>Pila de LIBROS sobre List&lt;T&gt;, precargada con los mismos 3 libros.</summary>
        public static IPilaVista<Libro> LibrosListaPrecargada()
        {
            var pila = new PilaLista<Libro>();
            pila.Push(new Libro("El Principito", "A. de Saint-Exupéry"));
            pila.Push(new Libro("1984", "George Orwell"));
            pila.Push(new Libro("Cien Años de Soledad", "G. García Márquez"));
            return new VistaLista<Libro>(pila);
        }

        // ------------------------------------------------------------------
        // LIBROS en 5 implementaciones, todas PRECARGADAS con los mismos 3 libros
        // (el ÚLTIMO es el tope). Ya existían arriba: LibrosPrecargada (nodos propios)
        // y LibrosListaPrecargada (List<T>).
        //
        //   CON arreglo:  LibrosEstatica (T[] propio) · LibrosNativa (Stack<T>) · LibrosListaPrecargada (List<T>)
        //   SIN arreglo:  LibrosPrecargada (nodos propios) · LibrosLinkedList (LinkedList<T>)
        // ------------------------------------------------------------------

        private static Libro[] LibrosEjemplo() => new[]
        {
            new Libro("El Principito", "A. de Saint-Exupéry"),
            new Libro("1984", "George Orwell"),
            new Libro("Cien Años de Soledad", "G. García Márquez")
        };

        private static IPilaVista<Libro> Cargar(IPilaVista<Libro> pila)
        {
            foreach (Libro l in LibrosEjemplo()) pila.Push(l);
            return pila;
        }

        /// <summary>Libros en ARREGLO propio de tamaño fijo (mínimo 3 por los 3 libros precargados).</summary>
        public static IPilaVista<Libro> LibrosEstatica(int tamaño = 5) =>
            Cargar(new VistaArreglo<Libro>(new PilaArreglo<Libro>(tamaño)));

        /// <summary>Libros sobre Stack&lt;T&gt; nativa (arreglo interno que crece solo).</summary>
        public static IPilaVista<Libro> LibrosNativa() =>
            Cargar(new VistaStack<Libro>(new PilaStack<Libro>()));

        /// <summary>Libros sobre LinkedList&lt;T&gt; nativa (sin arreglo: nodos del framework).</summary>
        public static IPilaVista<Libro> LibrosLinkedList() =>
            Cargar(new VistaLinkedList<Libro>(new PilaLinkedList<Libro>()));

        /// <summary>Pila de libros de NODOS propios (PilaLibros) VACÍA (la usa el ejemplo de la bodega).</summary>
        public static IPilaVista<Libro> LibrosNodosVacia() => new VistaLibros(new PilaLibros());
    }
}
