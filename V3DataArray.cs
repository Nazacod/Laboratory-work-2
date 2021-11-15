using System;
using System.IO;
using System.Numerics;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;


namespace Lab1
{
    class V3DataArray : V3Data
    {
        public int cntNodesX { get; private set; }
        public int cntNodesY { get; private set; }
        public double stepX { get; private set; }
        public double stepY { get; private set; }
        public Vector2[,] DataArray { get; private set; }
        public V3DataArray(string id, DateTime time) : base(id, time)
        {
            DataArray = new Vector2[,] { };
        }
        public V3DataArray(string id, DateTime time, int cntNodesX, int cntNodesY, double stepX, double stepY, FdblVector2 F)
            : base(id, time)
        {
            this.cntNodesX = cntNodesX;
            this.cntNodesY = cntNodesY;
            this.stepX = stepX;
            this.stepY = stepY;
            DataArray = new Vector2[this.cntNodesY, this.cntNodesX];
            for (int i = 0; i < this.cntNodesY; i++)
            {
                for (int j = 0; j < this.cntNodesX; j++)
                {
                    DataArray[i, j] = F(0.0 + j * stepX, 0.0 + i * stepY);
                }
            }
        }
        public override int Count
        {
            get { return cntNodesX * cntNodesY; }
        }
        public override double MaxDistance
        {
            get {
                return Math.Sqrt(Math.Pow(stepX * (cntNodesX - 1), 2) + Math.Pow(stepY * (cntNodesY - 1), 2));
            }
        }
        public override string ToString()
        {
            return $"V3DataArray: ({base.ToString()})\n" +
                $"Number of nodes by X: {cntNodesX}, number of nodes by Y: {cntNodesY}\n" +
                $"Step by x: {stepX}, step by y: {stepY}";
        }
        public override string ToLongString(string format)
        {
            string res = ToString() + "\n";
            double x, y;
            for (int i = 0; i < cntNodesY; i++)
            {
                for (int j = 0; j < cntNodesX; j++)
                {
                    x = 0.0 + j * stepX;
                    y = 0.0 + i * stepY;
                    res += $"В точке ({ x.ToString(format)}, { y.ToString(format)}) модуль переменной равен: { DataArray[i, j].Length().ToString(format)}, " +
                        $"компонента по X равна: {DataArray[i, j].X.ToString(format)}, " +
                        $"компонента по Y равна: {DataArray[i, j].Y.ToString(format)}\n";
                }
            }
            return res;
        }
        public static explicit operator V3DataList(V3DataArray param)
        {
            V3DataList obj = new V3DataList(param.id, param.time);
            for (int i = 0; i < param.cntNodesY; i++)
            {
                for (int j = 0; j < param.cntNodesX; j++)
                {
                    obj.Add(new DataItem(j * param.stepX, i * param.stepY, param.DataArray[i, j]));
                    //Console.WriteLine($"{i}, {j}");
                    //Console.WriteLine(obj.Add(new DataItem(j * param.stepX, i * param.stepY, param.DataArray[i, j])));
                }
            }
            return obj;
        }

        public override IEnumerator<DataItem> GetEnumerator()
        {
            for (int i = 0; i < cntNodesY; i++)
            {
                double y = stepY * i;

                for (int j = 0; j < cntNodesX; j++)
                {
                    double x = stepX * j;
                    yield return new DataItem(x, y, DataArray[i, j]);
                }
            }
        }

        public static bool SaveBinary(string filename, V3DataArray v3)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.OpenOrCreate);
                BinaryWriter writer = new BinaryWriter(fs);

                writer.Write(v3.id);
                writer.Write(v3.time.ToString());
                writer.Write(v3.stepX.ToString());
                writer.Write(v3.stepY.ToString());
                writer.Write(v3.cntNodesX.ToString());
                writer.Write(v3.cntNodesY.ToString());
                //writer.Write(v3.stepX);
                //writer.Write(v3.stepY);
                //writer.Write(v3.cntNodesX);
                //writer.Write(v3.cntNodesY);

                for (int i = 0; i < v3.cntNodesY; i++)
                    for (int j = 0; j < v3.cntNodesX; j++)
                    {
                        writer.Write(v3.DataArray[i, j].X + " " + v3.DataArray[i, j].Y);
                    }

                writer.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
            return true;
        }

        public static bool LoadBinary(string filename, ref V3DataArray v3)
        {
            CultureInfo CI = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            FileStream fs = null;

            try
            {
                fs = new FileStream(filename, FileMode.Open);
                BinaryReader reader = new BinaryReader(fs);

                v3.id = reader.ReadString();
                v3.time = DateTime.Parse(reader.ReadString(), CI);

                v3.stepX = double.Parse(reader.ReadString(), CI);
                v3.stepY = double.Parse(reader.ReadString(), CI);
                v3.cntNodesX = int.Parse(reader.ReadString(), CI);
                v3.cntNodesY = int.Parse(reader.ReadString(), CI);
                v3.DataArray = new Vector2[v3.cntNodesY, v3.cntNodesX];

                string[] elem = null;

                for (int i = 0; i < v3.cntNodesY; i++)
                {
                    for (int j = 0; j < v3.cntNodesX; j++)
                    {
                        elem = reader.ReadString().Split(' ');
                        v3.DataArray[i, j] = new Vector2(float.Parse(elem[0], CI), float.Parse(elem[1], CI));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
            return true;
        }
    }
}
