using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Numerics;
using E = System.Linq.Enumerable;

namespace ProbD {
    class Record:IComparable<Record> {
        public int id,x,y;

        public Record(int id, int x,int y) {
            this.id = id;
            this.x = x;
            this.y = y;
        }
        
        int IComparable<Record>.CompareTo(Record other) {
            if (x != other.x) return x < other.x ? 1 : -1;
            if (y != other.y) return y < other.y ? -1 : 1;
            return id < other.id ? -1 : 1;
        }
    }

    class Program {
        protected IOHelper io;

        int n;
        int[] a;
        int lowbit(int x) {
            return x & (-x);
        }
        void add(int x, int val) {
            while (x <= 2 * n) {
                a[x] += val;
                x += lowbit(x);
            }
        }
        int sum(int x) {
            int ans = 0;
            while (x > 0) {
                ans += a[x];
                x -= lowbit(x);
            }
            return ans;
        }
        public Program(string inputFile, string outputFile) {
            io = new IOHelper(inputFile, outputFile, Encoding.Default);

            SortedSet<int> s = new SortedSet<int>();
            n= io.NextInt();
            int[] ans = new int[n];
            a = new int[2 * n + 5];
            Record[] rec = new Record[n];
            for (int i = 0; i < n; i++) {
                rec[i] = new Record(i,io.NextInt(),io.NextInt());
                if(!s.Contains(rec[i].x))s.Add(rec[i].x);
                if(!s.Contains(rec[i].y))s.Add(rec[i].y);
            }
            Array.Sort(rec);
            var it=s.GetEnumerator();
            var s2 = new SortedDictionary<int, int>();
            int id=1;
            while (it.MoveNext()) {
                s2.Add(it.Current, id++);
            }

            for (int i = 0; i < n; i++) {
                rec[i].x = s2[rec[i].x];
                rec[i].y = s2[rec[i].y];
            }

            for (int i = 0; i < n; i++) {
                ans[rec[i].id] = sum(rec[i].y);
                add(rec[i].y, 1);
            }

            for (int i = 0; i < n; i++) {
                io.WriteLine(ans[i]);
            }
            io.Dispose();
        }

        static void Main(string[] args) {
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
