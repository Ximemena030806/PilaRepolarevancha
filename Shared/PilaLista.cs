using System;
using System.Collections.Generic;
using System.Linq;

namespace PilaRepo.Shared
{
    /// <summary>
    /// Pila genérica respaldada por List&lt;T&gt; usada COMO pila (en este proyecto se usa con Libro):
    ///   Push = Add (al final), Pop/Peek = último índice, Pop = RemoveAt(último).
    /// El tope es siempre el último elemento. No existe IsEmpty: Pop/Peek revisan "Count == 0".
    /// Nunca hay Overflow (la lista crece sola). Con T = Libro es la "pila de libros (List&lt;T&gt;)".
    /// </summary>
    internal class PilaLista<T>
    {
        private const string MsgUnderflow = "La pila está vacía.";

        private readonly List<T> _lista = new List<T>();

        /// <summary>Número de elementos (MÉTODO, como en stack_).</summary>
        public int Count() => _lista.Count;

        /// <summary>Apila al final de la lista. Amortizado O(1).</summary>
        public void Push(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _lista.Add(item);
        }

        /// <summary>Retira y devuelve el último elemento (O(1), sin desplazar nada).</summary>
        public T Pop()
        {
            if (Count() == 0) throw new InvalidOperationException(MsgUnderflow);
            T tope = _lista[_lista.Count - 1];
            _lista.RemoveAt(_lista.Count - 1);
            return tope;
        }

        /// <summary>Devuelve el último elemento sin retirarlo. O(1).</summary>
        public T Peek()
        {
            if (Count() == 0) throw new InvalidOperationException(MsgUnderflow);
            return _lista[_lista.Count - 1];
        }

        /// <summary>Vacía la pila. O(n).</summary>
        public void Clear() => _lista.Clear();

        /// <summary>
        /// Busca sobre una COPIA (ToList) con el cursor "current", de la cima hacia la base.
        /// Nunca modifica la pila original. La igualdad la define T (Libro: por título).
        /// </summary>
        public bool Contains(T item)
        {
            List<T> copia = _lista.ToList();
            int current = copia.Count - 1;           // cursor: empieza en la cima
            while (current >= 0)
            {
                if (EqualityComparer<T>.Default.Equals(copia[current], item))
                    return true;
                current--;                           // baja hacia la base
            }
            return false;
        }

        /// <summary>Copia base -> tope (el último es el tope).</summary>
        public List<T> ObtenerCopia() => _lista.ToList();
    }
}
