# PilaRepo — Pila (Stack) en C# / .NET 8

Repositorio unificado:

* **Núcleo = el código del equipo (zip1, hecho con las diapositivas):** `Node` y `stack_`
  (pila estática con arreglo + pila dinámica con nodos). **No se tocó.**
* **Extras = lo del zip2:** pila nativa sobre `Stack<T>`, `Contains`, pila de libros,
  pestaña de comparación, interfaz WinForms y consola con menús.

```
PilaRepo/
├── PilaRepo.sln
├── Shared/                       (una sola copia, la usan los dos proyectos)
│   ├── Node.cs                   zip1 — IDÉNTICO byte a byte
│   ├── Stack's.cs                zip1 — clase stack_ (única diferencia: la palabra "partial")
│   ├── stack_.Extras.cs          agrega a stack_: Contains, ToList, EsEstatica, Capacidad, TopIndex
│   ├── IPilaEnteros.cs           superficie común de las pilas numéricas
│   ├── PilaNativa.cs             pila sobre Stack<T> (misma superficie que stack_)
│   ├── PilaArreglo.cs            NUEVO: pila ESTÁTICA propia (T[] + topIndex), usada con Libro
│   ├── PilaStack.cs              NUEVO: pila sobre Stack<T> (arreglo interno), usada con Libro
│   ├── PilaLista.cs              pila sobre List<T> (arreglo interno), usada con Libro
│   ├── PilaLinkedList.cs         NUEVO: pila sobre LinkedList<T> (SIN arreglo), usada con Libro
│   ├── Libro.cs / PilaLibros.cs  pila dinámica de libros (nodos con Prev), precargada
│   ├── EjemploBiblioteca.cs      NUEVO: ejemplo con AMBAS pilas (estantería de arreglo + bodega de nodos)
│   └── VistaPila.cs              IPilaVista<T>, adaptadores y FabricaPilas
├── PilaWinForms_Tabs/            Windows Forms (net8.0-windows), TabControl de 6 pestañas (Libros con 5 sub-pestañas)
└── PilaConsoleApp/               Consola (net8.0), dos menús
```

## Pilas de LIBROS: con arreglo y sin arreglo (una sola clase de datos: `Libro`)

| | Propia | Nativa (del framework) |
|---|---|---|
| **CON arreglo** | `PilaArreglo<Libro>` — `T[]` de tamaño fijo + `topIndex` (Overflow al llenarse) | `PilaStack<Libro>` sobre `Stack<T>` y `PilaLista<Libro>` sobre `List<T>` (los dos guardan un arreglo interno que crece solo) |
| **SIN arreglo** | `PilaLibros` — nodos enlazados con `Prev` y `top` | `PilaLinkedList<Libro>` sobre `LinkedList<T>` (nodos del framework) |

Las 5 arrancan **precargadas con los mismos 3 libros** (El Principito, 1984, Cien Años de Soledad = tope) y
tienen la misma lógica `Push / Pop / Peek / Count / Clear / Contains` (el `Contains` usa el cursor `current`
sobre una copia, como en las diapositivas). La de arreglo pide tamaño (**mínimo 3**) y lanza
`La pila estática está llena (Overflow).`; las otras cuatro nunca se desbordan.

### Ejemplo con ambas pilas (`EjemploBiblioteca`)

* **Estantería** = pila CON arreglo (tamaño 4, arranca con 3 libros).
* **Bodega** = pila SIN arreglo (nodos, arranca vacía).
* **Apilar libro**: va a la estantería; si está llena (Overflow del arreglo) se va a la bodega, que nunca se llena.
* **Estantería → Bodega** y **Bodega → Estantería**: Pop de una pila + Push en la otra.

Dónde verlo:
* **WinForms**: pestaña *Pila de Libros* (5 sub-pestañas: Arreglo propio, `Stack<T>`, `List<T>`, Nodos propios,
  `LinkedList<T>`) y pestaña *Ejemplo: ambas pilas* (las dos pilas lado a lado).
* **Consola**: Menú 1 → opción 4 (Libros, submenú de 5 formas) y opción 5 (Ejemplo con ambas pilas).

## Qué NO se modificó del zip1 (como pediste)

| Archivo | Estado |
|---|---|
| `Node.cs` (`Value`, `Prev`, `ToString`) | Idéntico byte a byte |
| `Stack's.cs` (clase `stack_`) | Todos los métodos intactos: constructores `stack_()` / `stack_(tamaño)`, `Push`, `Pop`, `Peek`, `IsEmpty`, `Clear`, `Count()`, `ToString` |

La **única** edición es `internal class stack_` → `internal partial class stack_`, para poder
agregar `Contains` y `ToList` en otro archivo (`stack_.Extras.cs`) sin escribir dentro del original.

Se conservan los mensajes del zip1: `La pila estática está llena (Overflow).` y `La pila está vacía.`

## Qué se agregó del zip2

* **`Contains`** como en las diapositivas: copia la pila con `ToList()` y recorre la copia con el
  cursor `current` de la cima a la base. Nunca toca la pila original ni su `top`.
* **Pila nativa** (`PilaNativa`): `Stack<int>` con la misma superficie que `stack_`; los comentarios
  de cada método explican las diferencias de operación y de memoria frente a la estática y la dinámica.
* **Pila de libros** (`PilaLibros`): pila dinámica (`top` + `Prev`) donde cada nodo guarda un `Libro`;
  arranca con 3 libros (El Principito, 1984, Cien Años de Soledad = tope).
* **WinForms**: 6 pestañas — Estática, Dinámica, Nativa, Pila de Libros (5 sub-pestañas), Ejemplo: ambas pilas
  y Comparación (ahora con 4 pilas: estática, dinámica, nativa y `List<T>`; Push, Pop, Peek, Contains, Count, Clear y Llenar N, con tiempo por pila). Un botón por operación; cada uno refresca la grilla y muestra la acción hecha (o el error
  en rojo). La estática trae tamaño 4 por defecto (índices 0-3) y muestra `top = N`.
* **Consola**: Menú 1 (Estática / Dinámica / Nativa / Libros [submenú de 5 formas] / Ejemplo con ambas pilas / Salir) y Menú 2
  (Push, Pop, Peek, Count, Clear, Contains, Ver pila, Cambiar de modo, Salir).

## Fidelidad con la presentación

* `Count()` es **método** en `stack_` — coincide con la diapositiva 100 (`public int Count()`).
* `Count` de la dinámica **recorre los nodos** con un cursor desde `top` hasta `null`
  (diapositivas "Listas enlazadas count": count = 0 → 1 → 2 → 3 → 4).
* Estática: `top` empieza en -1; con N elementos `top = N-1`; `Count() = top + 1`.
* Dinámica: el nodo nuevo apunta con `Prev` al tope anterior y pasa a ser el `top`.

## Notas

* `IsEmpty()` sigue existiendo **dentro de `stack_`** porque es código del zip1 y lo usan `Pop`, `Peek`
  y `ToString`; pero ninguna pantalla (WinForms ni consola) lo ofrece como operación.
* La diapositiva 98 muestra `Stack Overflow (la pila está llena)`, mientras `stack_` lanza
  `La pila estática está llena (Overflow).` Para que coincidan hay que cambiar la diapositiva o
  el texto de esa línea de `Stack's.cs`.
* `Node` y `stack_` viven en el namespace `stack_` (como en el zip1). Fuera de ese namespace la clase se
  nombra `stack_.stack_`; por eso toda la creación de pilas pasa por `FabricaPilas` (en `VistaPila.cs`).

## Compilar y ejecutar

```
dotnet build PilaRepo.sln
dotnet run --project PilaConsoleApp
dotnet run --project PilaWinForms_Tabs      # solo en Windows
```
(En Visual Studio 2022: abrir `PilaRepo.sln`.)
