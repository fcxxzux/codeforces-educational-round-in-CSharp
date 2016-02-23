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

        readonly int[,] di = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };
        string[] mp;
        int[,] rec;
        int[] sz;
        int[] qx;
        int[] qy;
        int n, m;

        int bfs(int px, int py,int id) {
            int cnt = 1;
            
            int hd=0,tail=1;
            qx[0]=px;
            qy[0]=py;
            rec[px, py] = id;
            for (;hd < tail;hd++) {
                for (int i = 0; i < 4; i++) {
                    int tx = qx[hd]+di[i,0];
                    int ty = qy[hd] + di[i, 1];
                    if (tx >= 0 && tx < n && ty >= 0 && ty < m) {
                        if (mp[tx][ty] != '*' && rec[tx, ty] == 0) {
                            qx[tail] = tx;
                            qy[tail] = ty;
                            rec[tx, ty] = id;
                            cnt++;
                            tail++;
                        }
                    }
                }
            }

            return cnt;
        }

        public Program(string inputFile, string outputFile) {
            io = new IOHelper(inputFile, outputFile, Encoding.Default);

            n = io.NextInt();
            m = io.NextInt();
            mp = new string[n];
            for (int i = 0; i < n; i++) mp[i] = io.NextToken();
            rec = new int[n, m];
            sz = new int[n * m + 5];
            qx = new int[n * m];
            qy = new int[n * m];
            int cid = 0;
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < m; j++) {
                    if (rec[i, j] == 0 && mp[i][j] != '*') {
                        ++cid;
                        sz[cid] = bfs(i, j, cid);
                    }
                }
            }
            
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < m; j++) {
                    if (mp[i][j] == '.') {
                        io.Write('.');
                    } else {
                        HashSet<int> l=new HashSet<int>();
                        int ans = 1;
                        for (int k = 0; k < 4; k++) {
                            int tx = i + di[k, 0];
                            int ty = j + di[k, 1];
                            if (tx >= 0 && tx < n && ty >= 0 && ty < m) {
                                if (mp[tx][ty] != '*' && l.Contains(rec[tx,ty])==false) {
                                    ans += sz[rec[tx, ty]];
                                    l.Add(rec[tx, ty]);
                                }
                            }
                        }
                        ans %= 10;
                        io.Write(ans);
                    }
                }
                if (i != n - 1) io.WriteLine("");
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
