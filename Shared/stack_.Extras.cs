using System;
using System.Collections.Generic;

namespace stack_
{
    /// <summary>
    /// Segunda mitad de la clase stack_ (Stack's.cs es la primera y NO se modificó, salvo la
    /// palabra "partial"). Aquí solo se AGREGAN miembros; Push, Pop, Peek, IsEmpty, Clear,
    /// Count() y ToString() siguen siendo exactamente los de las diapositivas.
    /// </summary>
    internal partial class stack_ : IPilaEnteros
    {
        // ------------------------------------------------------------------
        // Información de solo lectura (la usa la interfaz para dibujar "top = N")
        // ------------------------------------------------------------------

        /// <summary>true si la pila es de arreglo (constructor con tamaño).</summary>
        public bool EsEstatica => esEstatica;

        /// <summary>Tamaño fijo del arreglo (estática); 0 en la dinámica.</summary>
        public int Capacidad => esEstatica ? tamañoMax : 0;

        /// <summary>
        /// Índice del tope. Estática: topIndex (-1 si está vacía). Dinámica: Count() - 1
        /// (solo informativo: en la dinámica el tope es una referencia, no un índice).
        /// </summary>
        public int TopIndex => esEstatica ? topIndex : Count() - 1;

        // ------------------------------------------------------------------
        // Contains y copia
        // ------------------------------------------------------------------

        /// <summary>
        /// Indica si el valor está en la pila, como en las diapositivas "Contains": un cursor
        /// "current" parte del tope (top) y baja comparando uno por uno hasta encontrarlo o
        /// llegar al final. Se recorre una COPIA (ToList), así la pila original y su top
        /// no se modifican nunca. O(n) en tiempo y O(n) extra de memoria por la copia.
        /// </summary>
        public bool Contains(int valor)
        {
            List<int> copia = ToList();          // copia base -> tope
            int current = copia.Count - 1;       // el cursor empieza en el tope
            while (current >= 0)
            {
                if (copia[current] == valor)     // ¿40 == 20? FALSE ... ¿20 == 20? TRUE
                    return true;
                current--;                       // baja al siguiente
            }
            return false;                        // llegó al final sin encontrarlo
        }

        /// <summary>
        /// Copia de los valores en orden base -> tope (el último de la lista es el tope).
        /// Estática: casillas 0..topIndex del arreglo. Dinámica: se baja por Prev desde topNode
        /// y se invierte el resultado. No modifica la pila.
        /// </summary>
        public List<int> ToList()
        {
            var copia = new List<int>();

            if (esEstatica)
            {
                for (int i = 0; i <= topIndex; i++)
                    copia.Add(array![i].Value);
            }
            else
            {
                for (Node? t = topNode; t != null; t = t.Prev)
                    copia.Add(t.Value);
                copia.Reverse();                 // se recorrió tope -> base; se deja base -> tope
            }
            return copia;
        }
    }
}
