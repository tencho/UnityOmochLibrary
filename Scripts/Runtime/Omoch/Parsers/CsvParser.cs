using System.Collections.Generic;

namespace Omoch.Parser
{
    public class CsvParser
    {
        public static Dictionary<string, string>[] Parse(string csv)
        {
            // FIXME: 無駄に各行にDictionaryを持たせてしまっているのをなんとかする
            List<Dictionary<string, string>> result = new();
            string[] columns = new string[]{};
            string[] lines = csv.Replace("\r\n", "\n").Replace("\r", "\n").Split("\n");
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                string[] values = line.Split(",");
                if (i == 0)
                {
                    columns = values;
                }
                else
                {
                    Dictionary<string, string> rows = new();
                    for (int j = 0; j < values.Length; j++)
                    {
                        string key = columns[j];
                        string value = values[j];
                        rows.Add(key, value);
                    }
                    result.Add(rows);
                }
            }
            return result.ToArray();
        }
    }
}

