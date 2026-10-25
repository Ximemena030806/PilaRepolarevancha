using System;
using System.Collections.Generic;

namespace PilaRepo.Shared
{
    /// <summary>
    /// Nodo de la pila de libros (diapositivas "Listas enlazadas"): guarda el objeto Libro
    /// y el enlace "Prev" al nodo de abajo. En la base de la pila Prev es null.
    /// </summary>
    internal class NodoLibro
    {
        public Libro Libro { get; set; }
        public NodoLibro? Prev { get; set; }

        public NodoLibro(Libro libro)
        {
            Libro = libro;
            Prev = null;
        }

        public override string ToString() => Libro.ToString();
    }

    /// <summary>
    /// Pila DINÁMICA de libros: misma lógica que la pila dinámica de stack_ (top + Prev,
    /// sin límite de tamaño), pero cada nodo guarda un Libro en vez de un número.
    /// Se crea PRECARGADA con 3 libros (CrearPrecargada); el último queda como tope.
    ///
    /// Igual que en stack_, Count() recorre los nodos (O(n)) y no hay un IsEmpty() público:
    /// Pop/Peek revisan directamente "topNode == null".
    /// </summary>
    internal class PilaLibros
    {
        // Mismo mensaje que usa stack_ para la pila vacía.
        private const string MsgVacia = "La pila está vacía.";

        private NodoLibro? topNode;     // el tope: referencia al último nodo agregado
        private int cantidad;           // contador: sube en Push, baja en Pop, se reinicia en Clear

        /// <summary>Pila de las diapositivas: El Principito, 1984 y Cien Años de Soledad (tope).</summary>
        public static PilaLibros CrearPrecargada()
        {
            var pila = new PilaLibros();
            pila.Push(new Libro("El Principito", "A. de Saint-Exupéry"));
            pila.Push(new Libro("1984", "George Orwell"));
            pila.Push(new Libro("Cien Años de Soledad", "G. García Márquez"));
            return pila;
        }

        /// <summary>
        /// Apila: el nodo nuevo apunta con Prev al que antes era el top y pasa a ser el top.
        /// O(1) estricto, sin Overflow (solo el límite de memoria).
        /// </summary>
        public void Push(Libro libro)
        {
            if (libro == null) throw new ArgumentNullException(nameof(libro));

            var nuevo = new NodoLibro(libro);
            nuevo.Prev = topNode;       // "Prev" apunta al nodo que antes era el top
            topNode = nuevo;            // el nuevo nodo ahora es el top
            cantidad++;
        }

        /// <summary>Retira y devuelve el libro del tope: el top baja a Prev (O(1)).</summary>
        public Libro Pop()
        {
            NodoLibro val = topNode ?? throw new InvalidOperationException(MsgVacia);
            topNode = val.Prev;         // bajamos el top al nodo anterior
            cantidad--;
            return val.Libro;
        }

        /// <summary>Devuelve el libro del tope sin retirarlo (O(1)).</summary>
        public Libro Peek()
        {
            NodoLibro top = topNode ?? throw new InvalidOperationException(MsgVacia);
            return top.Libro;
        }

        /// <summary>Vacía la pila: basta con soltar el top; el GC recoge toda la cadena.</summary>
        public void Clear()
        {
            topNode = null;
            cantidad = 0;
        }

        /// <summary>Cuenta los nodos con un cursor que parte del top y baja por Prev (O(n)).</summary>
        public int Count() => cantidad;   // O(1): contador actualizado en Push/Pop/Clear

        /// <summary>
        /// Busca por título (la igualdad de Libro es por Titulo). Recorre una COPIA con el
        /// cursor "current", de la cima a la base; la pila original no se modifica.
        /// </summary>
        public bool Contains(Libro libro)
        {
            List<Libro> copia = ToList();        // copia base -> tope
            int current = copia.Count - 1;       // el cursor empieza en el tope
            while (current >= 0)
            {
                if (EqualityComparer<Libro>.Default.Equals(copia[current], libro))
                    return true;
                current--;
            }
            return false;
        }

        /// <summary>Copia base -> tope (el último de la lista es el tope).</summary>
        public List<Libro> ToList()
        {
            var copia = new List<Libro>();
            for (NodoLibro? t = topNode; t != null; t = t.Prev)
                copia.Add(t.Libro);
            copia.Reverse();
            return copia;
        }
    }
}
