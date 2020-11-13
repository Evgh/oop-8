using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oop_8
{
    // ---------------------------------------------------------- классы из 5 лабы 

    abstract class Tech
    {
        protected bool isRunning;
        protected Tech()
        {
            isRunning = false;
        }

        public bool TurnOn()
        {
            isRunning = true;
            return true;
        }

        public bool TurnOff()
        {
            isRunning = false;
            return true;
        }

        public virtual bool IsItRunning()
        {
            return isRunning;
        }

        public override string ToString()
        {
            return $"Техника типа {GetType()}, работает ли: {isRunning}.";
        }
    }


    sealed class Scaner : Tech
    {
        public override bool IsItRunning()
        {
            if (base.isRunning)
            {
                Console.WriteLine("Сканер сканирует.");
            }
            else
            {
                Console.WriteLine("Сканер не сканирует.");
            }
            return base.isRunning;
        }

        public override string ToString()
        {
            return $"Сканер типа {GetType()}, работает ли: {isRunning}.";
        }
    }


    // ---------------------------------------------------------- новое в 8 лабе

    // ------------------------------ Исключения

    [Serializable]
    public class SetException : Exception
    {
        public SetException() { }
        public SetException(string message) : base(message) { }
        public SetException(string message, Exception inner) : base(message, inner) { }
        protected SetException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class SetAddException : SetException
    {
        public SetAddException() { }
        public SetAddException(string message) : base(message) { }
        public SetAddException(string message, Exception inner) : base(message, inner) { }
        protected SetAddException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class SetDeleteException : SetException
    {
        public SetDeleteException() { }
        public SetDeleteException(string message) : base(message) { }
        public SetDeleteException(string message, Exception inner) : base(message, inner) { }
        protected SetDeleteException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    // ------------------------------ Абстрактный интерфейс 
    interface IDo<T> where T : new()
    {
        bool Add(T element);
        bool Delete(T element);
        T Watch(int i);
    }

    // ---------------------------------------------------------- перелопаченный код из 4 лабы
    internal class Set<T> : IDo <T> where T: new()
    {
        List<T> _data; 
        internal int Length
        {
            get => _data.Count;
        }

        internal Set()
        {
            _data = new List<T> { };
        }

        internal T this[int i]
        {
            get
            {
                if (i > _data.Count)
                    return default;

                return _data[i];
            }
        }

        // добавление во множество
        public bool Add(T element)
        {

            if (this | element)
                throw new SetAddException("Ошибка добавления: во множестве уже существует данный элемент");

            _data.Add(element);
            return true;
        }

        public bool Delete(T element)
        {
            if (_data.Remove(element))
            {
                return true;
            }
            else
                throw new SetDeleteException("Ошибка удаления: такого элемента во множестве не существует");
        }

        public T Watch(int i)
        {
            if (i > _data.Count)
                throw new SetException();
            return this[i];
        }

        // проверка на принадлежность элемента
        public static bool operator | (Set<T> set, T element)
        {
            return set._data.Contains(element);
        }

        // Пересечение
        public static Set<T> operator %(Set<T> set1, Set<T> set2)
        {
            Set<T> buff = new Set<T>();
            for (int i = 0; i < set1._data.Count; i++)
            {
                if (set2 | set1[i])
                    buff.Add(set1[i]);
            }
            return buff;
        }

        // Является второе подмножеством первого
        public static bool operator > (Set<T> set1, Set<T> set2) 
        {
            for (int i = 0; i < set2._data.Count; i++)
            {
                if (!(set1 | set2[i]))
                    return false;     
            }
            return true;
        }
        // является ли первое подмножеством второго
        public static bool operator < (Set<T> set1, Set<T> set2) 
        {
            return set2 > set1;
        }


        public static bool operator == (Set<T> set1, Set<T> set2)
        {
            return set1.Equals(set2);
        }
        public static bool operator != (Set<T> set1, Set<T> set2)
        {
            return !set1.Equals(set2);
        }


        // переопределение методов object
        public override int GetHashCode()
        {
            int hash = 0;
            for(int i = 0; i < Length; i++)
            {
                hash += (int)(Math.Pow(this[i].GetHashCode(), i));
            }
            return hash;
        }
        public override bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder buff = new StringBuilder();
            buff.Append("{\n");
            for (int i = 0; i < Length; i++)
            {
                buff.Append("\t" + this[i] + "\n");
            }
            buff.Append("}");
            return buff.ToString();
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            
            var first = new Set<int> { };
            var second = new Set<Scaner> { };

            for (int i = 0; i < 15; i++)
            {
                try
                {
                    first.Add(i);
                }
                catch(SetAddException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch(SetException e)
                {
                    Console.WriteLine(e.Message);
                }
 
            }
            Console.WriteLine("Первое множество: " + first.ToString());


            try
            {
                if (first.Delete(6))
                    Console.WriteLine("Удален элемент 6"); 
                if (first.Delete(56)) 
                    Console.WriteLine("Удален элемент 56"); 
            }
            catch (SetDeleteException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (SetException e)
            {
                Console.WriteLine(e.Message);
            }






            for (int i = 0; i < 3; i++)
            {
                try
                {
                    second.Add(new Scaner());
                }
                catch (SetAddException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (SetException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            var scan = new Scaner();
            var scan1 = new Scaner();
            scan.TurnOn();
            try
            {
                second.Add(scan);
                second.Add(scan1);
            }
            catch (SetAddException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (SetException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("Второе множество: " + second.ToString());


            try
            {
                if (second.Delete(scan1))
                    Console.WriteLine("Удален сканер");
            }
            catch (SetDeleteException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (SetException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("Второе множество: " + second.ToString());





            /*if (first.Add(2)) { Console.WriteLine("Вставка удалась"); } else { Console.WriteLine("Не вставлено"); }
            if (first.Add(2)) { Console.WriteLine("Вставка удалась"); } else { Console.WriteLine("Не вставлено"); }
            if (first.Add(3)) { Console.WriteLine("Вставка удалась"); } else { Console.WriteLine("Не вставлено"); }
            if (first.Add(4)) { Console.WriteLine("Вставка удалась"); } else { Console.WriteLine("Не вставлено"); }
            if (first.Add(6)) { Console.WriteLine("Вставка удалась"); } else { Console.WriteLine("Не вставлено"); }
            if (first.Add(5)) { Console.WriteLine("Вставка удалась"); } else { Console.WriteLine("Не вставлено"); }
            */
            /*            Set second = new Set();
                        if (second << 10) { Console.WriteLine("Вставка удалась"); } else { Console.WriteLine("Не вставлено"); }
                        if (second << 2) { Console.WriteLine("Вставка удалась"); } else { Console.WriteLine("Не вставлено"); }
                        if (second << 56) { Console.WriteLine("Вставка удалась"); } else { Console.WriteLine("Не вставлено"); }
                        if (second << 5) { Console.WriteLine("Вставка удалась"); } else { Console.WriteLine("Не вставлено"); }
                        if (second << 6) { Console.WriteLine("Вставка удалась"); } else { Console.WriteLine("Не вставлено"); }
                        Console.WriteLine("Второе множество: " + second.ToString());
                        Console.WriteLine("Упорядоченный вариант: " + (second.OrderBy()).ToString());
                        Console.WriteLine("Содержится ли второе множество в первом? " + (second < first));

                        Set third = first % second;
                        Console.WriteLine("Третье множество -- пересечение первых двух: " + third.ToString());
                        Console.WriteLine("Содержится ли пересечение двух множеств в первом? " + (third < first));
                        Console.WriteLine("Равняется ли пересечение двух множеств третьему множеству? " + (third == first % second));
                        Console.WriteLine("Верно ли, что третье множество не равняется первому? " + (third != first));*/
        }
    }
}
