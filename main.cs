using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    class Program
    {
        //part 1

        public static int min = 0;
        public static int max = 0;

        class MyMatrix
        {
            internal int _m, _n;
            internal int[][] _matrix;

            public MyMatrix(int m = 0, int n = 0, bool b = true)
            {
                _m = m;
                _n = n;
                Random random = new Random();
                _matrix = new int[m][];
                if (b)
                {
                    for (int i = 0; i < m; ++i)
                    {
                        _matrix[i] = new int[n];
                        for (int j = 0; j < n; ++j)
                        {
                            Random r = new Random();
                            _matrix[i][j] = min + (r.Next()) % (max - min + 1);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < m; ++i)
                    {
                        _matrix[i] = new int[n];
                        for (int j = 0; j < n; ++j)
                        {
                            _matrix[i][j] = 0;
                        }
                    }
                }
            }

            public MyMatrix(MyMatrix bas)
            {
                _m = bas._m;
                _n = bas._n;
                Random random = new Random();
                _matrix = new int[_m][];
                for (int i = 0; i < _m; ++i)
                {
                    _matrix[i] = new int[_n];
                    for (int j = 0; j < _n; ++j)
                    {
                        Random r = new Random();
                        _matrix[i][j] = bas._matrix[i][j];
                    }
                }
            }
            public void Print()
            {
                if (_m == 0 || _n == 0)
                {
                    Console.WriteLine("empty.");
                }
                else
                {
                    for (int i = 0; i < _m; ++i)
                    {
                        for (int j = 0; j < _n; ++j)
                        {
                            Console.Write($"{_matrix[i][j]} ");
                        }
                        Console.WriteLine();
                    }
                }
            }

            public static MyMatrix operator +(MyMatrix m1, MyMatrix m2)
            {
                if (m1._m == m2._m && m1._n == m2._n)
                {
                    MyMatrix rez = new MyMatrix(Math.Max(m1._m, m2._m), Math.Max(m1._n, m2._n), false);
                    for (int i = 0; i < rez._m; ++i)
                    {
                        for (int j = 0; j < rez._n; ++j)
                        {
                            if (i < m1._m && j < m1._n)
                            {
                                rez._matrix[i][j] += m1._matrix[i][j];
                            }
                            if (i < m2._m && j < m2._n)
                            {
                                rez._matrix[i][j] += m2._matrix[i][j];
                            }
                        }
                    }
                    return rez;
                }
                else
                {
                    Console.WriteLine("ERROR: matrixes have different sizes.");
                    return new MyMatrix();
                }
            }
            public static MyMatrix operator -(MyMatrix m1, MyMatrix m2)
            {
                if (m1._m == m2._m && m1._n == m2._n)
                {
                    MyMatrix rez = new MyMatrix(Math.Max(m1._m, m2._m), Math.Max(m1._n, m2._n), false);
                    for (int i = 0; i < rez._m; ++i)
                    {
                        for (int j = 0; j < rez._n; ++j)
                        {
                            if (i < m1._m && j < m1._n)
                            {
                                rez._matrix[i][j] += m1._matrix[i][j];
                            }
                            if (i < m2._m && j < m2._n)
                            {
                                rez._matrix[i][j] -= m2._matrix[i][j];
                            }
                        }
                    }
                    return rez;
                }
                else
                {
                    Console.WriteLine("ERROR: matrixes have different sizes.");
                    return new MyMatrix();
                }
            }

            public static MyMatrix operator *(MyMatrix m1, MyMatrix m2)
            {
                if (m1._n == m2._m)
                {
                    MyMatrix rez = new MyMatrix(m1._m, m2._n);
                    for (int i = 0; i < m1._m; ++i)
                    {
                        for (int j = 0; j < m2._n; ++j)
                        {
                            rez[i, j] = 0;
                            for (int k = 0; k < m1._n; ++k)
                                rez[i, j] += m1[i,k] * m2[k,j];
                        }
                    }
                    return rez;
                }
                else
                {
                    Console.WriteLine("Error: Incorrect matrixs to *.");
                    return new MyMatrix();
                }
            }

            public static MyMatrix operator *(MyMatrix m1, int x)
            {
                MyMatrix rez = new MyMatrix(m1);
                for (int i = 0; i < rez._m; ++i)
                {
                    for (int j = 0; j < rez._n; ++j)
                    {
                        rez._matrix[i][j] *= x;
                    }
                }
                return rez;
            }

            public static MyMatrix operator *(int x, MyMatrix m1)
            {
                MyMatrix rez = new MyMatrix(m1);
                for (int i = 0; i < rez._m; ++i)
                {
                    for (int j = 0; j < rez._n; ++j)
                    {
                        rez._matrix[i][j] *= x;
                    }
                }
                return rez;
            }

            public static MyMatrix operator /(MyMatrix m1, int x)
            {
                if (x == 0)
                {
                    MyMatrix rez = new MyMatrix();
                    Console.WriteLine("ERROR: can't divane on 0.");
                    return rez;
                }
                else
                {
                    MyMatrix rez = new MyMatrix(m1);
                    for (int i = 0; i < rez._m; ++i)
                    {
                        for (int j = 0; j < rez._n; ++j)
                        {
                            rez._matrix[i][j] = rez._matrix[i][j] / x;
                        }
                    }
                    return rez;
                }
            }
            public static MyMatrix operator /(int x, MyMatrix m1)
            {
                MyMatrix rez = new MyMatrix(m1);
                for (int i = 0; i < rez._m; ++i)
                {
                    for (int j = 0; j < rez._n; ++j)
                    {
                        if (rez._matrix[i][j] == 0) {
                            Console.WriteLine("ERROR: can't divane on 0.");
                        }
                        else {
                            rez._matrix[i][j] = x / rez._matrix[i][j];
                        }
                    }
                }
                return rez;
            }

            public int this[int i, int j]
            {
                get => _matrix[i][j];

                set => _matrix[i][j] = value;
            }
        }

        //part 2

        class Car
        {
            public string Name { get; set; }
            public int ProductionYear { get; set; }
            public int MaxSpeed { get; set; }

            public Car(string name, int year, int speed)
            {
                Name = name;
                ProductionYear = year;
                MaxSpeed = speed;
            }

            public override string ToString()
            {
                return $"{Name}, {ProductionYear}, {MaxSpeed}";
            }
        }



        class CarComparer: IComparer<Car>
        {
            int _mod;
            public CarComparer(int mod)
            {
                _mod = mod;
            }

            public int Compare(Car? X, Car? Y)
            {
                int rez;
                switch (_mod)
                {
                    case 0:
                        rez = X.Name.CompareTo(Y.Name);
                        break;
                    case 1:
                        rez = X.ProductionYear.CompareTo(Y.ProductionYear);
                        break;
                    default:
                        rez = X.MaxSpeed.CompareTo(Y.MaxSpeed);
                        break;
                }
                return rez;
            }
        }

        //part 3
        class CarCatalog
        {
            Car[] _catalog;

            public CarCatalog(Car[] catalog)
            {
                _catalog = catalog;
            }

            public IEnumerator<Car> GetEnumerator()
            {
                for (int i = 0; i < _catalog.Length; i++)
                {
                    yield return _catalog[i];
                }
            }
            public IEnumerable<Car> Reverse()
            {
                for (int i = _catalog.Length-1; i >= 0; i--)
                {
                    yield return _catalog[i];
                }
            }

            public IEnumerable<Car> SubsetYear(int year)
            {
                for (int i = 0; i < _catalog.Length; ++i)
                {
                    if (_catalog[i].ProductionYear == year)
                    {
                        yield return _catalog[i];
                    }
                }
            }
            public IEnumerable<Car> SubsetSpeed(int speed)
            {
                for (int i = 0; i < _catalog.Length; ++i)
                {
                    if (_catalog[i].MaxSpeed == speed)
                    {
                        yield return _catalog[i];
                    }
                }
            }
        }


        static void Main(string[] args)
        {
            //part 1
            Console.WriteLine("Part 1:");
            Console.WriteLine("Enter min and max:");

            string input = Console.ReadLine(); //получает строку с консоли
            var granica = input.Split(' '); //разделить на два элемента по пробелу
            int.TryParse(granica[0], out min);
            int.TryParse(granica[1], out max);


            Console.WriteLine("Enter m and n:"); 
            int n, m;
            input = Console.ReadLine(); //получает строку с консоли
            granica = input.Split(' '); //разделить на два элемента по пробелу
            int.TryParse(granica[0], out m);
            int.TryParse(granica[1], out n);

            MyMatrix matr1 = new MyMatrix(m, n);
            Console.WriteLine("matr1:");
            matr1.Print();

            Console.WriteLine("Enter m and n:");
            input = Console.ReadLine(); //получает строку с консоли
            granica = input.Split(' '); //разделить на два элемента по пробелу
            int.TryParse(granica[0], out m);
            int.TryParse(granica[1], out n);

            MyMatrix matr2 = new MyMatrix(m, n);
            Console.WriteLine("matr2:");
            matr2.Print();

            MyMatrix rez = matr1 + matr2;
            Console.WriteLine("matr1+matr2:");
            rez.Print();

            rez = matr1 - matr2;
            Console.WriteLine("matr1-matr2:");
            rez.Print();

            rez = matr1 * matr2;
            Console.WriteLine("matr1*matr2:");
            rez.Print();

            rez = matr1 * 2;
            Console.WriteLine("matr1*2:");
            rez.Print();

            rez = 2 * matr1;
            Console.WriteLine("2*matr1:");
            rez.Print();

            rez = matr1 / 2;
            Console.WriteLine("matr1/2:");
            rez.Print();

            rez = 2 / matr1;
            Console.WriteLine("2/matr1:");
            rez.Print();


            Console.WriteLine("matr2:");
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; ++j)
                {
                    Console.Write($"{matr2[i, j]} ");
                }
                Console.WriteLine(";");
            }

            //part 2
            Console.WriteLine("\nPart 2:");

            Car[] Base = new Car[]{
                                        new Car("Avia", 1998, 630),
                                        new Car("Mixa", 1905, 13),
                                        new Car("Avi",  2005, 20),
                                        new Car("Basta", 1998, 1000),
                                        new Car("Zoober", 2020, 630)
                                   };
            
            Console.WriteLine("Base:");
            for (int i = 0; i < Base.Length; i++)
            {
                Console.WriteLine($"    {Base[i]}");
            }

            CarComparer comparerString = new CarComparer(0);
            Car[] sort1 = Base;
            Array.Sort(sort1, comparerString);
            Console.WriteLine("Sorted by name:");
            for (int i = 0; i < sort1.Length; i++)
            {
                Console.WriteLine($"    {sort1[i]}");
            }

            comparerString = new CarComparer(1);
            Array.Sort(sort1, comparerString);
            Console.WriteLine("Sorted by year:");
            for (int i = 0; i < sort1.Length; i++)
            {
                Console.WriteLine($"    {sort1[i]}");
            }

            comparerString = new CarComparer(2);
            Array.Sort(sort1, comparerString);
            Console.WriteLine("Sorted by speed:");
            for (int i = 0; i < sort1.Length; i++)
            {
                Console.WriteLine($"    {sort1[i]}");
            }

            //part 3
            Console.WriteLine("\nPart 3:");
            CarCatalog catalog = new CarCatalog(Base);
            
            Console.WriteLine("Simple:");
            foreach (Car car in catalog)
            {
                Console.WriteLine($"    {car}");
            }

            Console.WriteLine("reverse:");
            foreach (Car car in catalog.Reverse())
            {
                Console.WriteLine($"    {car}");
            }
            
            Console.WriteLine("Filtred Year:");
            foreach (Car car in catalog.SubsetYear(1998))
            {
                Console.WriteLine($"    {car}");
            }
            Console.WriteLine("Filtres Speed:");
            foreach (Car car in catalog.SubsetSpeed(630))
            {
                Console.WriteLine($"    {car}");
            }

            Console.Read();
        }
    }
}
