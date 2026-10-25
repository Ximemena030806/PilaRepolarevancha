using System;
using System.Collections.Generic;

namespace PilaRepo.Shared
{
    /// <summary>
    /// Pila NATIVA SIN ARREGLOS: usa System.Collections.Generic.LinkedList&lt;T&gt; (lista doblemente
    /// enlazada del framework: cada elemento es un LinkedListNode, no hay arreglo interno).
    ///   Push = AddLast, Pop = RemoveLast, Peek = Last. El tope es el ÚLTIMO nodo.
    ///  - Push/Pop/Peek/Count son O(1) estrictos (no hay redimensionado como en List&lt;T&gt;).
    ///  - Nunca hay Overflow.
    ///  - Cada elemento gasta un nodo con dos referencias (Next y Prev), más memoria que un arreglo.
    /// </summary>
    internal class PilaLinkedList<T>
    {
        private const string MsgVacia = "La pila está vacía.";

        private readonly LinkedList<T> _lista = new LinkedList<T>();

        public int Count() => _lista.Count;

        public void Push(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _lista.AddLast(item);
        }

        public T Pop()
        {
            if (_lista.Last == null) throw new InvalidOperationException(MsgVacia);
            T tope = _lista.Last.Value;
            _lista.RemoveLast();
            return tope;
        }

        public T Peek()
        {
            if (_lista.Last == null) throw new InvalidOperationException(MsgVacia);
            return _lista.Last.Value;
        }

        public void Clear() => _lista.Clear();

        /// <summary>Busca sobre una COPIA con el cursor "current", de la cima a la base.</summary>
        public bool Contains(T item)
        {
            List<T> copia = ObtenerCopia();
            int current = copia.Count - 1;
            while (current >= 0)
            {
                if (EqualityComparer<T>.Default.Equals(copia[current], item))
                    return true;
                current--;
            }
            return false;
        }

        /// <summary>Copia base -> tope (LinkedList enumera del primero al último).</summary>
        public List<T> ObtenerCopia() => new List<T>(_lista);
    }
}
