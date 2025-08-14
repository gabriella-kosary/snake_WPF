using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Snake.Model;

namespace Snake.Persistence
{
    public class ReadFile
    {
        public void Load(string _path, SnakeTableModel snakeTable)
        {
            try
            {
                StreamReader sr = new StreamReader(_path);
                string? s = sr.ReadLine();

                if (s == null) { throw new IOException(); }
                snakeTable.NewTable(int.Parse(s));
                int lineCount = 0;
                string[] line;
                int thing;
                while (!sr.EndOfStream)
                {
                    s = sr.ReadLine();
                    if (s == null) { throw new IOException(); }
                    line = s.Split(' ');
                    for (int i = 0; i < line.Length; i++)
                    {
                        thing = int.Parse(line[i]);
                        if(thing == 1) // FAL
                        {
                            snakeTable.walls.Add(new PointP(lineCount, i));
                        }
                    }
                    lineCount++;
                }
                snakeTable.NewEgg();
                sr.Close();
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
