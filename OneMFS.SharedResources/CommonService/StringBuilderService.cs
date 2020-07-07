using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace OneMFS.SharedResources.CommonService
{
    public class StringBuilderService
    {
        public string CreateRandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }

        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public string GenerateMD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        public string GenerateSha1Hash(string value)
        {
            var data = Encoding.ASCII.GetBytes(value);
            var hashData = new SHA1Managed().ComputeHash(data);
            var hash = string.Empty;
            foreach (var b in hashData)
            {
                hash += b.ToString("X2");
            }
            return hash;
        }

		//To extract text in between from and to 
	    public string ExtractText(string strSource, string strStart, string strEnd)
	    {
		    int Start, End;
			char[] trimChars = { ':','"','*', '@', ' ' };
		    if (strSource.Contains(strStart) && strSource.Contains(strEnd))
		    {
			    Start = strSource.IndexOf(strStart, 0) + strStart.Length;
			    End = strSource.IndexOf(strEnd, Start);
			    return strSource.Substring(Start, End - Start).Trim(trimChars);
		    }
		    else
		    {
			    return "";
		    }
	    }
	}
}
