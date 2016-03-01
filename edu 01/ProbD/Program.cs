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

        struct Point{
            public int x;
            public int y;
            public Point(int _x,int _y){
                x=_x;
                y=_y;
            }
            public static Point operator +(Point a, Point b) {
                return new Point(a.x + b.x, a.y + b.y);
            }
            public bool inRange(int n,int m){
                return x < n && y < m && x >= 0 && y >= 0;
            }
        }
        Point[] direction = new Point[] {
            new Point(-1, 0),
            new Point(1, 0),
            new Point(0, 1),
            new Point(0, -1) 
        };

        int blocks = 0;
        List<int> sizeOfEach;
        int n, m, k;
        string[] map;
        int[,] vis;

        int bfs(Point start) {
            ++blocks;
            vis[start.x, start.y] = blocks;

            int ans = 0;
            Queue<Point> q=new Queue<Point>();
            q.Enqueue(start);
            while (q.Count > 0) {
                Point top = q.Dequeue();
                for (int i = 0; i < 4;i++ ) {
                    Point tmp = top + direction[i];
                    if (tmp.inRange(n, m) && map[tmp.x][tmp.y] != '*' && vis[tmp.x,tmp.y] == 0) {
                        vis[tmp.x, tmp.y] = blocks;
                        q.Enqueue(tmp);
                    } else if (tmp.inRange(n, m) && map[tmp.x][tmp.y] == '*') {
                        ++ans;
                    }
                }
            }
            return ans;
        }

        public Program(string inputFile, string outputFile) {
            io = new IOHelper(inputFile, outputFile, Encoding.Default);

            sizeOfEach = new List<int>();
            sizeOfEach.Add(0);
            n = io.NextInt(); m = io.NextInt(); k = io.NextInt();
            map = new string[n];
            foreach (int i in E.Range(0, n)) {
                map[i] = io.NextToken();
            }
            vis = new int[n, m];
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < m; j++) {
                    if (map[i][j]!='*'&&vis[i, j] == 0) {
                        sizeOfEach.Add(bfs(new Point(i, j)));
                    }
                }
            }
            for (; k-- > 0; ) {
                int xi = io.NextInt();
                int yi = io.NextInt();
                --xi; --yi;
                io.WriteLine(sizeOfEach[vis[xi, yi]]);
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
