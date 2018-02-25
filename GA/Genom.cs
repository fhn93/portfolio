using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA
{
    struct Genom
    {
        public List<int> genomList;
        public int age;
        public Double rate;
        public bool del;
        public char t;
        public char dt;

        public void Print(List<int> data, int a)
        {
            List<int> tmpResult = new List<int>();
            for(int i=0;i<a;i++)
                tmpResult.Add(0);
            for (int i = 0; i < genomList.Count; i++)
                tmpResult[genomList[i]] += data[i];
            foreach (int res in tmpResult)
                Console.WriteLine(res);

            Console.WriteLine("age - {0,4} rate - {1,7}  del - {2} typeAdd = {3}", age,rate,del,t);
        }

        public void AddAge()
        {
            age++;
        }

        public void Raiting(int n, List<int> inputData, double MO)
        {
            int[] res=new int[n];
            for (int i = 0; i < genomList.Count; i++)

                res[genomList[i]] += inputData[i];
           
            double vexDig = 0;
            foreach (int x in res)
                vexDig += Math.Pow(MO-x,2);
            rate = Math.Pow(vexDig/n, 0.5);
           // Console.WriteLine("my rate = {0}",rate);
        }
    }

   

    public class Pool
    {
        List<Genom> poolLIst;//пул геномов, в нем проходит размножение, мутации, селлекция геномов
        Genom best;//лучгее на данный момент решение

        private Random random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

        private int poolSize;//параметры пула и геномов
        private int genomCount;
        private int genCount;
        private int maxAge;
        private int mortality;
        private int muttionCout;
        private int randomCoutn;
        private int childCount;
        private int foolCount;

        private double MO;

        public List<int> inputData;


        public Pool(int ps, int genC, int genomC, int mA,int mort, int mR,int rR, int cR, List<int> iD )//создание пула(начальное заполнение, полностью рандомно)!!!!!!!!!
        {
            poolSize = ps;
            genCount = genC;
            genomCount = genomC;
            maxAge = mA;
            mortality = mort;
            muttionCout = mR;
            randomCoutn = rR;
            childCount = cR;
            foolCount = rR + rR + mR;
            inputData = iD;
            MO = 0;
            foreach (int x in iD)
                MO += x;
            MO /= genCount;
            poolLIst = new List<Genom>();
            while (poolLIst.Count < poolSize)
                AddRandom();            
        }

        private void AddRandom()//создание рандомнго генома !!!!!!!!!!!!
        {
            Genom tmp = new Genom
            {
                age = 0,
                del = false,
                genomList = new List<int>(),
                t = 'r'
            };


            for (int i = 0; i < genomCount; i++)
                tmp.genomList.Add(random.Next(genCount));
            tmp.Raiting(genCount, inputData, MO);
            poolLIst.Add(tmp);
        }
        
        private void AddChildren()//создание дочернего генома из двух родительских 
        {
            Genom tmp = new Genom
            {
                age = 0,
                del = false,
                genomList = new List<int>(),
                t = 'c'
            };
            int mid = poolLIst.Count / 2;
            Genom p1 = poolLIst[random.Next(mid)];
            Genom p2 = poolLIst[mid + random.Next(mid)];


            for (int i = 0; i < p1.genomList.Count; i++)
                if (random.Next(2) == 0) 
                    tmp.genomList.Add(p1.genomList[i]);
                else
                    tmp.genomList.Add(p2.genomList[i]);
            tmp.Raiting(genCount, inputData, MO);
            poolLIst.Add(tmp);
        }

        private void Mutation()//добавляет новй геном изменяя один из существующих
        {
            Genom tmp = new Genom
            {
                age = 0,
                del = false,
                genomList = new List<int>(),
                t = 'm'
            };

            int coutn = poolLIst.Count / 10;

            int begin = coutn*9;
            int numGenom = begin+random.Next(coutn);

            tmp.genomList = poolLIst[numGenom].genomList.GetRange(0, genomCount);

            int changGen = random.Next(genomCount);
            int genValue= random.Next(genCount);
            
            tmp.genomList[changGen] = genValue;

            tmp.Raiting(genCount, inputData, MO);
            poolLIst.Add(tmp);
        }        

        private void NotDouble()
        {
           for(int i=0;i<poolLIst.Count-1;i++)
            for(int j=i+1; j < poolLIst.Count; j++)
            {
                if (poolLIst[i].genomList.SequenceEqual(poolLIst[j].genomList))
                {
                    Genom tmp = poolLIst[i];
                    tmp.del=true;
                    tmp.dt = 'd';
                    poolLIst[i] = tmp;
                }
            }
        }

        private void SoAld()
        {
            if (maxAge != 0 && poolLIst.Count != 0)
                for (int i = 0; i < poolLIst.Count; i++)
                    if(poolLIst[i].age>maxAge)
                    {
                        Genom tmp = poolLIst[i];
                        tmp.del = true;
                        tmp.dt = 'a';
                        poolLIst[i] = tmp;
                    }   
        }

        private void SoWeak()
        {
            int deathCount = poolLIst.Count * mortality / 100;
            for (int i = 0; i < deathCount; i++)
            {
                //if (poolLIst[i].age > maxAge / 10)
                {
                    Genom tmp = poolLIst[i];
                    tmp.del = true;
                    tmp.dt = 'w';
                    poolLIst[i] = tmp;
                }
            }
        }

        private void Selection()
        {

//            Console.WriteLine("selection");

            for (int i = 0; i < poolLIst.Count; i++)//костыль          
            {
                Genom tmp = poolLIst[i];
                tmp.age++;
                poolLIst[i] = tmp;
            }

            

            poolLIst.Sort(delegate(Genom g1, Genom g2)
            {
                return g2.rate.CompareTo(g1.rate);
            });

            if(poolLIst.Last().rate<=best.rate)
                best = poolLIst.Last();
            if (best.genomList == null)
                best.rate = 1000;
           
            SoAld();
            SoWeak();
            NotDouble();

            poolLIst.RemoveAll(delegate (Genom x)
            {
                if (x.del && x.age > 0)
                    return (true);
                else
                    return (false);                    
            });
        

            int curPoolSize = poolLIst.Count;
            int need = poolSize - curPoolSize;
            int burn = need * childCount / foolCount;
            int mut = need * muttionCout / foolCount;
            //проодим скрещивания
            for (int i = 0; i < burn; i++)
                AddChildren();

            //проодим мутаации
            for (int i = 0; i < mut; i++)
            {  
                Mutation();
            }

            while (poolLIst.Count < poolSize) AddRandom();//дополняем пул случайными геномами
        }

        public void Evolution()
        {
            Console.WriteLine("evolution begin");
            Selection();
            //Print();
            while (best.rate > 0)  
            {
                for(int i=0;i<10;i++)
                    Selection();
                Console.WriteLine("best:   ");
                best.Print(inputData,genCount);
                int max = 0;
                int sa = 0;
                double sr = 0;
                foreach (Genom g in poolLIst)
                {
                    sa += g.age;
                    sr += g.rate;
                    if (g.age > max) max = g.age;
                }
                Console.WriteLine("sa={0,10}   max={1,5}    sr={2,10}", (double)sa / poolSize, max, sr / poolSize);
            }
            Console.WriteLine("evolution end");

            int summ;
            for (int j = 0; j < genCount; j++)
            {
                summ = 0;
                Console.Write("{0})   ", j);
                for (int i = 0; i < genomCount; i++)
                    if (best.genomList[i] == j)
                    {
                        summ += inputData[i];
                        Console.Write("{0} ", inputData[i]);
                    }

                Console.WriteLine(summ);
                summ = 0;
            }
            
        }
    }
}
