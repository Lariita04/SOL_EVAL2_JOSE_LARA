using System;
using System.Threading;

class Program
{
    static int saldo = 1000; // Saldo inicial
    static readonly object lockObject = new object(); // Objeto de bloqueo para la sección crítica

    static void Main()
    {
        Console.WriteLine("Saldo inicial: $" + saldo);

        Console.Write("Ingrese el monto para el primer ingreso: ");
        int ingreso1 = Convert.ToInt32(Console.ReadLine());

        Console.Write("Ingrese el monto para el primer retiro: ");
        int retiro1 = Convert.ToInt32(Console.ReadLine());

        Console.Write("Ingrese el monto para el segundo ingreso: ");
        int ingreso2 = Convert.ToInt32(Console.ReadLine());

        Console.Write("Ingrese el monto para el segundo retiro: ");
        int retiro2 = Convert.ToInt32(Console.ReadLine());

        // Crear dos hilos para simular operaciones concurrentes
        Thread t1 = new Thread(() => RealizarOperacion(Ingreso, ingreso1));
        Thread t2 = new Thread(() => RealizarOperacion(Retiro, retiro1));

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        // Realizar el segundo conjunto de operaciones
        Thread t3 = new Thread(() => RealizarOperacion(Ingreso, ingreso2));
        Thread t4 = new Thread(() => RealizarOperacion(Retiro, retiro2));

        t3.Start();
        t4.Start();

        t3.Join();
        t4.Join();

        Console.WriteLine("Saldo final: $" + saldo);
    }

    static void RealizarOperacion(Action<int> operacion, int monto)
    {
        // Llamar a la operación específica (Ingreso o Retiro) y esperar a que termine
        operacion(monto);
    }

    static void Ingreso(int monto)
    {
        Console.WriteLine("Ingreso de $" + monto);

        // Sección crítica - operación de actualización del saldo
        lock (lockObject)
        {
            saldo += monto;
        }
    }

    static void Retiro(int monto)
    {
        Console.WriteLine("Retiro de $" + monto);

        // Sección crítica - operación de actualización del saldo
        lock (lockObject)
        {
            if (saldo >= monto)
            {
                saldo -= monto;
            }
            else
            {
                Console.WriteLine("Saldo insuficiente para el retiro.");
            }
        }
    }
}