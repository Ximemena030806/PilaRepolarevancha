using System;
using System.Text;
using PilaRepo.Shared;

namespace PilaConsoleApp
{
    /// <summary>Lectura de datos desde la consola.</summary>
    internal static class Entrada
    {
        public static string LeerTexto(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine() ?? string.Empty;
        }

        public static int LeerEntero(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                if (int.TryParse(Console.ReadLine(), out int v)) return v;
                Console.WriteLine("Número inválido, intenta de nuevo.");
            }
        }
    }

    internal static class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            bool salir = false;

            while (!salir)
            {
                IMenuPila? pila = ElegirModo();          // Menú 1
                if (pila == null) return;                // "Salir"

                bool cambiarModo = false;
                while (!cambiarModo && !salir)           // Menú 2
                {
                    MostrarMenuOperaciones(pila.Nombre);
                    string opcion = (Console.ReadLine() ?? "").Trim();
                    try
                    {
                        switch (opcion)
                        {
                            case "1": pila.Push(); break;
                            case "2": pila.Pop(); break;
                            case "3": pila.Peek(); break;
                            case "4": pila.Count(); break;
                            case "5": pila.Clear(); break;
                            case "6": pila.Contains(); break;
                            case "7": pila.Ver(); break;
                            case "8": cambiarModo = true; break;
                            case "9": salir = true; break;
                            default: Console.WriteLine("Opción no válida."); break;
                        }
                    }
                    catch (InvalidOperationException ex)   // Stack Overflow / Stack Underflow
                    {
                        Console.WriteLine($"ERROR: {ex.Message}");
                    }
                }
            }
        }

        // ---------------- Menú 1: elegir modo ----------------
        private static IMenuPila? ElegirModo()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("===== PILA: elige el modo =====");
                Console.WriteLine("1. Estática (arreglo con límite, pide tamaño)");
                Console.WriteLine("2. Dinámica (nodos enlazados, sin límite)");
                Console.WriteLine("3. Nativa (Stack<T>)");
                Console.WriteLine("4. Pila de LIBROS (elegir una de 5 implementaciones, precargada)");
                Console.WriteLine("5. EJEMPLO con ambas pilas (arreglo + nodos)");
                Console.WriteLine("6. Salir");
                Console.Write("Opción: ");

                switch ((Console.ReadLine() ?? "").Trim())
                {
                    case "1":
                        int tam = Entrada.LeerEntero("Tamaño de la pila (por defecto en las diapositivas: 4): ");
                        if (tam <= 0) { Console.WriteLine("El tamaño debe ser mayor que cero."); break; }
                        return NuevaPilaEnteros($"Pila Estática (tamaño {tam})", FabricaPilas.Estatica(tam));
                    case "2":
                        return NuevaPilaEnteros("Pila Dinámica (nodos)", FabricaPilas.Dinamica());
                    case "3":
                        return NuevaPilaEnteros("Pila Nativa (Stack<T>)", FabricaPilas.Nativa());
                    case "4":
                    {
                        IMenuPila? libros = ElegirLibros();         // Submenú: 5 implementaciones
                        if (libros != null) return libros;
                        break;
                    }
                    case "5":
                    {
                        EjemploAmbasPilas();                        // usa la pila de arreglo y la de nodos juntas
                        break;
                    }
                    case "6":
                        return null;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
        }

        // ---------------- Submenú de libros: 5 implementaciones ----------------

        private static IMenuPila? ElegirLibros()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("===== LIBROS: elige la implementación =====");
                Console.WriteLine("-- CON arreglo --");
                Console.WriteLine("1. Arreglo propio (T[], tamaño fijo)");
                Console.WriteLine("2. Stack<T> (nativa)");
                Console.WriteLine("3. List<T> (lista nativa)");
                Console.WriteLine("-- SIN arreglo --");
                Console.WriteLine("4. Nodos propios (lista enlazada con Prev)");
                Console.WriteLine("5. LinkedList<T> (lista enlazada nativa)");
                Console.WriteLine("6. Volver");
                Console.Write("Opción: ");

                IPilaVista<Libro> pila;
                string nombre;
                switch ((Console.ReadLine() ?? "").Trim())
                {
                    case "1":
                        int tam = Entrada.LeerEntero("Tamaño del arreglo (mínimo 3, ya trae 3 libros precargados): ");
                        if (tam < 3) { Console.WriteLine("El tamaño debe ser al menos 3."); continue; }
                        pila = FabricaPilas.LibrosEstatica(tam);
                        nombre = $"Libros: arreglo propio T[] (tamaño {tam})";
                        break;
                    case "2":
                        pila = FabricaPilas.LibrosNativa();
                        nombre = "Libros: Stack<T> nativa (arreglo interno)";
                        break;
                    case "3":
                        pila = FabricaPilas.LibrosListaPrecargada();
                        nombre = "Libros: List<T> (arreglo interno)";
                        break;
                    case "4":
                        pila = FabricaPilas.LibrosPrecargada();
                        nombre = "Libros: nodos propios (sin arreglo)";
                        break;
                    case "5":
                        pila = FabricaPilas.LibrosLinkedList();
                        nombre = "Libros: LinkedList<T> (sin arreglo)";
                        break;
                    case "6":
                        return null;
                    default:
                        Console.WriteLine("Opción no válida.");
                        continue;
                }

                return new AdaptadorPila<Libro>(nombre, pila,
                    () => new Libro(Entrada.LeerTexto("Título: "), Entrada.LeerTexto("Autor: ")),
                    () => new Libro(Entrada.LeerTexto("Título a buscar: "), ""),
                    l => $"{l.Titulo} / {l.Autor}");
            }
        }

        // ---------------- Ejemplo con ambas pilas ----------------

        /// <summary>
        /// Estantería (pila de ARREGLO, tamaño 4) + Bodega (pila de NODOS, sin límite).
        /// La lógica vive en Shared/EjemploBiblioteca.cs; aquí solo se muestra el menú.
        /// </summary>
        private static void EjemploAmbasPilas()
        {
            var ejemplo = new EjemploBiblioteca();
            var estanteria = new AdaptadorPila<Libro>($"ESTANTERÍA (arreglo, tamaño {EjemploBiblioteca.CapacidadEstanteria})",
                ejemplo.Estanteria, () => new Libro("", ""), () => new Libro("", ""), l => $"{l.Titulo} / {l.Autor}");
            var bodega = new AdaptadorPila<Libro>("BODEGA (nodos, sin límite)",
                ejemplo.Bodega, () => new Libro("", ""), () => new Libro("", ""), l => $"{l.Titulo} / {l.Autor}");

            bool volver = false;
            while (!volver)
            {
                Console.WriteLine();
                Console.WriteLine("===== EJEMPLO: estantería (arreglo) + bodega (nodos) =====");
                Console.WriteLine("1. Apilar libro (si la estantería está llena, va a la bodega)");
                Console.WriteLine("2. Mover tope: estantería -> bodega");
                Console.WriteLine("3. Mover tope: bodega -> estantería");
                Console.WriteLine("4. Ver las dos pilas");
                Console.WriteLine("5. Volver");
                Console.Write("Opción: ");

                try
                {
                    switch ((Console.ReadLine() ?? "").Trim())
                    {
                        case "1":
                            var libro = new Libro(Entrada.LeerTexto("Título: "), Entrada.LeerTexto("Autor: "));
                            Console.WriteLine(ejemplo.Apilar(libro));
                            break;
                        case "2": Console.WriteLine(ejemplo.MoverEstanteriaABodega()); break;
                        case "3": Console.WriteLine(ejemplo.MoverBodegaAEstanteria()); break;
                        case "4": estanteria.Ver(); bodega.Ver(); break;
                        case "5": volver = true; break;
                        default: Console.WriteLine("Opción no válida."); break;
                    }
                }
                catch (InvalidOperationException ex)   // Overflow / Underflow
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                }
            }
        }

        private static IMenuPila NuevaPilaEnteros(string nombre, IPilaVista<int> pila) =>
            new AdaptadorPila<int>(nombre, pila,
                () => Entrada.LeerEntero("Valor entero: "),
                () => Entrada.LeerEntero("Valor entero a buscar: "),
                v => v.ToString());

        // ---------------- Menú 2: operaciones ----------------
        private static void MostrarMenuOperaciones(string nombre)
        {
            Console.WriteLine();
            Console.WriteLine($"===== {nombre} =====");
            Console.WriteLine("1. Push");
            Console.WriteLine("2. Pop");
            Console.WriteLine("3. Peek");
            Console.WriteLine("4. Count");
            Console.WriteLine("5. Clear");
            Console.WriteLine("6. Contains");
            Console.WriteLine("7. Ver pila completa");
            Console.WriteLine("8. Cambiar de modo");
            Console.WriteLine("9. Salir");
            Console.Write("Opción: ");
        }
    }
}
