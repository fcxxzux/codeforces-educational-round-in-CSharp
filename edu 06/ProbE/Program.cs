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
        List<int>[] G;
        int[] valN;
        int[] vis;
        int dfs_clock;
        int[] firstId, lastId;
        int n, m;

        long[] mask;
        bool[] lazy;

        void dfs(int u, int fa) {
            firstId[u] = ++dfs_clock;
            vis[dfs_clock] = u;
            int sz = G[u].Count();
            for (int i = 0; i < sz; ++i) {
                int v = G[u][i];
                if (v == fa) continue;
                dfs(v, u);
            }
            lastId[u] = dfs_clock;
        }

        long setMask(int x) {
            return ((long)1) << x;
        }
        int countMask(long x) {
            int ans = 0;
            while (x>0) {
                ans++;
                x -= (x & (-x));
            }
            return ans;
        }

        void build(int p, int l, int r) {
            lazy[p] = false;
            if (l == r) {
                mask[p] = setMask(valN[vis[l]]);
                return;
            }
            int m = (l + r) >> 1;
            int lchild = p << 1, rchild = p << 1 | 1;
            build(lchild, l, m);
            build(rchild, m + 1, r);
            mask[p] = mask[lchild] | mask[rchild];
        }
        long query(int L, int R, int o, int l, int r) {
            if ((L <= l && r <= R) || lazy[o]) {
                return mask[o];
            }
            int m = (l + r) >> 1;
            long lans = -1, rans = -1;
            if (L <= m) lans = query(L, R, o << 1, l, m);
            if (m < R) rans = query(L, R, o << 1 | 1, m + 1, r);
            if (lans == -1) return rans;
            if (rans == -1) return lans;
            return lans | rans;
        }
        void update(int L, int R, int o, int l, int r,int x) {
            if (L <= l && r <= R) {
                mask[o] = setMask(x);
                lazy[o] = true;
                return;
            }
            if (lazy[o]) {
                lazy[o] = false;
                mask[o << 1] = mask[o];
                mask[o << 1 | 1] = mask[o];
                lazy[o << 1] = lazy[o << 1 | 1] = true;
            }
            int m = (l + r) >> 1;
            if (L <= m) update(L, R, o << 1, l, m,x);
            if (m < R) update(L, R,  o << 1 | 1, m + 1, r,x);
            mask[o] = mask[o << 1] | mask[o << 1 | 1];
        }

        public Program(string inputFile, string outputFile) {
            io = new IOHelper(inputFile, outputFile, Encoding.Default);

            n = io.NextInt(); m = io.NextInt();
            valN = new int[n + 1];
            for (int i = 1; i <= n; i++) {
                valN[i] = io.NextInt();
            }
            G=new List<int>[n+1];
            for (int i = 1; i <= n; i++) G[i] = new List<int>();
            int task,sa, sb;
            for(int i=0;i<n-1;i++){
                sa=io.NextInt();
                sb=io.NextInt();
                G[sa].Add(sb);
                G[sb].Add(sa);
            }
            dfs_clock = 0;
            vis = new int[n * 2 + 5];
            firstId = new int[n + 1];
            lastId = new int[n + 1];
            dfs(1, 0);

            mask = new long[n * 8 + 5];
            lazy = new bool[n * 8 + 5];
            build(1, 1, dfs_clock);
            for (; m-- > 0; ) {
                task = io.NextInt();
                sa = io.NextInt();
                if (task == 1) {
                    sb = io.NextInt();
                    update(firstId[sa], lastId[sa], 1, 1, dfs_clock, sb);
                } else {
                    long mm = query(firstId[sa], lastId[sa], 1, 1, dfs_clock);
                    io.WriteLine(countMask(mm));
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
