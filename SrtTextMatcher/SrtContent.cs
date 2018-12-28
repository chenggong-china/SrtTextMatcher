using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrtTextMatcher
{
    
    class SrtContent
    {
        public int index;
        public string timeSrt;
        public string content;

        public override string ToString()
        {
            return $"{index}-{timeSrt}-{content}";
        }



        //读文件
        public static List<SrtContent> LoadStrFile(string path)
        {
            List<SrtContent> rst = new List<SrtContent>();
            string[] lines = File.ReadAllLines(path);
            int i = 0;
            while (i < lines.Length)
            {
                string line = lines[i];
                if (string.IsNullOrEmpty(line))
                {
                    i++;
                    continue;
                }

                int index = int.Parse(line);
                string time = lines[++i];
                string content = lines[++i];
                rst.Add(new SrtContent() { index = index, timeSrt = time, content = content });

                i++;
            }
            return rst;
        }

        public static string SaveToString(List<SrtContent> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var line in list)
            {
                sb.Append(line.index);
                sb.AppendLine();

                sb.Append(line.timeSrt);
                sb.AppendLine();

                sb.Append(line.content);
                sb.AppendLine();
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
