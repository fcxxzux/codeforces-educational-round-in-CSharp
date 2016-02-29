using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;

namespace ProbC {
    class Vect : IComparable<Vect> {
        public long x, y;
        public int p, t;

        public int CompareTo(Vect obj) {
            if (t != obj.t) return t < obj.t ? -1 : 1;
            long res = y * obj.x - obj.y * x;
            if (res == 0) return 0;
            return res < 0 ? -1 : 1;
        }
    }
    class Frac : IComparable<Frac> {
        public long fz, fm;
        public int CompareTo(Frac obj) {
            BigInteger left = (BigInteger)fz * obj.fm;
            BigInteger right = (BigInteger)fm * obj.fz;
            return left.CompareTo(right);
        }
        public static bool operator <(Frac a, Frac b) {
            return a.CompareTo(b) < 0;
        }
        public static bool operator >(Frac a, Frac b) {
            return a.CompareTo(b) > 0;
        }
    }

    class Program {
        static Vect[] a;
        static Frac cal(int i, int j) {
            long x = a[i].x * a[j].x + a[i].y * a[j].y,
              y = a[i].x * a[i].x + a[i].y * a[i].y,
              z = a[j].x * a[j].x + a[j].y * a[j].y;
            Frac ans = new Frac();
            ans.fz = x * x;
            ans.fm = y * z;
            if (x < 0) ans.fz = -ans.fz;
            return ans;
        }

        protected IOHelper io;
        public Program(string inputFile, string outputFile) {
            io = new IOHelper(inputFile, outputFile, Encoding.Default);

            int n = io.NextInt();
            a = new Vect[n];
            for (int i = 0; i < n; i++) {
                a[i] = new Vect();
                int x = io.NextInt();
                int y = io.NextInt();
                a[i].x = x; a[i].y = y; a[i].p = i + 1;
                if (x >= 0 && y >= 0) a[i].t = 0;
                else if (x < 0 && y > 0) a[i].t = 1;
                else if (x <= 0 && y <= 0) a[i].t = 2;
                else a[i].t = 3;
            }
            Array.Sort(a);
            int ansx = a[0].p, ansy = a[n - 1].p;
            Frac best = cal(0, n - 1);
            for (int i = 0; i < n - 1; i++) {
                Frac now = cal(i, i + 1);
                if (now > best) {
                    ansx = a[i].p; ansy = a[i + 1].p;
                    best = now;
                }
            }
            io.WriteLine(ansx + " " + ansy);

            io.Dispose();
        }
        public static void Main(string[] args) {
            Program myProgram = new Program(null, null);
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

        public long NextLong() {
            return long.Parse(NextToken());
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