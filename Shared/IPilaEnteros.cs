using System;
using System.Collections.Generic;

namespace stack_
{
    /// <summary>
    /// Superficie común de las pilas numéricas: la ESTÁTICA y la DINÁMICA (clase stack_ del
    /// semestre pasado, sin tocar) y la NATIVA (PilaNativa, sobre Stack&lt;T&gt;).
    /// Los métodos Push/Pop/Peek/Clear/Count() son EXACTAMENTE los de stack_ (Count es método,
    /// como en la diapositiva 100). Así la interfaz gráfica y la consola usan las tres pilas
    /// con el mismo código.
    ///
    /// NOTA: IsEmpty() NO forma parte de esta interfaz a propósito: la clase stack_ lo conserva
    /// tal cual (lo usan Pop/Peek/ToString), pero ninguna pantalla lo ofrece como operación.
    /// </summary>
    internal interface IPilaEnteros
    {
        void Push(Node nodo);
        Node Pop();
        Node Peek();
        void Clear();
        int Count();

        /// <summary>¿Existe el valor? Recorre una COPIA con el cursor "current"; no toca la pila.</summary>
        bool Contains(int valor);

        /// <summary>Copia de los valores en orden BASE -> TOPE (el último de la lista es el tope).</summary>
        List<int> ToList();

        /// <summary>true solo en la pila de arreglo de tamaño fijo.</summary>
        bool EsEstatica { get; }

        /// <summary>Tamaño fijo del arreglo (estática); 0 en las demás.</summary>
        int Capacidad { get; }

        /// <summary>Índice del tope: en la estática es topIndex (-1 si está vacía); en las demás Count() - 1.</summary>
        int TopIndex { get; }
    }
}
