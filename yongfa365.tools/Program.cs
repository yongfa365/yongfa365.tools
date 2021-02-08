using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace yongfa365.tools
{
    class Program
    {
        static void Main(string[] args)
        {
            //           var result= StringAlign(new string[] { "", @"
            //cabinType | airlineCode | isAsc | sortType | remark|
            //1         |             | false | 0        | 查询经济舱|
            //2         |             | false | 0        | 查询高级经济舱|
            //3         |             | false | 0        | 查询商务舱|
            //" });
            switch (args[0])
            {
                case "StringAlign":
                    var result = StringAlign(args);
                    Console.Write(result);
                    break;
                default:
                    break;
            }
        }

        static string StringAlign(string[] args)
        {
            var filename = args[1];
            var input = File.ReadAllText(filename, Encoding.UTF8);

            var items = input.Split('\n')
                .Where(p => p.Trim() != "" && !p.Trim().StartsWith("#"))
                .ToList();
            var dict = new Dictionary<int, int>();
            items.ForEach(item =>
            {
                var items2 = item.Split('|');
                for (int i = 0; i < items2.Length; i++)
                {
                    dict.TryGetValue(i, out int oldlen);

                    var newlen = Regex.Replace(items2[i].Trim(), @"[^\x00-\xff]", "aa").Length;
                    dict[i] = oldlen > newlen ? oldlen : newlen;
                }
            });

            var sb = new StringBuilder();
            items.ForEach(item =>
            {
                //sb.Append("|");
                var items2 = item.Split('|');
                for (int i = 0; i < items2.Length; i++)
                {
                    var nowItem = items2[i].Trim();

                    var expectLen = dict[i];
                    var nowLen = Regex.Replace(nowItem, @"[^\x00-\xff]", "aa").Length;
                    //var left = (expectLen - nowLen) / 2;
                    var left = 0;
                    var right = expectLen - nowLen - left;
                    sb.Append("".PadLeft(left));
                    sb.Append(nowItem);
                    if (i != items2.Length - 1)
                    {
                        sb.Append("".PadRight(right) + "|");
                    }
                }

                sb.Append("\r\n");
            });
            return sb.ToString().TrimEnd();
        }
    }
}
