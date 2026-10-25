using System;

namespace stack_
{
    internal class Node
    {
        public int Value { get; set; }

        // Referencia para stack linked list (apunta al nodo anterior/abajo)
        public Node? Prev { get; set; }

        public Node(int value)
        {
            Value = value;
            Prev = null;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
