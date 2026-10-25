using System;
using System.Collections.Generic;
using System.Linq;

namespace stack_
{
    /// <summary>
    /// Pila NATIVA: usa System.Collections.Generic.Stack&lt;int&gt; en vez de nuestro arreglo
    /// (estática) o nuestros nodos (dinámica). Tiene la misma superficie que la clase stack_
    /// para poder compararlas lado a lado.
    ///
    /// DIFERENCIAS CLAVE frente a stack_ (propia):
    ///  - Almacenamiento: Stack&lt;T&gt; guarda un arreglo interno que CRECE solo (duplica su
    ///    tamaño al llenarse y copia todo); la estática propia es de tamaño fijo y la dinámica
    ///    usa un objeto Node por elemento enlazado con Prev.
    ///  - Push: O(1) AMORTIZADO (de vez en cuando hay una copia O(n) al redimensionar); nunca
    ///    hay Overflow. En la estática propia es O(1) estricto pero se desborda; en la dinámica
    ///    es O(1) estricto.
    ///  - Count: O(1), Stack&lt;T&gt; mantiene su propio contador. La dinámica propia recorre
    ///    los nodos (O(n)); la estática calcula top + 1 (O(1)).
    ///  - Memoria: sin un objeto por elemento ni el enlace Prev, pero el arreglo interno puede
    ///    tener capacidad de sobra.
    ///  - Mantenimiento: es una clase del framework, ya probada y optimizada; las propias hay
    ///    que programarlas y mantenerlas a mano.
    ///  - Stack&lt;T&gt; guarda solo el VALOR (int), no objetos Node. Por eso Pop y Peek crean aquí
    ///    un Node NUEVO como objeto de retorno (no existía dentro de la pila); en stack_ se
    ///    devuelve el mismo Node que entró con Push.
    ///
    /// No existe un IsEmpty() público: Pop/Peek revisan "Count == 0" internamente.
    /// </summary>
    internal class PilaNativa : IPilaEnteros
    {
        private readonly Stack<int> nativa = new Stack<int>();

        public bool EsEstatica => false;
        public int Capacidad => 0;
        public int TopIndex => nativa.Count - 1;

        public void Push(Node nodo)
        {
            nativa.Push(nodo.Value);             // Stack<T> guarda solo el valor
        }

        public Node Pop()
        {
            if (nativa.Count == 0)
                throw new InvalidOperationException("La pila está vacía.");

            return new Node(nativa.Pop());       // Node NUEVO como objeto de retorno
        }

        public Node Peek()
        {
            if (nativa.Count == 0)
                throw new InvalidOperationException("La pila está vacía.");

            return new Node(nativa.Peek());      // Node NUEVO como objeto de retorno
        }

        public void Clear()
        {
            nativa.Clear();                      // vacía, pero conserva la capacidad ya reservada
        }

        public int Count()
        {
            return nativa.Count;                 // O(1): contador interno del framework
        }

        /// <summary>
        /// Igual que en stack_: recorre una COPIA con el cursor "current", de la cima a la base.
        /// (Stack&lt;T&gt; trae su propio Contains; aquí se usa el método de la presentación para
        /// que las tres pilas se comporten igual.)
        /// </summary>
        public bool Contains(int valor)
        {
            List<int> copia = ToList();          // copia base -> tope
            int current = copia.Count - 1;
            while (current >= 0)
            {
                if (copia[current] == valor)
                    return true;
                current--;
            }
            return false;
        }

        /// <summary>Copia base -> tope. Stack&lt;T&gt; enumera de la cima a la base, por eso se invierte.</summary>
        public List<int> ToList()
        {
            List<int> copia = nativa.ToList();
            copia.Reverse();
            return copia;
        }

        // Mismo formato que el ToString de stack_ (tope primero).
        public override string ToString()
        {
            string result = "Modo: Nativo (Stack<T>)\n";
            if (nativa.Count == 0) return result + "Pila vacía.";

            foreach (int v in nativa)            // Stack<T> enumera de la cima a la base
                result += $"[{v}]\n";
            return result;
        }
    }
}
