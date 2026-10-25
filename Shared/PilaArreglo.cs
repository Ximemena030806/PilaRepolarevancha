using System;
using System.Collections.Generic;

namespace PilaRepo.Shared
{
    /// <summary>
    /// Pila ESTÁTICA genérica: arreglo T[] de tamaño fijo + índice topIndex (igual que la parte
    /// estática de stack_, pero guardando cualquier tipo; aquí se usa con Libro).
    /// topIndex empieza en -1; con N elementos vale N-1; Count() = topIndex + 1.
    /// Push en una pila llena lanza Overflow; Pop/Peek en una pila vacía lanzan Underflow.
    /// </summary>
    internal class PilaArreglo<T> where T : class
    {
        private readonly T?[] _array;
        private int _topIndex = -1;

        public PilaArreglo(int tamaño)
        {
            if (tamaño <= 0) throw new ArgumentException("El tamaño debe ser positivo.");
            _array = new T?[tamaño];
        }

        public int Capacidad => _array.Length;
        public int TopIndex => _topIndex;

        /// <summary>O(1): top + 1.</summary>
        public int Count() => _topIndex + 1;

        public void Push(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (_topIndex == _array.Length - 1)
                throw new InvalidOperationException("La pila estática está llena (Overflow).");

            _topIndex++;
            _array[_topIndex] = item;
        }

        public T Pop()
        {
            if (_topIndex == -1)
                throw new InvalidOperationException("La pila está vacía.");

            T val = _array[_topIndex]!;
            _array[_topIndex] = null;     // suelta la referencia
            _topIndex--;
            return val;
        }

        public T Peek()
        {
            if (_topIndex == -1)
                throw new InvalidOperationException("La pila está vacía.");

            return _array[_topIndex]!;
        }

        public void Clear()
        {
            Array.Clear(_array, 0, _array.Length);
            _topIndex = -1;
        }

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

        /// <summary>Copia base -> tope (casillas 0..topIndex).</summary>
        public List<T> ObtenerCopia()
        {
            var copia = new List<T>();
            for (int i = 0; i <= _topIndex; i++)
                copia.Add(_array[i]!);
            return copia;
        }
    }
}
