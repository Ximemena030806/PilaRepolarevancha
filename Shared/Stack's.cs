using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stack_
{
    internal partial class stack_
    {
        private bool esEstatica; // variable para distinguir tipo de pila

        // variables para lógica dinámica
        private Node? topNode; // Tope para la dinámica

        // variables para lógica estática
        private Node[]? array; // Arreglo para la estática
        private int topIndex;  // Índice para la estática
        private int tamañoMax;
        private int cantidad; // contador: sube en Push, baja en Pop, se reinicia en Clear

        // CONSTRUCTOR 1: Para Pila DINÁMICA (Sin límite)
        public stack_()
        {
            esEstatica = false;
            topNode = null;
        }

        // CONSTRUCTOR 2: Para Pila ESTÁTICA (Con límite)
        public stack_(int tamaño)
        {
            if (tamaño <= 0) throw new ArgumentException("El tamaño debe ser positivo.");

            esEstatica = true;
            tamañoMax = tamaño;
            array = new Node[tamaño];
            topIndex = -1;
        }

        // MÉTODOS  PUSH, POP, PEEK
        public void Push(Node nodo)
        {
            if (esEstatica)
            {
                if (topIndex == tamañoMax - 1)
                    throw new InvalidOperationException("La pila estática está llena (Overflow).");

                topIndex++;
                array![topIndex] = nodo;
            }
            else
            {
                // "Prev" apunta al nodo que antes era el top (hacia abajo)
                nodo.Prev = topNode;
                topNode = nodo;
            }
            cantidad++;
        }

        public Node Pop()
        {
            if (IsEmpty())
                throw new InvalidOperationException("La pila está vacía.");

            Node val;
            if (esEstatica)
            {
                val = array![topIndex];
                array[topIndex] = null!;
                topIndex--;
            }
            else
            {
                val = topNode!;
                topNode = topNode!.Prev;
            }
            cantidad--;
            return val;
        }

        public Node Peek()
        {
            if (IsEmpty())
                throw new InvalidOperationException("La pila está vacía.");

            return esEstatica ? array![topIndex] : topNode!;
        }

        public bool IsEmpty()
        {
            return esEstatica ? topIndex == -1 : topNode == null;
        }

        public void Clear()
        {
            if (esEstatica)
            {
                topIndex = -1;
                Array.Clear(array!, 0, array!.Length);
            }
            else
            {
                topNode = null;
            }
            cantidad = 0;
        }

        // Contador que se actualiza en Push, Pop y Clear (O(1)).
        public int Count() => cantidad;

        // Método ToString para ver el contenido
        public override string ToString()
        {
            string result = "";

            if (esEstatica)
            {
                result += "Modo: Estático (Array)\n";
                if (IsEmpty()) return result + "Pila vacía.";

                for (int i = topIndex; i >= 0; i--)
                {
                    result += $"[{array![i].Value}]\n";
                }
            }
            else
            {
                result += "Modo: Dinámico (Nodos)\n";
                if (IsEmpty()) return result + "Pila vacía.";

                Node? t = topNode;
                while (t != null)
                {
                    result += $"[{t.Value}]\n";
                    t = t.Prev;
                }
            }
            return result;
        }
    }
}