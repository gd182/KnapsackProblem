using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace kr2
{
    class Program
    {

        public const string KRProcDLL = @"..\..\..\Debug\KRProc.dll";
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public  struct item 
        {
            public int Weiht;
            public double Coast;
            public double SpecCoast;
            public int NumItem;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct itemDP
        {
            public int Weiht;
            public double Coast;
            public int NumItem;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct OutInf
        {
            public int Weiht;
            public double Coast;
        };

        [DllImport(KRProcDLL, CallingConvention=CallingConvention.Cdecl)]
        public static extern void Sort([MarshalAs(UnmanagedType.LPArray), In, Out] item [] ArrItem, int Count);

        [DllImport(KRProcDLL, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.BStr)]
        public static extern string Alg([MarshalAs(UnmanagedType.LPArray), In] item[] ArrItem, int Count, int MaxW, ref OutInf Out);

        [DllImport(KRProcDLL, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.BStr)]
        public static extern string AlgDP([MarshalAs(UnmanagedType.LPArray), In] itemDP[] ArrItem, int Count, int MaxW, ref OutInf Out);

        static int inputI(ref item [] ArrItem)
        {
            Console.WriteLine("Введите количество доступных предметов");
            int Count = int.Parse(Console.ReadLine());
            Array.Resize(ref ArrItem, Count);
            Console.WriteLine("Введите вес предметов и его стоимость ");
            for (int i = 0; i < Count; i++)
            {
                Console.WriteLine($"предмет №{i+1}");
                ArrItem[i].Weiht = int.Parse(Console.ReadLine());
                ArrItem[i].Coast = int.Parse(Console.ReadLine());
                ArrItem[i].SpecCoast = ArrItem[i].Coast / ArrItem[i].Weiht;
                ArrItem[i].NumItem = i+1;
            }
            return Count;
        }

        static int inputDP(ref itemDP[] ArrItem)
        {
            Console.WriteLine("Введите количество доступных предметов");
            int Count = int.Parse(Console.ReadLine());
            Array.Resize(ref ArrItem, Count);
            Console.WriteLine("Введите вес предметов и его стоимость ");
            for (int i = 0; i < Count; i++)
            {
                Console.WriteLine($"предмет №{i + 1}");
                ArrItem[i].Weiht = int.Parse(Console.ReadLine());
                ArrItem[i].Coast = int.Parse(Console.ReadLine());
                ArrItem[i].NumItem = i + 1;
            }
            return Count;
        }

        static void greAlg()
        {
            Console.WriteLine("Жадный алгоритм");
            Console.WriteLine("Введите максимальную вместимость");
            int MaxW = int.Parse(Console.ReadLine());
            item[] ArrItem = new item[0];
            int Count = inputI(ref ArrItem);
            Sort(ArrItem, Count);
            OutInf Out;
            Out.Coast = Out.Weiht = 0;
            string NumItem = Alg(ArrItem, Count, MaxW, ref Out);
            Console.WriteLine($"Максимальная стоимость: {Out.Coast}");
            Console.WriteLine($"Вес: {Out.Weiht}");
            Console.WriteLine($"Номера предметов: {NumItem}");
            Console.ReadKey();
        }

        static void DPAlg()
        {
            Console.WriteLine("Динамическое программирование");
            Console.WriteLine("Введите максимальную вместимость");
            int MaxW = int.Parse(Console.ReadLine());
            itemDP[] ArrItem = new itemDP[0];
            int Count = inputDP(ref ArrItem);
            OutInf Out;
            Out.Coast = Out.Weiht = 0;
            string NumItem = AlgDP(ArrItem, Count, MaxW, ref Out);
            Console.WriteLine($"Максимальная стоимость: {Out.Coast}");
            Console.WriteLine($"Вес: {Out.Weiht}");
            Console.WriteLine($"Номера предметов: {NumItem}");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Для выбора жадного алгоритма введите 1, для выбора динамического программирования - 2");
            int ch = int.Parse(Console.ReadLine());
            switch (ch) {
                case 1:
                   greAlg();
                    break;
                case 2:
                    DPAlg();
                    break;
            }
        }
    }
}
