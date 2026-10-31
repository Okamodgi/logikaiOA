using System;
using System.Diagnostics;
using System.Globalization;

namespace Lab2
{
    internal static class Program
    {
        private static readonly Random Rng = new Random();

        static void FillMatrix(int[] m, int n)
        {
            for (int i = 0; i < n * n; i++)
                m[i] = Rng.Next(1, 101);
        }

        static void Multiply(int[] a, int[] b, int[] c, int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int sum = 0;
                    for (int r = 0; r < n; r++)
                        sum += a[i * n + r] * b[r * n + j];
                    c[i * n + j] = sum;
                }
            }
        }

        static void Task1()
        {
            int[] sizes = { 100, 200, 400, 1000, 2000, 4000, 10000 };
            double[] times = new double[sizes.Length];

            Console.WriteLine("Сложность матрицы, O(n^3)\n");
            Console.WriteLine($"{"n",10} {"time(s)",12}");
            Console.WriteLine("------------------------");

            for (int k = 0; k < sizes.Length; k++)
            {
                int n = sizes[k];
                int[] a, b, c;

                try
                {
                    a = new int[n * n];
                    b = new int[n * n];
                    c = new int[n * n];
                }
                catch (OutOfMemoryException)
                {
                    Console.WriteLine($"{n,10} {"нет памяти",12}");
                    times[k] = -1;
                    continue;
                }

                FillMatrix(a, n);
                FillMatrix(b, n);

                Stopwatch sw = Stopwatch.StartNew();
                Multiply(a, b, c, n);
                sw.Stop();

                times[k] = sw.Elapsed.TotalSeconds;
                Console.WriteLine($"{n,10} {times[k],12:F4}");
            }

            double maxTime = 0;
            for (int k = 0; k < sizes.Length; k++)
                if (times[k] > maxTime) maxTime = times[k];

            Console.WriteLine("\n-- graph --");
            for (int k = 0; k < sizes.Length; k++)
            {
                Console.Write($"{sizes[k],6} | ");
                if (times[k] < 0)
                {
                    Console.WriteLine("skipped");
                    continue;
                }
                int bar = maxTime > 0 ? (int)(times[k] / maxTime * 50) : 0;
                Console.WriteLine($"{new string('#', bar)} {times[k]:F4}");
            }
        }


        static void ShellSort(int[] items, int count)
        {
            int[] gaps = { 9, 5, 3, 2, 1 };

            foreach (int gap in gaps)
            {
                for (int i = gap; i < count; ++i)
                {
                    int x = items[i];
                    int j;
                    for (j = i - gap; j >= 0 && x < items[j]; j -= gap)
                        items[j + gap] = items[j];
                    items[j + gap] = x;
                }
            }
        }

        static void QuickSort(int[] items, int left, int right)
        {
            int i = left, j = right;
            int x = items[(left + right) / 2];

            do
            {
                while (i < right && items[i] < x) i++;
                while (j > left && x < items[j]) j--;

                if (i <= j)
                {
                    int y = items[i];
                    items[i] = items[j];
                    items[j] = y;
                    i++;
                    j--;
                }
            } while (i <= j);

            if (left < j) QuickSort(items, left, j);
            if (i < right) QuickSort(items, i, right);
        }

        static void FillRandom(int[] arr, int n)
        {
            for (int i = 0; i < n; i++)
                arr[i] = Rng.Next(n * 10);
        }

        static void FillAscending(int[] arr, int n)
        {
            for (int i = 0; i < n; i++)
                arr[i] = i;
        }

        static void FillDescending(int[] arr, int n)
        {
            for (int i = 0; i < n; i++)
                arr[i] = n - i;
        }

        static void FillMountain(int[] arr, int n)
        {
            int half = n / 2;
            for (int i = 0; i < half; i++)
                arr[i] = i;
            for (int i = half; i < n; i++)
                arr[i] = n - i;
        }

        static double Measure(int[] src, Action<int[]> sort)
        {
            int[] tmp = (int[])src.Clone();

            Stopwatch sw = Stopwatch.StartNew();
            sort(tmp);
            sw.Stop();

            return sw.Elapsed.TotalSeconds;
        }

        static void RunTest(string label, int[] arr, int n)
        {
            double shell = Measure(arr, t => ShellSort(t, n));
            double quick = Measure(arr, t => QuickSort(t, 0, n - 1));
            double builtIn = Measure(arr, t => Array.Sort(t));

            Console.WriteLine($"{label,-14}  Shell: {shell:F6}  QuickSort: {quick:F6}  Array.Sort: {builtIn:F6}");
        }

        static void Task2()
        {
            int[] sizes = { 1000, 5000, 10000, 50000, 100000 };

            Console.WriteLine("sort alg timing\n");

            foreach (int n in sizes)
            {
                int[] arr = new int[n];

                Console.WriteLine($"--- n = {n} ---");

                FillRandom(arr, n);
                RunTest("Random", arr, n);

                FillAscending(arr, n);
                RunTest("Ascending", arr, n);

                FillDescending(arr, n);
                RunTest("Descending", arr, n);

                FillMountain(arr, n);
                RunTest("Mountain", arr, n);

                Console.WriteLine();
            }
        }

        static void Main()
        {
            // чтобы дробная часть выводилась через точку независимо от региональных настроек
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            Console.WriteLine("lab2");
            Console.WriteLine("1 - сложность матрицы");
            Console.WriteLine("2 - сортировка");
            Console.Write("> ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
                choice = -1;

            switch (choice)
            {
                case 1: Task1(); break;
                case 2: Task2(); break;
                default: Console.WriteLine("wrong"); break;
            }
        }
    }
}