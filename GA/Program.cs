using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GA
{
    class Program
    {
        static Pool a;
        
        static void Main(string[] args)
        {
            //int n, m;



            //Console.WriteLine("вас приветтвует программа для распределения на кучи ");













            Random r = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            List<int> summ=new List<int>();
            List<int> data = new List<int>();
            int m = 5;

            for (int i = 0; i < m; i++)
                summ.Add(0);

            const int rChis = 200;
            int n = 10000;//30 / (m - 1) + r.Next(100);
            int tmp,tmpData;
            for (int i=0;i<n;i++)
            {
                tmp = r.Next(m);
                tmpData = r.Next(rChis);
                summ[tmp] += tmpData;
                data.Add(tmpData);
            }

            tmp = 0;

            foreach(int x in summ)
            {
                if (x > tmp) tmp = x;
                Console.WriteLine(x);
            }

            for (int i=0;i<m;i++)
            {
                Console.WriteLine("{0}          {1}", tmp, summ[i]);
                while(summ[i]<tmp)
                {
                    n++;
                    if((tmp - summ[i])> rChis)
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

            a = new Pool(n/10, m, n, 100, 20, 1, 1, 1, data);
            Console.WriteLine("n= {0}  m={1}", n, m);
            a.Evolution();
            Console.WriteLine();
            Console.WriteLine("n= {0}  m={1}",n,m);
            Console.ReadKey();
        }
    }
}
