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

        readonly long mod = 1000000007;
        int m, d;
        long calc(string a) {
            int len = a.Length;
            long[, ,] f = new long[len, m, 2];

            for (int i = 0; i < a[0] - '0'; i++) {
                if (i == d) continue;
                f[0, i % m, 0] += 1;
            }
            if (a[0]-'0'!= d) f[0, (a[0] - '0') % m, 1] += 1;

            for (int i = 1; i < len; i++) {
                int lower = 0, upper = 9;
                if ((i & 1) > 0) {
                    lower = upper = d;
                }
                for (int j = lower; j <= upper; j++) {
                    if (j == d && (i & 1) == 0) continue;
                    for (int prev = 0; prev < m; prev++) {
                        int target = (prev * 10 + j) % m;
                        f[i, target, 0] += f[i - 1, prev, 0];
                        if (j < a[i] - '0') {
                            f[i, target, 0] += f[i - 1, prev, 1];
                        } else if (j == a[i] - '0') {
                            f[i, target, 1] += f[i - 1, prev, 1];
                            f[i, target, 1] %= mod;
                        }
                        f[i, target, 0] %= mod;
                    }
                }
            }
            return (f[len - 1, 0, 0] + f[len - 1, 0, 1])%mod;
        }

        public Program(string inputFile, string outputFile) {
            io = new IOHelper(inputFile, outputFile, Encoding.Default);

            m = io.NextInt(); d = io.NextInt();
            string a = io.NextToken(), b = io.NextToken();
            BigInteger tmp = BigInteger.Parse(a);
            tmp -= 1;
            String.Format("{0:D" + b.Length + "}", tmp);
            //io.WriteLine(a);
            //io.WriteLine(calc(b)); io.WriteLine(calc(a));
            long ans = calc(b) - calc(a);

            ans %= mod;
            if (ans < 0) ans += mod;
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
