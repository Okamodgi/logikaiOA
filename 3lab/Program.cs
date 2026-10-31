using System;

namespace Lab3
{
    internal class Node
    {
        public string Inf;
        public int Priority;
        public Node Next;
    }

    internal struct FindResult
    {
        public Node Node;
        public int Position;
    }

    internal static class Program
    {
        static Node head = null;
        static Node last = null;

        static string ReadLineOrExit()
        {
            string s = Console.ReadLine();
            if (s == null) Environment.Exit(0);
            return s;
        }

        static string ReadName()
        {
            while (true)
            {
                Console.Write("Введите имя: ");
                string s = ReadLineOrExit().Trim();
                if (s.Length > 0) return s;
                Console.WriteLine("Имя не может быть пустым.");
            }
        }

        static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(ReadLineOrExit(), out int v)) return v;
                Console.WriteLine("Введите целое число.");
            }
        }

        static int ReadCommand(string menu)
        {
            Console.Write(menu);
            return int.TryParse(ReadLineOrExit(), out int cmd) ? cmd : -1;
        }

        static Node GetStruct()
        {
            return new Node
            {
                Inf = ReadName(),
                Priority = 0,
                Next = null
            };
        }

        static void Review()
        {
            if (head == null)
            {
                Console.WriteLine("Список пуст");
                return;
            }
            for (Node n = head; n != null; n = n.Next)
                Console.WriteLine($"  {n.Inf}");
        }

        static void ReviewPq()
        {
            if (head == null)
            {
                Console.WriteLine("Список пуст");
                return;
            }
            for (Node n = head; n != null; n = n.Next)
                Console.WriteLine($"  {n.Inf} (приоритет {n.Priority})");
        }

        static FindResult FindWithPosition(string name)
        {
            if (head == null)
            {
                Console.WriteLine("Список пуст");
                return new FindResult { Node = null, Position = -1 };
            }

            int pos = 1;
            for (Node n = head; n != null; n = n.Next)
            {
                if (n.Inf == name)
                    return new FindResult { Node = n, Position = pos };
                pos++;
            }

            Console.WriteLine("Не найдено");
            return new FindResult { Node = null, Position = -1 };
        }

        static Node Find(string name) => FindWithPosition(name).Node;

        static void Del(string name)
        {
            if (head == null)
            {
                Console.WriteLine("Список пуст");
                return;
            }

            bool found = false;
            Node prev = null;
            Node cur = head;

            while (cur != null)
            {
                if (cur.Inf == name)
                {
                    found = true;
                    if (prev == null) head = cur.Next;
                    else prev.Next = cur.Next;

                    if (cur == last) last = prev;
                    cur = cur.Next;
                }
                else
                {
                    prev = cur;
                    cur = cur.Next;
                }
            }

            if (!found)
                Console.WriteLine("Не найдено");
        }

        static void FreeList()
        {
            head = null;
            last = null;
        }

        static void PqAdd()
        {
            Node p = GetStruct();
            p.Priority = ReadInt("Введите приоритет: ");

            if (head == null || p.Priority > head.Priority)
            {
                p.Next = head;
                head = p;
                if (last == null) last = p;
                return;
            }

            Node prev = head;
            Node cur = head.Next;
            while (cur != null && cur.Priority >= p.Priority)
            {
                prev = cur;
                cur = cur.Next;
            }
            prev.Next = p;
            p.Next = cur;
            if (cur == null) last = p;
        }

        static void DelByPriority(int prio)
        {
            int count = 0;
            Node prev = null;
            Node cur = head;

            while (cur != null)
            {
                if (cur.Priority == prio)
                {
                    count++;
                    if (prev == null) head = cur.Next;
                    else prev.Next = cur.Next;

                    if (cur == last) last = prev;
                    cur = cur.Next;
                }
                else
                {
                    prev = cur;
                    cur = cur.Next;
                }
            }

            if (count == 0) Console.WriteLine($"Нет элементов с приоритетом {prio}");
            else Console.WriteLine($"Удалено {count} элементов");
        }

        static void Task1()
        {
            head = null;
            last = null;

            Console.WriteLine("Очередь с приоритетами\n");

            while (true)
            {
                int cmd = ReadCommand("\n1 - Добавить\n2 - Просмотреть\n3 - Найти\n4 - Удалить\n5 - Удалить по приоритету\n6 - Выход\n> ");

                switch (cmd)
                {
                    case 1:
                        PqAdd();
                        break;
                    case 2:
                        ReviewPq();
                        break;
                    case 3:
                        {
                            string name = ReadName();
                            var res = FindWithPosition(name);
                            if (res.Node != null)
                                Console.WriteLine($"Найдено: {res.Node.Inf} (приоритет {res.Node.Priority}), позиция: {res.Position}");
                            break;
                        }
                    case 4:
                        Del(ReadName());
                        break;
                    case 5:
                        DelByPriority(ReadInt("Введите приоритет: "));
                        break;
                    case 6:
                        FreeList();
                        return;
                }
            }
        }

        static void QueueAdd()
        {
            Node p = GetStruct();

            if (head == null)
            {
                head = p;
                last = p;
            }
            else
            {
                last.Next = p;
                last = p;
            }
        }

        static void QueueRemove()
        {
            if (head == null)
            {
                Console.WriteLine("Очередь пуста");
                return;
            }

            Console.WriteLine($"Удалено: {head.Inf}");
            head = head.Next;
            if (head == null) last = null;
        }

        static void QueueMoveToEnd(string name)
        {
            if (head == null)
            {
                Console.WriteLine("Очередь пуста");
                return;
            }

            Node prev = null;
            Node cur = head;
            while (cur != null)
            {
                if (cur.Inf == name) break;
                prev = cur;
                cur = cur.Next;
            }

            if (cur == null)
            {
                Console.WriteLine("Не найдено");
                return;
            }

            if (cur == last)
            {
                Console.WriteLine("Уже находится в конце");
                return;
            }

            if (prev == null) head = cur.Next;
            else prev.Next = cur.Next;

            cur.Next = null;
            last.Next = cur;
            last = cur;
            Console.WriteLine($"Перемещено {name} в конец");
        }

        static void Task2()
        {
            head = null;
            last = null;

            Console.WriteLine("Очередь (FIFO)\n");

            while (true)
            {
                int cmd = ReadCommand("\n1 - Вставить (Enqueue)\n2 - Извлечь (Dequeue)\n3 - Просмотреть\n4 - Найти\n5 - Удалить\n6 - Переместить в конец\n7 - Выход\n> ");

                switch (cmd)
                {
                    case 1:
                        QueueAdd();
                        break;
                    case 2:
                        QueueRemove();
                        break;
                    case 3:
                        Review();
                        break;
                    case 4:
                        {
                            string name = ReadName();
                            var res = FindWithPosition(name);
                            if (res.Node != null)
                                Console.WriteLine($"Найдено: {res.Node.Inf}, позиция: {res.Position}");
                            break;
                        }
                    case 5:
                        Del(ReadName());
                        break;
                    case 6:
                        QueueMoveToEnd(ReadName());
                        break;
                    case 7:
                        FreeList();
                        return;
                }
            }
        }

        static void StackPush()
        {
            Node p = GetStruct();

            p.Next = head;
            head = p;
            if (last == null) last = p;
        }

        static void StackPop()
        {
            if (head == null)
            {
                Console.WriteLine("Стек пуст");
                return;
            }

            Console.WriteLine($"Извлечено: {head.Inf}");
            head = head.Next;
            if (head == null) last = null;
        }

        static void StackReverse()
        {
            if (head == null)
            {
                Console.WriteLine("Стек пуст");
                return;
            }

            Node prev = null;
            Node cur = head;

            last = head;
            while (cur != null)
            {
                Node next = cur.Next;
                cur.Next = prev;
                prev = cur;
                cur = next;
            }
            head = prev;
            Console.WriteLine("Стек развёрнут");
        }
        static void MoveAllByNameToTop(string name)
        {
            if (head == null)
            {
                Console.WriteLine("Стек пуст");
                return;
            }

            // Сначала соберём все узлы с нужным именем в отдельный список (сохраняя порядок)
            Node topHead = null;
            Node topTail = null;

            Node prev = null;
            Node cur = head;

            while (cur != null)
            {
                Node next = cur.Next;

                if (cur.Inf == name)
                {
                    if (prev == null)
                    {
                        head = next;
                    }
                    else
                    {
                        prev.Next = next;
                    }

                    cur.Next = null;
                    if (topHead == null)
                    {
                        topHead = cur;
                        topTail = cur;
                    }
                    else
                    {
                        topTail.Next = cur;
                        topTail = cur;
                    }
                }
                else
                {
                    prev = cur;
                }

                cur = next;
            }

            if (topHead == null)
            {
                Console.WriteLine("Не найдено ни одного элемента с таким именем");
                return;
            }
            if (head != null)
            {
                Node n = head;
                while (n.Next != null) n = n.Next;
                last = n;
            }
            else
            {
                last = topTail;
            }

            topTail.Next = head;
            head = topHead;

            Console.WriteLine($"Все элементы с именем \"{name}\" перемещены наверх стека");
        }



        static void Task3()
        {
            head = null;
            last = null;

            Console.WriteLine("Стек (LIFO)\n");

            while (true)
            {
                int cmd = ReadCommand("\n1 - Добавить (Push)\n2 - Извлечь (Pop)\n3 - Просмотреть\n4 - Найти\n5 - Удалить\n6 - Развернуть\n7 - Перенос наверх\n8 - Выход\n> ");

                switch (cmd)
                {
                    case 1:
                        StackPush();
                        break;
                    case 2:
                        StackPop();
                        break;
                    case 3:
                        Review();
                        break;
                    case 4:
                        {
                            string name = ReadName();
                            var res = FindWithPosition(name);
                            if (res.Node != null)
                                Console.WriteLine($"Найдено: {res.Node.Inf}, позиция: {res.Position}");
                            break;
                        }
                    case 5:
                        Del(ReadName());
                        break;
                    case 6:
                        StackReverse();
                        break;
                    case 7:
                        MoveAllByNameToTop(ReadName());
                        break;
                    case 8:
                        FreeList();
                        return;
                }
            }
        }

        static void Main()
        {
            Console.WriteLine("Лабораторная работа 3");
            Console.WriteLine("1 - очередь с приоритетами");
            Console.WriteLine("2 - очередь (FIFO)");
            Console.WriteLine("3 - стек (LIFO)");
            Console.Write("> ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
                choice = -1;

            switch (choice)
            {
                case 1: Task1(); break;
                case 2: Task2(); break;
                case 3: Task3(); break;
                default: Console.WriteLine("Неверный выбор"); break;
            }
        }
    }
}
