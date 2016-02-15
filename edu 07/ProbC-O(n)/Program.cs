using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Numerics;
using E = System.Linq.Enumerable;

namespace ProbC_O_n_ {
    class Program {
        protected IOHelper io;

        public Program(string inputFile, string outputFile) {
            io = new IOHelper(inputFile, outputFile, Encoding.Default);

            int n = io.NextInt(),m=io.NextInt();
            int[] nextDiff = new int[n];
            int[] d = new int[n];
            for (int i = 0; i < n;i++ ) d[i] = io.NextInt();
            nextDiff[n - 1] = -1;
            for (int i = n - 2; i >= 0; i--)
                nextDiff[i] = d[i] == d[i + 1] ? nextDiff[i + 1] : i + 1;

            for (; m-- > 0; ) {
                int L=io.NextInt(), R=io.NextInt(), X=io.NextInt();
                --L; --R;
                if (d[L] != X) io.WriteLine(L+1);
                else io.WriteLine(nextDiff[L] > R ||nextDiff[L]<0 ? -1 : nextDiff[L]+1);
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
