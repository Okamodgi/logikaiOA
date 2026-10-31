using System;
using System.Collections.Generic;

namespace Lab4
{
    internal class Node
    {
        public int Data;
        public Node Left;
        public Node Right;
    }

    internal static class Program
    {
        static Node root = null;

        static string ReadLineOrExit()
        {
            string s = Console.ReadLine();
            if (s == null) Environment.Exit(0);
            return s;
        }

        static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(ReadLineOrExit(), out int v)) return v;
                Console.WriteLine("Введите целое число");
            }
        }

        static int ReadCommand(string menu)
        {
            Console.Write(menu);
            return int.TryParse(ReadLineOrExit(), out int cmd) ? cmd : -1;
        }

        static Node CreateTree(Node parent, Node r, int data)
        {
            if (r == null)
            {
                r = new Node { Data = data, Left = null, Right = null };

                if (parent == null) return r;
                if (data > parent.Data) parent.Left = r;
                else parent.Right = r;
                return r;
            }

            if (data > r.Data)
                CreateTree(r, r.Left, data);
            else
                CreateTree(r, r.Right, data);

            return parent;
        }

        static Node CreateTreeUnique(Node parent, Node r, int data)
        {
            if (r == null)
            {
                r = new Node { Data = data, Left = null, Right = null };

                if (parent == null) return r;
                if (data > parent.Data) parent.Left = r;
                else parent.Right = r;
                return r;
            }

            if (data == r.Data)
            {
                Console.WriteLine($"Дубликат {data}, пропущен");
                return parent;
            }

            if (data > r.Data)
                CreateTreeUnique(r, r.Left, data);
            else
                CreateTreeUnique(r, r.Right, data);

            return parent;
        }

        static void PrintTree(Node r, int level)
        {
            if (r == null) return;

            PrintTree(r.Right, level + 1);
            Console.WriteLine($"L{level}: {new string(' ', 3 * level)}{r.Data}");
            PrintTree(r.Left, level + 1);
        }

        static void CollectLevels(Node r, int data, int currentLevel, List<int> levels)
        {
            if (r == null) return;

            if (r.Data == data)
                levels.Add(currentLevel);

            CollectLevels(r.Left, data, currentLevel + 1, levels);
            CollectLevels(r.Right, data, currentLevel + 1, levels);
        }

        static void TrimTree(Node node, int currentLevel, int maxLevel)
        {
            if (node == null || currentLevel > maxLevel) return;

            if (currentLevel == maxLevel)
            {
                node.Left = null;
                node.Right = null;
                return;
            }

            TrimTree(node.Left, currentLevel + 1, maxLevel);
            TrimTree(node.Right, currentLevel + 1, maxLevel);
        }

        static int CountVal(Node r, int data)
        {
            if (r == null) return 0;
            if (data == r.Data) return 1 + CountVal(r.Right, data);
            if (data > r.Data) return CountVal(r.Left, data);
            return CountVal(r.Right, data);
        }

        static void RunTree(string title, Func<Node, Node, int, Node> create)
        {
            root = null;

            Console.WriteLine($"{title}\n");

            while (true)
            {
                int cmd = ReadCommand(
                    "\n1 - Добавить\n" +
                    "2 - Показать \n" +
                    "3 - Найти \n" +
                    "4 - Посчитать\n" +
                    "5 - Сложность поиска\n" +
                    "6 - Обрезать дерево до уровня N\n" +
                    "7 - Выход\n" +
                    "> ");

                switch (cmd)
                {
                    case 1:
                        {
                            int val = ReadInt("Введите число: ");
                            root = create(root, root, val);
                            break;
                        }

                    case 2:
                        if (root == null)
                            Console.WriteLine("Дерево пустое");
                        else
                            PrintTree(root, 0);
                        break;

                    case 3:
                        {
                            int val = ReadInt("Введите значение: ");
                            var levels = new List<int>();
                            CollectLevels(root, val, 0, levels);

                            if (levels.Count == 0)
                                Console.WriteLine("Не найдено");
                            else
                            {
                                levels.Sort();
                                Console.WriteLine($"Найдено {levels.Count} раз(а) на уровнях: {string.Join(", ", levels)}");
                            }
                            break;
                        }

                    case 4:
                        {
                            int val = ReadInt("Введите значение: ");
                            Console.WriteLine($"Количество: {CountVal(root, val)}");
                            break;
                        }

                    case 5:
                        Console.WriteLine("Сложность поиска:");
                        Console.WriteLine("  Средняя: O(log n)");
                        Console.WriteLine("  Худшая:   O(n)");
                        break;

                    case 6:
                        {
                            int maxLevel = ReadInt("До какого уровня обрезать (начиная с 0)? ");
                            if (maxLevel < 0)
                            {
                                Console.WriteLine("Уровень должен быть >= 0");
                                break;
                            }
                            TrimTree(root, 0, maxLevel);
                            Console.WriteLine($"Дерево обрезано до уровня {maxLevel}");
                            break;
                        }

                    case 7:
                        root = null;
                        return;

                    default:
                        Console.WriteLine("Неверная команда");
                        break;
                }
            }
        }

        static void Task1()
        {
            RunTree("дерево (с дублями)", CreateTree);
        }

        static void Task2()
        {
            RunTree("дерево (без дублей)", CreateTreeUnique);
        }

        static void Main()
        {
            Console.WriteLine("lab4");
            Console.WriteLine("1 - дерево (с дублями)");
            Console.WriteLine("2 - дерево (без дублей)");
            Console.Write("> ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
                choice = -1;

            switch (choice)
            {
                case 1: Task1(); break;
                case 2: Task2(); break;
                default: Console.WriteLine("Неверный выбор"); break;
            }
        }
    }
}
