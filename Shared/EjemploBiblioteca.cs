using System;

namespace PilaRepo.Shared
{
    /// <summary>
    /// EJEMPLO CON AMBAS PILAS trabajando juntas (solo la clase Libro):
    ///
    ///   ESTANTERÍA = pila CON ARREGLO (PilaArreglo&lt;Libro&gt;, tamaño fijo 4)
    ///   BODEGA     = pila SIN ARREGLO (PilaLibros: nodos enlazados con Prev, sin límite)
    ///
    /// Los libros se apilan en la estantería; cuando se llena (Overflow del arreglo) el libro
    /// se manda a la bodega, que nunca se llena. También se pueden mover libros de una pila
    /// a la otra: Pop de una + Push en la otra.
    /// </summary>
    internal class EjemploBiblioteca
    {
        public const int CapacidadEstanteria = 4;

        /// <summary>Pila de arreglo: arranca con 3 libros (queda 1 espacio libre).</summary>
        public IPilaVista<Libro> Estanteria { get; } = FabricaPilas.LibrosEstatica(CapacidadEstanteria);

        /// <summary>Pila de nodos: arranca vacía.</summary>
        public IPilaVista<Libro> Bodega { get; } = FabricaPilas.LibrosNodosVacia();

        /// <summary>Apila en la estantería; si está llena, el libro se va a la bodega.</summary>
        public string Apilar(Libro libro)
        {
            if (Estanteria.Count() < Estanteria.Capacidad)
            {
                Estanteria.Push(libro);
                return $"Apilado en la ESTANTERÍA (arreglo): {libro}";
            }

            Bodega.Push(libro);
            return $"Estantería llena (Overflow evitado) -> apilado en la BODEGA (nodos): {libro}";
        }

        /// <summary>Pop de la estantería y Push en la bodega.</summary>
        public string MoverEstanteriaABodega()
        {
            Libro l = Estanteria.Pop();          // Underflow si la estantería está vacía
            Bodega.Push(l);
            return $"Movido de la ESTANTERÍA a la BODEGA: {l}";
        }

        /// <summary>Pop de la bodega y Push en la estantería (revisa antes de sacar, así no se pierde el libro).</summary>
        public string MoverBodegaAEstanteria()
        {
            Libro l = Bodega.Peek();             // Underflow si la bodega está vacía
            Estanteria.Push(l);                  // Overflow si la estantería está llena
            Bodega.Pop();
            return $"Movido de la BODEGA a la ESTANTERÍA: {l}";
        }
    }
}
