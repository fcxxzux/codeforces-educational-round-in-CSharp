using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Numerics;
using E = System.Linq.Enumerable;

namespace ProbD {
    class Program {
        protected IOHelper io;

        public Program(string inputFile, string outputFile) {
            io = new IOHelper(inputFile, outputFile, Encoding.Default);
            //直接思路：枚举1~m的lcm，对每个lcm枚举约数，然后约数的bin里都要，复杂度msqrt(m)
            //那么现在反转，枚举约数，再枚举这个数是哪些可能的lcm值的约数
            //根据调和级数公式，n/1+n/2+n/3+...n/n=nlogn

            int n=io.NextInt(),m = io.NextInt();
            List<int>[] bin = new List<int>[m + 1];
            for (int i = 0; i <= m; i++) bin[i] = new List<int>();
            for (int i = 1; i <= n; i++) {
                int x = io.NextInt();
                if (x <= m) bin[x].Add(i);
            }
            int[] cnt = new int[m + 1];
            for (int i = 1; i <= m; i++) {
                if (bin[i].Count == 0) continue;
                for (int j = i; j <= m; j += i) {
                    cnt[j] += bin[i].Count;
                }
            }
            int maxElement=cnt.Max();
            for (int i = 1; i <= m; i++) {
                if (cnt[i] == maxElement) {
                    io.WriteLine(i+" "+maxElement);
                    List<int> ans=new List<int>();
                    for (int j = 1; j <= Math.Sqrt(i); j++) {
                        if (i % j == 0) {
                            ans.AddRange(bin[j]);
                            if (j * j != i) ans.AddRange(bin[i / j]);
                        }
                    }
                    ans.Sort();
                    foreach (int k in ans) {
                        io.Write(k + " ");
                    }
                    break;
                }
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
