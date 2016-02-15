using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Numerics;
using E = System.Linq.Enumerable;

namespace ProbC {
    class Program {
        protected IOHelper io;

        public Program(string inputFile, string outputFile) {
            io = new IOHelper(inputFile, outputFile, Encoding.Default);

            List<int>[] bin = new List<int>[1000001];
            for (int i = 1; i <= 1000000; i++) bin[i] = new List<int>();

            int n = io.NextInt(), m = io.NextInt();
            int data;
            for (int i = 1; i <= n;i++ ) {
                data = io.NextInt();
                bin[data].Add(i);
            }
            int L, R, X;
            for (; m-- > 0; ) {
                L = io.NextInt(); R = io.NextInt(); X = io.NextInt();
                int Lans = bin[X].BinarySearch(L);
                if (Lans < 0) {
                    io.WriteLine(L);
                    continue;
                }
                int Rans = bin[X].BinarySearch(R);
                if (Rans < 0) {
                    io.WriteLine(R);
                    continue;
                }
                if (Rans - Lans == R - L) {
                    io.WriteLine(-1);
                    continue;
                }
                int mid, left = L, right = R;
                while (left <= right) {
                    mid = (left + right) >> 1;
                    int tmp = bin[X].BinarySearch(mid);
                    if (tmp < 0) { left = mid; break; }
                    if (tmp-Lans!=mid-L) right = mid - 1;
                    else left = mid + 1;
                }
                io.WriteLine(left);
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
