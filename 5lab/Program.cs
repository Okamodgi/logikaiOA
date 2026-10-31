using System;

class Edge
{
    public int U;
    public int V;
    public bool IsLoop;

    public Edge(int u, int v)
    {
        U = u;
        V = v;
        IsLoop = (u == v);
    }
}

class Graph
{
    public int N;
    public int M;
    public bool IsDirected;

    public int[,] Adj;
    public int[,] Inc = new int[0, 0];
    public Edge[] Edges = Array.Empty<Edge>();

    public Graph(int n, bool isDirected)
    {
        N = n;
        IsDirected = isDirected;
        Adj = new int[n, n];
    }

    public void BuildEdgeList()
    {
        var edges = new System.Collections.Generic.List<Edge>();

        if (IsDirected)
        {
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    if (Adj[i, j] != 0)
                        edges.Add(new Edge(i + 1, j + 1));
        }
        else
        {
            for (int i = 0; i < N; i++)
                for (int j = i; j < N; j++)
                    if (Adj[i, j] != 0)
                        edges.Add(new Edge(i + 1, j + 1));
        }

        Edges = edges.ToArray();
        M = Edges.Length;
    }

    public void PrintEdges()
    {
        Console.WriteLine($"\nСписок рёбер (m = {M}):");

        if (M == 0)
        {
            Console.WriteLine("  Рёбер нет");
            return;
        }

        for (int i = 0; i < M; i++)
        {
            if (Edges[i].IsLoop)
            {
                Console.WriteLine($"  e{i + 1,-2}: петля на v{Edges[i].U}");
            }
            else if (IsDirected)
                Console.WriteLine($"  e{i + 1,-2}: v{Edges[i].U} -> v{Edges[i].V}");
            else
                Console.WriteLine($"  e{i + 1,-2}: (v{Edges[i].U}, v{Edges[i].V})");
        }
    }

    public void PrintAdjacencyMatrix()
    {
        Console.WriteLine("Матрица смежности:");
        Console.Write("    ");

        for (int j = 0; j < N; j++)
            Console.Write($"v{j + 1} ");

        Console.WriteLine();

        for (int i = 0; i < N; i++)
        {
            Console.Write($"v{i + 1}  ");

            for (int j = 0; j < N; j++)
                Console.Write($"{Adj[i, j],-3}");

            Console.WriteLine();
        }
    }

    public void BuildIncidenceMatrix()
    {
        Inc = new int[N, M];

        for (int k = 0; k < M; k++)
        {
            int u = Edges[k].U - 1;
            int v = Edges[k].V - 1;

            if (Edges[k].IsLoop)
            {
                Inc[u, k] = 2;
            }
            else if (IsDirected)
            {
                Inc[u, k] = 1;
                Inc[v, k] = -1;
            }
            else
            {
                Inc[u, k] = 1;
                Inc[v, k] = 1;
            }
        }
    }

    public void PrintIncidenceMatrix()
    {
        if (M == 0)
        {
            Console.WriteLine("Рёбер нет");
            return;
        }

        Console.WriteLine("Матрица инцидентности:");
        Console.Write("    ");

        for (int j = 0; j < M; j++)
            Console.Write($"e{j + 1,-3}");

        Console.WriteLine();

        for (int i = 0; i < N; i++)
        {
            Console.Write($"v{i + 1}  ");

            for (int j = 0; j < M; j++)
                Console.Write($"{Inc[i, j],-4}");

            Console.WriteLine();
        }
    }

    public void Task1()
    {
        Console.WriteLine("\n[Задание 1: Анализ матрицы смежности]");
        PrintAdjacencyMatrix();

        int[] degOut = new int[N];
        int[] degIn = new int[N];
        int[] degTotal = new int[N];

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (Adj[i, j] != 0)
                {
                    degOut[i]++;
                    degIn[j]++;
                }
            }
        }

        for (int i = 0; i < N; i++)
            degTotal[i] = IsDirected ? degOut[i] + degIn[i] : degOut[i];

        Console.WriteLine($"Размер графа (число рёбер): {M}");
        Console.WriteLine("Степени вершин:");

        for (int i = 0; i < N; i++)
        {
            if (IsDirected)
            {
                Console.WriteLine(
                    $"  v{i + 1}: deg+ = {degOut[i]} (исход), " +
                    $"deg- = {degIn[i]} (заход), сумма = {degTotal[i]}");
            }
            else
            {
                Console.WriteLine($"  v{i + 1}: deg = {degTotal[i]}");
            }
        }

        PrintVertices("Изолированные вершины", degTotal, 0);
        PrintVertices("Концевые (висячие) вершины", degTotal, 1);

        Console.Write("Доминирующие вершины: ");
        int count = 0;
        for (int i = 0; i < N; i++)
        {
            int target = IsDirected ? degOut[i] : degTotal[i];
            if (target == N - 1 && N > 1)
            {
                Console.Write($"v{i + 1} ");
                count++;
            }
        }
        if (count == 0) Console.Write("нет");
        Console.WriteLine();
    }

    public void Task2()
    {
        BuildIncidenceMatrix();

        Console.WriteLine("\n[Задание 2*: Анализ матрицы инцидентности]");
        PrintIncidenceMatrix();

        int[] deg = new int[N];
        for (int i = 0; i < N; i++)
            for (int k = 0; k < M; k++)
                deg[i] += Math.Abs(Inc[i, k]);

        Console.WriteLine($"Размер графа: {M}");

        Console.Write("Степени вершин: ");
        for (int i = 0; i < N; i++)
        {
            Console.Write($"v{i + 1}: {deg[i]}");
            if (i < N - 1) Console.Write(", ");
        }
        Console.WriteLine();

        PrintVertices("Изолированные", deg, 0);
        PrintVertices("Концевые", deg, 1);

        Console.Write("Доминирующие вершины (по матрице инцидентности): ");
        int count = 0;
        for (int i = 0; i < N; i++)
        {
            int target;
            if (IsDirected)
            {
                int degOutInc = 0;
                for (int k = 0; k < M; k++)
                {
                    int val = Inc[i, k];
                    if (val == 1 || val == 2) degOutInc++;
                }
                target = degOutInc;
            }
            else
            {
                target = deg[i];
            }

            if (target == N - 1 && N > 1)
            {
                Console.Write($"v{i + 1} ");
                count++;
            }
        }
        if (count == 0) Console.Write("нет");
        Console.WriteLine();

        int loops = 0;
        foreach (var e in Edges)
            if (e.IsLoop) loops++;

        double loopPercent = M == 0 ? 0 : (double)loops * 100.0 / M;
        Console.WriteLine($"Число петель: {loops}");
        Console.WriteLine($"Вероятность (доля) петель: {loopPercent:F2}%");
    }

    private void PrintVertices(string title, int[] degrees, int target)
    {
        Console.Write($"{title}: ");
        int count = 0;

        for (int i = 0; i < N; i++)
        {
            if (degrees[i] == target)
            {
                Console.Write($"v{i + 1} ");
                count++;
            }
        }

        if (count == 0) Console.Write("нет");
        Console.WriteLine();
    }
}

class Program
{
    static readonly Random Random = new Random();

    static void RunManual()
    {
        Graph g = new Graph(6, false);

        Console.WriteLine("\n--- Граф из методички (Рисунок 1) ---");

        AddUndirectedEdge(g, 0, 1);
        AddUndirectedEdge(g, 0, 2);
        AddUndirectedEdge(g, 0, 5);
        AddUndirectedEdge(g, 1, 3);
        AddUndirectedEdge(g, 2, 4);

        g.BuildEdgeList();
        g.PrintEdges();
        g.Task1();
        g.Task2();
    }

    static void AddUndirectedEdge(Graph g, int u, int v)
    {
        g.Adj[u, v] = 1;
        g.Adj[v, u] = 1;
    }

    static void RunRandom(int n, bool isDirected, int probability)
    {
        Graph g = new Graph(n, isDirected);

        string type = isDirected ? "ориентированный" : "неориентированный";

        Console.WriteLine(
            $"\n--- Случайный {type} граф (n = {n}, p = {probability}%) ---");

        if (isDirected)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    g.Adj[i, j] = Random.Next(100) < probability ? 1 : 0;
                }
            }
        }
        else
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    int value = Random.Next(100) < probability ? 1 : 0;

                    g.Adj[i, j] = value;
                    if (i != j) g.Adj[j, i] = value;
                }
            }
        }

        g.BuildEdgeList();
        g.PrintEdges();
        g.Task1();
        g.Task2();
    }

    static int ReadInt(string message, int defaultValue)
    {
        Console.Write(message);
        string? input = Console.ReadLine();

        return int.TryParse(input, out int value) ? value : defaultValue;
    }

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1. Граф из методички (Рисунок 1)");
            Console.WriteLine("2. Случайный граф");
            Console.WriteLine("0. Выход");

            int choice = ReadInt("Выбор: ", 0);

            if (choice == 0) break;

            if (choice == 1)
            {
                RunManual();
            }
            else if (choice == 2)
            {
                int n = ReadInt("Количество вершин n: ", 6);
                if (n < 2) n = 6;

                int type = ReadInt(
                    "Тип графа (0 - неориентированный, 1 - ориентированный): ",
                    0);
                if (type != 0 && type != 1) type = 0;

                int probability = ReadInt(
                    "Вероятность наличия ребра (0..100%): ",
                    30);
                if (probability < 0 || probability > 100) probability = 30;

                RunRandom(n, type == 1, probability);
            }
            else
            {
                Console.WriteLine("Неизвестный пункт меню.");
            }
        }
    }
}
