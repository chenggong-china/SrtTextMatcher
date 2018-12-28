using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrtTextMatcher
{
    class SrtTextMatcher
    {
        public SrtTextMatcher(string srtFilePath, string textFilePath, string outputSrtFilePath)
        {
            //解析字幕文件
            List<SrtContent> srtFile = SrtContent.LoadStrFile(srtFilePath);

            //读取text文件
            TxtString = File.ReadAllText(textFilePath).Replace("\r\n","");
            TxtStringMap = new int[TxtString.Length];
            for (int i = 0; i < TxtStringMap.Length; ++i)
                TxtStringMap[i] = 0;

            //准备语音识别的匹配数据
            SrtString = "";
            List<int> tmp = new List<int>();

            foreach(var srtLine in srtFile)
            {
                SrtString += srtLine.content;
                foreach(var c in srtLine.content)
                {
                    tmp.Add(srtLine.index);
                }
            }

            StrStringMap = tmp.ToArray();

            SplitMatch(0, SrtString.Length, 0, TxtString.Length);

            int lastIndex = 0;
            //处理
            for(int i = 0; i < TxtStringMap.Length; ++i)
            {
                if (TxtStringMap[i] != 0)
                {
                    lastIndex = TxtStringMap[i];
                    
                }else
                {
                    TxtStringMap[i] = lastIndex;
                }

                Console.WriteLine($"{TxtString[i]}-{TxtStringMap[i]} ");
            }

            Dictionary<int, string> rst = new Dictionary<int, string>();
            //生成
            for (int i = 0; i < TxtStringMap.Length; ++i)
            {
                int index = TxtStringMap[i];
                char c = TxtString[i];
                if (!rst.ContainsKey(index))
                {
                    rst[index] = "";
                }

                rst[index] += c;
            }

            foreach(var kv in rst)
            {
                srtFile[kv.Key - 1].content = kv.Value;
            }

            var result = SrtContent.SaveToString(srtFile);
            Console.WriteLine(result);
            File.WriteAllText(outputSrtFilePath, result);
        }

        string SrtString; //语音识别出来全部的文字
        int[] StrStringMap; //对应每个字符的偏移

        string TxtString; //待匹配字幕文件
        int[] TxtStringMap; //匹配结果


        void SplitMatch(int startA,int endA,int startB, int endB)
        {
            if (startA == endA || startB == endB)
                return;
            
            string subStrA = SrtString.Substring(startA, endA - startA);
            string subStrB = TxtString.Substring(startB, endB - startB);

            List<string> subStr = SearchMaxSubStr(subStrA.ToLower(), subStrB.ToLower());
            if(subStr.Count > 0)
            {
                //只取第一个
                string match = subStr[0];
                
                int offsetA = subStrA.ToLower().IndexOf(match);
                int offsetB = subStrB.ToLower().IndexOf(match);

                for(int i = 0; i < match.Length; ++i)
                {
                    TxtStringMap[startB + offsetB + i] = StrStringMap[startA + offsetA + i];
                }

                //向左
                SplitMatch(startA, startA + offsetA, startB, startB + offsetB);

                //向右
                SplitMatch(startA + offsetA + match.Length, endA, startB + offsetB + match.Length, endB);
            }
        }

        /// <summary>
        /// 查找最大公共子串 (方法二)
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        static List<string> SearchMaxSubStr(string s1, string s2)
        {
            List<string> maxSubStr = new List<string>();
            if (s1 == s2)
            {
                maxSubStr.Add(s1);
                return maxSubStr;
            }

            int len1 = s1.Length;
            int len2 = s2.Length;
            int maxLen = 0;
            List<string> subStr = new List<string>();
            for (int i = 0; i < len2; i++)
            {
                for (int k = 0; k < len1; k++)
                {
                    if (s2[i] == s1[k])
                    {
                        int n = i;
                        int m = k;
                        int subLen = 0;
                        while (s2[n] == s1[m])
                        {
                            n++;
                            m++;
                            subLen++;
                            if (m == len1 || n == len2)
                            {
                                break;
                            }
                        }

                        if (maxLen < subLen)
                        {
                            maxLen = subLen;
                            subStr.Clear();
                            subStr.Add((n - 1) + "," + subLen);

                        }
                        else if (maxLen == subLen)
                        {
                            if (!subStr.Contains((n - 1) + "," + subLen))
                                subStr.Add((n - 1) + "," + subLen);
                        }
                    }
                }
            }
            for (int j = 0; j < subStr.Count; j++)
            {
                string[] subStrIndex = subStr[j].Split(',');
                int len = int.Parse(subStrIndex[1]);
                int index = int.Parse(subStrIndex[0]);
                string sub = "";
                for (int k = index - len + 1; k <= index; k++)
                {
                    sub = sub + s2.Substring(k, 1);
                }
                maxSubStr.Add(sub);
            }

            return maxSubStr;
        }
        
    }
}
