using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

// Based on https://bravenewmethod.com/2014/09/13/lightweight-csv-reader-for-unity/

public class CSVReader
{
    private static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    private static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    private static char[] TRIM_CHARS = { '\"' };

    private static string START_READING = "_start";
    private static string STOP_READING = "_stop";

    public static List<Dictionary<string, string>> Read(TextAsset data)
    {
        var list = new List<Dictionary<string, string>>();

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        var read = false;
        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length > 0 && (read || values[0] == START_READING))
            {
                read = true;

                var entry = new Dictionary<string, string>();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    string value = values[j].Replace("\"\"", "\"");
                    value = UnquoteString(value);
                    value = value.Replace("\\", "");
                    entry[header[j]] = value;
                }
                list.Add(entry);

                if (values[0] == STOP_READING)
                {
                    read = false;
                }
            }
        }
        return list;
    }

    public static string UnquoteString(string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        int length = str.Length;
        if (length > 1 && str[0] == '\"' && str[length - 1] == '\"')
            str = str.Substring(1, length - 2);

        return str;
    }
}
