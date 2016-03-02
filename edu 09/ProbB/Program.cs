using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Numerics;
using E = System.Linq.Enumerable;

namespace ProbB {
    class Program {
        protected IOHelper io;

        public Program(string inputFile, string outputFile) {
            io = new IOHelper(inputFile, outputFile, Encoding.Default);

            int n = io.NextInt();
            int[] p = new int[n];
            for (int i = 0; i < n;i++ ) {
                p[i] = io.NextInt();
            }
            string s = io.NextToken();
            long ans = 0,init=0;
            for (int i = 0; i < n; i++) {
                if (s[i] == 'B') ans += p[i];
            }
            init = ans;
            long[] presum = new long[n];
            long[] sufsum = new long[n];
            for (int i = 0; i < n; i++) {
                presum[i] = (i != 0 ? presum[i - 1] : 0) + (s[i] == 'A' ? p[i] : -p[i]);
                ans = Math.Max(ans, init + presum[i]);
            }
            for (int i = n-1; i >=0; i--) {
                sufsum[i] = (i != n-1 ? sufsum[i + 1] : 0) + (s[i] == 'A' ? p[i] : -p[i]);
                ans = Math.Max(ans, init + sufsum[i]);
            }
            io.WriteLine(ans);

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
