using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Numerics;
using E = System.Linq.Enumerable;

namespace ProbE {
    class Program {
        protected IOHelper io;
        public const int mod = 1000000007;
        long llmul(long a, long b) {
            return (a % mod) * (b % mod) % mod;
        }

        void bf(long n, long m) {
            long ans = 0;
            for (long i = 1; i <= m; i++) {
                ans +=n % i;
                ans %= mod;
            }
            Console.WriteLine(ans);
        }

        public Program(string inputFile, string outputFile) {
            io = new IOHelper(inputFile, outputFile, Encoding.Default);
            
            long n, m;
            n = Int64.Parse(io.NextToken());
            m = Int64.Parse(io.NextToken());
            //bf(n, m);
            long ans = 0;
            if (m > n) {
                ans = llmul(n, m - n);
                m = n;
            }
            int sn = (int)Math.Sqrt(n);
            long lastp = m;
            for (int i = 1; i < sn; i++) {
                long p2 = n / (i + 1) + 1;
                if (p2 > lastp) continue;
                long num = lastp-p2 + 1;
                long ap = llmul(n%lastp,num)+llmul(llmul(llmul(num,num-1),i),500000004);
                //io.WriteLine("=" + i+" "+p2+" "+lastp + " " + ap);
                ans = (ans + ap) % mod;
                lastp = p2 - 1;
                
            }
            for (int i = 1; i <= lastp; i++) {
                ans += n % i;
                //io.WriteLine("-" + i + " " + ans);
                if (ans > (long)100 * mod) {
                    ans %= mod;
                }
            }
            io.WriteLine(ans%mod);

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
