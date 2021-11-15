﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace Lab1
{
    class Program
    {
        private const string filename = @"C:\Users\zaxar\source\repos\Lab1\TextFile1.txt";
        static void Main(string[] args)
        {
            Console.WriteLine("Test 1\n\n");
            Test1();
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("Test 2\n\n");
            Test2();
        }

        static void Test1()
        {
            FdblVector2 f = FdblVector2Types.f2;
            V3DataArray array = new V3DataArray("Velocity", DateTime.Now, 3, 4, 1.0f, 1.0f, f);
            Console.WriteLine("Array until saving");
            Console.WriteLine(array.ToLongString("f"));
            V3DataArray.SaveBinary(filename, array);
            V3DataArray.LoadBinary(filename, ref array);
            Console.WriteLine("Array after saving");
            Console.WriteLine(array.ToLongString("f"));

            V3DataList arrayConverted = (V3DataList)array;
            Console.WriteLine("Array until saving");
            Console.WriteLine(arrayConverted.ToLongString("f"));
            V3DataList.SaveAsText(filename, arrayConverted);
            V3DataList.LoadAsText(filename, ref arrayConverted);
            Console.WriteLine("Array after saving");
            Console.WriteLine(arrayConverted.ToLongString("f"));
        }

        static void Test2()
        {
            FdblVector2 f = FdblVector2Types.f3;
            V3MainCollection mainCollection = new V3MainCollection();
            V3DataArray array = new V3DataArray("Velocity", DateTime.Now, 3, 4, 1.0f, 1.0f, f);
            V3DataArray array1 = new V3DataArray("Electricity", DateTime.Now, 2, 2, 1.0f, 1.0f, f);
            V3DataList array2 = new V3DataList("Density", DateTime.MinValue);
            V3DataArray array3 = new V3DataArray("Electricity_zero", DateTime.Now, 0, 0, 1.0f, 1.0f, f); // zero elements
            V3DataList array4 = new V3DataList("Density_zero", DateTime.MinValue); // zero elements
            array2.Add(new DataItem(0.0f, 1.2f, f(0.0f, 1.2f)));
            array2.Add(new DataItem(2.0f, 3.2f, f(2.0f, 3.2f)));
            array2.Add(new DataItem(2.5f, 3.7f, f(2.5f, 3.7f)));
            mainCollection.Add(array);
            mainCollection.Add(array1);
            mainCollection.Add(array2);
            mainCollection.Add(array3);
            mainCollection.Add(array4);


            Console.WriteLine("MainCollection");
            Console.WriteLine(mainCollection.ToLongString("f"));
            Console.WriteLine("First request: Average lengths of points");
            double average = mainCollection.Average;
            Console.WriteLine(average);
            Console.WriteLine();


            Console.WriteLine("MainCollection");
            Console.WriteLine(mainCollection.ToLongString("f"));
            Console.WriteLine("Second request: Difference between maximum and minimum absolute value of field");
            IEnumerable<float> diametr = mainCollection.DiaField;
            foreach (var el in diametr)
            {
                Console.WriteLine(el);
            }
            Console.WriteLine();

            Console.WriteLine("MainCollection");
            Console.WriteLine(mainCollection.ToLongString("f"));
            Console.WriteLine("Third request: Group DataItem's by X");
            IEnumerable<IGrouping<double, DataItem>> group = mainCollection.GroupByX;
            foreach (IGrouping<double, DataItem> el in group)
            {
                foreach (DataItem item in el.ToList())
                {
                    Console.WriteLine(item);
                }
            }
            Console.WriteLine();
        }
    }
}