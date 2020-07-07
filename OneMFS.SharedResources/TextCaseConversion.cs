using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneMFS.SharedResources
{
    public class TextCaseConversion
    {
        public string ToPascalCase(string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }


        public string EscapeInvalidCharacter(object strSource)
        {
            return strSource.ToString().Replace("'", "''");
        }
    }
}
