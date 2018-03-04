using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace GA
{
    class Program
    {
        static Pool a;
        static int n, m;
        static List<int> data = new List<int>();

        static void GetMN(out int M, out int N)//считываем размер начальной кучи и количество куч после деления 
        {
            string s;
            Console.Write("Введите количество куч:  ");
            s = Console.ReadLine();
            while (!int.TryParse(s, out M) || M < 0)
            {
                Console.Write("Ввелено не корректное значание, введите количество куч:  ");
                s = Console.ReadLine();
            }


            Console.Write("Введите количество эллементов:  ");
            s = Console.ReadLine();
            while (!int.TryParse(s, out N) || N < 0)
            {
                Console.Write("Ввелено не корректное значание, введите количество эллементов:  ");
                s = Console.ReadLine();
            }
        }

        static void RandomInput()//запоненнение исходны данных случайными данными
        {
            GetMN(out m, out n);

            Random r = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            List<int> summ = new List<int>();

            for (int i = 0; i < m; i++)
                summ.Add(0);

            const int rChis = 200;
            int tmp, tmpData;
            for (int i = 0; i < n; i++)
            {
                tmp = r.Next(m);
                tmpData = r.Next(rChis);
                summ[tmp] += tmpData;
                data.Add(tmpData);
            }
            tmp = 0;

            foreach (int x in summ)
            {
                if (x > tmp) tmp = x;
                Console.WriteLine(x);
            }

            for (int i = 0; i < m; i++)
            {
                Console.WriteLine("{0}          {1}", tmp, summ[i]);
                while (summ[i] < tmp)
                {
                    n++;
                    if ((tmp - summ[i]) > rChis)
                    {
                        tmpData = r.Next(rChis);
                        summ[i] += tmpData;
                        data.Add(tmpData);
                    }
                    else
                    {
                        tmpData = tmp - summ[i];
                        summ[i] += tmpData;
                        data.Add(tmpData);
                    }
                }
            }
        }
        
        static void FileInput()//ввод данны из файла
        {
            bool flag = true;
            string s, line;
            string[] preData;

            while (flag)
            {
                flag = !flag;
                Console.Write("Введите полный путь к файлу:   ");
                s = Console.ReadLine();
                while (!File.Exists(s))
                {
                    Console.Write("файл не найден, Введите полный путь к файлу:   ");
                    s = Console.ReadLine();
                }

                using (StreamReader sr = new StreamReader(s))
                {
                    try
                        {
                        line = sr.ReadLine();

                        preData = line.Split(' ');
                        n = Convert.ToInt16(preData[1]);
                        m = Convert.ToInt16(preData[0]);

                        line = sr.ReadLine();

                        preData = line.Split(' ');

                        foreach (string tmp in preData)
                            data.Add(Convert.ToInt16(tmp));
                    }
                    catch
                    {
                        flag = !flag;
                    }
                    
                }
                if (data.Count != n) flag = !flag;
                if (flag) Console.Write("неверный формат файла");
            }
        }

        static void HandInput()//ввод данны вручную
        {
            string s;
            int tmp;
            GetMN(out m, out n);

            Console.WriteLine("Введите все эллементы начальной кучи, индивидуально:");
            for(int i=0;i<n; i++)
            {
                Console.Write("Введите {0}тый элемент  :", i);
                s = Console.ReadLine();
                while (!int.TryParse(s, out tmp))
                {
                    Console.Write("Ввелено не корректное значание, введите {0}тый элемент  :", i);
                    s = Console.ReadLine();
                }
                data.Add(tmp);
            }
        }

        static void Work()//обработка данных
        {
            int poolsize = (int)(n / 10 + Math.Log(n));
            if (poolsize < 20) poolsize = 20;
            a = new Pool(poolsize, m, n, 100, 20, 1, 1, 1, data);
            Console.WriteLine("n= {0}  m={1}", n, m);
            a.Evolution();
            Console.WriteLine();
            Console.WriteLine("n= {0}  m={1}", n, m);
            Console.ReadKey();
        }

        static bool Prow(out char mod)//проверка ввода мода для получания входных данных
        {
            if(!char.TryParse(Console.ReadLine(), out mod))          
                return true;
            if (!((mod == 'R') || (mod == 'F') || (mod == 'H')))
                return true;
            return false;
        }

        static void GetInpuData()//полуение данных
        {
            char c;
            
            Console.WriteLine("вас приветтвует программа для распределения на кучи ");

            Console.Write("Укажите метод введения исходных данных(из файла - F/в ручную - H/случайным образом - R)");
            
            while (Prow(out c))
            {
                Console.Write("Ввелено не корректное значание, укажите метод введения исходных данных(из файла - F/в ручную /случайным образом - R)");
            }

            switch (c)
            {

                case 'R':
                    RandomInput();
                    return;
                case 'F':
                    FileInput();
                    return;
                case 'H':
                    HandInput();
                    return;
            }
        }

        static void Main(string[] args)
        {

            GetInpuData();
            Work();
        }
    }
}
