using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Numerics;
using E = System.Linq.Enumerable;

namespace Prob15 {
    class Program {
        protected IOHelper io;

        public Program(string inputFile, string outputFile) {
            io = new IOHelper(inputFile, outputFile, Encoding.Default);

            //seems that in russia it's , as a separator not .
            double px = io.NextDouble(), py = io.NextDouble(), vx = io.NextDouble(), vy = io.NextDouble();
            int a=io.NextInt(),b=io.NextInt(),c=io.NextInt(),d=io.NextInt();
            Point p=new Point(px,py), v=new Point(vx,vy);
            v = v.unitize();
            Point vleft = v.left_turn(),vright=v.right_turn(),vreverse=v.reverse();
            Point p1 = p + v * b;
            io.WriteLine(p1.x.ToString("F18", System.Globalization.CultureInfo.InvariantCulture) + " " + p1.y.ToString("F18", System.Globalization.CultureInfo.InvariantCulture));
            Point p2 = p + vleft * (a / 2.0);
            io.WriteLine(p2.x.ToString("F18", System.Globalization.CultureInfo.InvariantCulture) + " " + p2.y.ToString("F18", System.Globalization.CultureInfo.InvariantCulture));
            Point p3 = p + vleft * (c / 2.0);
            io.WriteLine(p3.x.ToString("F18", System.Globalization.CultureInfo.InvariantCulture) + " " + p3.y.ToString("F18", System.Globalization.CultureInfo.InvariantCulture));
            Point p4 = p3 + vreverse * d;
            io.WriteLine(p4.x.ToString("F18", System.Globalization.CultureInfo.InvariantCulture) + " " + p4.y.ToString("F18", System.Globalization.CultureInfo.InvariantCulture));
            Point p6 = p + vright * (c / 2.0);
            Point p5 = p6 + vreverse * d;
            io.WriteLine(p5.x.ToString("F18", System.Globalization.CultureInfo.InvariantCulture) + " " + p5.y.ToString("F18", System.Globalization.CultureInfo.InvariantCulture));
            io.WriteLine(p6.x.ToString("F18", System.Globalization.CultureInfo.InvariantCulture) + " " + p6.y.ToString("F18", System.Globalization.CultureInfo.InvariantCulture));
            Point p7 = p + vright * (a / 2.0);
            io.WriteLine(p7.x.ToString("F18", System.Globalization.CultureInfo.InvariantCulture) + " " + p7.y.ToString("F18", System.Globalization.CultureInfo.InvariantCulture));

            io.Dispose();
        }

        static void Main(string[] args) {
            Program myProgram = new Program(null, null);
        }
    }

    class Point {
        public double x;
        public double y;
        public Point(double a, double b) {
            x = a; y = b;
        }
        public Point unitize() {
            double length =Math.Sqrt( x * x + y * y);
            Point ans=new Point(x/length,y/length);
            return ans;
        }
        public Point reverse() {
            return new Point(-x , -y );
        }
        public Point left_turn() {
            return new Point(-y, x);
        }
        public Point right_turn() {
            return new Point(y, -x);
        }
        public static Point operator *(Point a,double v){
            return new Point(a.x * v, a.y * v);
        }
        public static Point operator +(Point a, Point b) {
            return new Point(a.x + b.x, a.y + b.y);
        }
    }

    class IOHelper : IDisposable {
        public StreamReader reader;
        public StreamWriter writer;

        public IOHelper(string inputFile, string outputFile, Encoding encoding) {
            if (inputFile == null)
                reader = new StreamReader(Console.OpenStandardInput(), encoding);
            else
                reader = new StreamReader(inputFile, encoding);

            if (outputFile == null)
                writer = new StreamWriter(Console.OpenStandardOutput(), encoding);
            else
                writer = new StreamWriter(outputFile, false, encoding);

            curLine = new string[] { };
            curTokenIdx = 0;
        }

        string[] curLine;
        int curTokenIdx;

        char[] whiteSpaces = new char[] { ' ', '\t', '\r', '\n' };

        public bool hasNext() {
            if (curTokenIdx >= curLine.Length) {
                //Read next line
                string line = reader.ReadLine();
                if (line != null)
                    curLine = line.Split(whiteSpaces, StringSplitOptions.RemoveEmptyEntries);
                else
                    curLine = new string[] { };
                curTokenIdx = 0;
            }

            return curTokenIdx < curLine.Length;
        }

        public string NextToken() {
            return hasNext() ? curLine[curTokenIdx++] : null;
        }

        public int NextInt() {
            return int.Parse(NextToken());
        }

        public double NextDouble() {
            string tkn = NextToken();
            return double.Parse(tkn, System.Globalization.CultureInfo.InvariantCulture);
        }

        public void Write(double val, int precision) {
            writer.Write(val.ToString("F" + precision, System.Globalization.CultureInfo.InvariantCulture));
        }

        public void Write(object stringToWrite) {
            writer.Write(stringToWrite);
        }

        public void WriteLine(double val, int precision) {
            writer.WriteLine(val.ToString("F" + precision, System.Globalization.CultureInfo.InvariantCulture));
        }

        public void WriteLine(object stringToWrite) {
            writer.WriteLine(stringToWrite);
        }

        public void Dispose() {
            try {
                if (reader != null) {
                    reader.Dispose();
                }
                if (writer != null) {
                    writer.Flush();
                    writer.Dispose();
                }
            } catch { };
        }


        public void Flush() {
            if (writer != null) {
                writer.Flush();
            }
        }
    }
}
