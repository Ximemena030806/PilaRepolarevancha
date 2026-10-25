using System;

namespace PilaRepo.Shared
{
    /// <summary>Libro de la pila dinámica de libros (PilaLibros). Igualdad definida por Titulo.</summary>
    internal class Libro : IEquatable<Libro>
    {
        public string Titulo { get; }
        public string Autor { get; }

        public Libro(string titulo, string autor)
        {
            Titulo = (titulo ?? string.Empty).Trim();
            Autor = (autor ?? string.Empty).Trim();
        }

        // Equals / GetHashCode SOLO por Titulo (sin distinguir mayúsculas).
        public bool Equals(Libro? other) =>
            other is not null && string.Equals(Titulo, other.Titulo, StringComparison.OrdinalIgnoreCase);

        public override bool Equals(object? obj) => Equals(obj as Libro);

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Titulo);

        public override string ToString() => $"{Titulo} / {Autor}";
    }
}
