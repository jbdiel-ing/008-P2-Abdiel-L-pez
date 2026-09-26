using System;
using System.Collections.Generic;
using System.IO;

class Mascota
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Especie { get; set; }
    public decimal Peso { get; set; }

    public override string ToString()
    {
        return "ID: " + Id + " | Nombre: " + Nombre + " | Especie: " + Especie + " | Peso: " + Peso + " kg";
    }
}

class Program
{
    static void Main()
    {
        List<Mascota> mascotas = new List<Mascota>();
        int contadorId = 1;

        CargarMascotas(mascotas, ref contadorId);

        int opcion;

        do
        {
            Console.Clear();
            Console.WriteLine("===== REGISTRO DE MASCOTAS =====");
            Console.WriteLine("1. Agregar mascota");
            Console.WriteLine("2. Listar mascotas");
            Console.WriteLine("3. Buscar por especie");
            Console.WriteLine("4. Salir");
            Console.Write("Seleccione una opcion: ");

            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                opcion = 0;
            }

            switch (opcion)
            {
                case 1:
                    AgregarMascota(mascotas, ref contadorId);
                    break;

                case 2:
                    ListarMascotas(mascotas);
                    break;

                case 3:
                    BuscarPorEspecie(mascotas);
                    break;

                case 4:
                    GuardarMascotas(mascotas);
                    Console.WriteLine("Mascotas guardadas correctamente.");
                    break;

                default:
                    Console.WriteLine("Opcion invalida.");
                    break;
            }

            if (opcion != 4)
            {
                Console.WriteLine("\nPresione una tecla para continuar...");
                Console.ReadKey();
            }

        } while (opcion != 4);
    }

    static void AgregarMascota(List<Mascota> mascotas, ref int contadorId)
    {
        Console.Clear();
        Console.WriteLine("===== AGREGAR MASCOTA =====");

        Console.Write("Nombre: ");
        string nombre = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(nombre))
        {
            Console.WriteLine("Error: el nombre no puede estar vacio.");
            return;
        }

        Console.Write("Especie: ");
        string especie = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(especie))
        {
            Console.WriteLine("Error: la especie no puede estar vacia.");
            return;
        }

        Console.Write("Peso: ");
        decimal peso;

        if (!decimal.TryParse(Console.ReadLine(), out peso) || peso <= 0)
        {
            Console.WriteLine("Error: el peso debe ser un numero mayor que 0.");
            return;
        }

        Mascota mascota = new Mascota();
        mascota.Id = contadorId;
        mascota.Nombre = nombre;
        mascota.Especie = especie;
        mascota.Peso = peso;

        mascotas.Add(mascota);
        contadorId++;

        Console.WriteLine("Mascota agregada correctamente.");
    }

    static void ListarMascotas(List<Mascota> mascotas)
    {
        Console.Clear();
        Console.WriteLine("===== LISTA DE MASCOTAS =====");

        if (mascotas.Count == 0)
        {
            Console.WriteLine("No hay mascotas registradas.");
            return;
        }

        for (int i = 0; i < mascotas.Count; i++)
        {
            string clasificacion = mascotas[i].Peso > 25
                ? "Raza grande"
                : "Raza pequeña/mediana";

            Console.WriteLine("\nMascota " + (i + 1));
            Console.WriteLine(mascotas[i].ToString());
            Console.WriteLine("Clasificacion: " + clasificacion);
        }
    }

    static void BuscarPorEspecie(List<Mascota> mascotas)
    {
        Console.Clear();
        Console.WriteLine("===== ESPECIE =====");
        Console.Write("Ingrese la especie a buscar: ");

        string texto = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(texto))
        {
            Console.WriteLine("Error: debe ingresar una especie.");
            return;
        }

        bool encontrada = false;

        for (int i = 0; i < mascotas.Count; i++)
        {
            if (mascotas[i].Especie.ToLower().Contains(texto.ToLower()))
            {
                Console.WriteLine(mascotas[i].ToString());
                encontrada = true;
            }
        }

        if (!encontrada)
        {
            Console.WriteLine("No se encontraron mascotas.");
        }
    }

    static void GuardarMascotas(List<Mascota> mascotas)
    {
        List<string> lineas = new List<string>();

        for (int i = 0; i < mascotas.Count; i++)
        {
            string linea = mascotas[i].Id + "," +
                           mascotas[i].Nombre + "," +
                           mascotas[i].Especie + "," +
                           mascotas[i].Peso;

            lineas.Add(linea);
        }

        File.WriteAllLines("mascotas.csv", lineas);
    }

    static void CargarMascotas(List<Mascota> mascotas, ref int contadorId)
    {
        if (!File.Exists("mascotas.csv"))
        {
            return;
        }

        string[] lineas = File.ReadAllLines("mascotas.csv");
        int mayorId = 0;

        for (int i = 0; i < lineas.Length; i++)
        {
            string[] datos = lineas[i].Split(',');

            if (datos.Length == 4)
            {
                Mascota mascota = new Mascota();

                int id;
                decimal peso;

                if (int.TryParse(datos[0], out id) &&
                    decimal.TryParse(datos[3], out peso))
                {
                    mascota.Id = id;
                    mascota.Nombre = datos[1];
                    mascota.Especie = datos[2];
                    mascota.Peso = peso;

                    mascotas.Add(mascota);

                    if (id > mayorId)
                    {
                        mayorId = id;
                    }
                }
            }
        }

        contadorId = mayorId + 1;
    }
}