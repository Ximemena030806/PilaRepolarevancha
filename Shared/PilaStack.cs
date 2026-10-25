using System;
using System.Collections.Generic;
using System.Linq;

namespace PilaRepo.Shared
{
    /// <summary>
    /// Pila NATIVA genérica: envuelve System.Collections.Generic.Stack&lt;T&gt; para guardar
    /// cualquier tipo (aquí se usa con Libro). Es la versión genérica de PilaNativa (que guarda int).
    ///  - Nunca hay Overflow (el arreglo interno crece solo).
    ///  - Push O(1) amortizado, Pop/Peek O(1), Count O(1).
    ///  - Stack&lt;T&gt; enumera de la cima a la base; por eso ObtenerCopia invierte el resultado.
    /// </summary>
    internal class PilaStack<T>
    {
        private const string MsgVacia = "La pila está vacía.";

        private readonly Stack<T> _stack = new Stack<T>();

        public int Count() => _stack.Count;

        public void Push(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _stack.Push(item);
        }

        public T Pop()
        {
            if (_stack.Count == 0) throw new InvalidOperationException(MsgVacia);
            return _stack.Pop();
        }

        public T Peek()
        {
            if (_stack.Count == 0) throw new InvalidOperationException(MsgVacia);
            return _stack.Peek();
        }

        public void Clear() => _stack.Clear();

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

        /// <summary>Copia base -> tope.</summary>
        public List<T> ObtenerCopia()
        {
            List<T> copia = _stack.ToList();   // viene tope -> base
            copia.Reverse();
            return copia;
        }
    }
}
