using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace IPR2
{
    // структура, которая хранит в себе текущие данные самолета
    struct Params
    {
        public double Speed { get; set; }
        public int Height { get; set; }
        public string Direction { get; set; }
    }

    // структура, являющаяся частью программы самолета
    struct Data
    {
        public TimeSpan Time { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    // структура для хранения предыдущих значений программы, которые нужны для вычислений
    struct OldData
    {
        public TimeSpan Prevtime { get; set; }
        public double Prevspeed { get; set; }
        public int Prevx { get; set; }
        public int Prevy { get; set; }
    }

    static class DataExtension
    {
        public static Params Evaluate(this Data data1, OldData data2)
        {

            double time = (data1.Time - data2.Prevtime).TotalSeconds;
            double acc = 2 * (Math.Sqrt(Math.Pow(data1.X - data2.Prevx, 2) + Math.Pow(data1.Y - data2.Prevy, 2)) - data2.Prevspeed * time) / Math.Pow(time, 2);
            return new Params() { Speed = Math.Abs(data2.Prevspeed + acc * time), Height = data1.Height, Direction = data1.Y > data2.Prevy ? "Up" : "Down" };
        }
    }

    class Plane
    {
        readonly string Name;
        List<Data> FlightProgram;
        public Plane(string path, string Name)
        {
            this.Name = Name;
            FlightProgram = new List<Data>();
            try
            {
                using (Stream stream = File.OpenRead(path))
                {
                    using (BinaryReader sr = new BinaryReader(stream))
                    {
                        while (true) { FlightProgram.Add(new Data() { Time = TimeSpan.Parse(sr.ReadString()), Height = sr.ReadInt32(), X = sr.ReadInt32(), Y = sr.ReadInt32() }); };
                    }
                }
            }
            // исключение при достижении конца файла
            catch (EndOfStreamException) { }
        }

        public void Flight()
        {
            Console.WriteLine("Самолет {0} запущен", Name);
            OldData prevdata = new OldData();
            Params para = new Params();
            for (int i = 0; i < FlightProgram.Count; i++)
            {
                para = FlightProgram[i].Evaluate(prevdata);
                Thread.Sleep(FlightProgram[i].Time - prevdata.Prevtime);
                Console.WriteLine("{0}\t{1}\t{2:###.###} м\\с\tВысота - {3}м\tНаправление - {4}", Name, FlightProgram[i].Time, para.Speed, para.Height, para.Direction);
                // сохраняем текущие данные для вычислений
                prevdata.Prevspeed = para.Speed;
                prevdata.Prevx = FlightProgram[i].X;
                prevdata.Prevy = FlightProgram[i].Y;
                prevdata.Prevtime = FlightProgram[i].Time;
            }
            Console.WriteLine("Самолет {0} полет завершил", Name);
        }
    }
}
