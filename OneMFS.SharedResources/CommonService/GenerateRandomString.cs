using System;
using System.Collections.Generic;
using System.Text;

namespace OneMFS.SharedResources.CommonService
{
	public static class GenerateRandomString
	{
		public static string GenerateRandomStringByLength()
		{
			const string AllowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz#@$^*()";
			Random rng = new Random();
			StringBuilder randomstringCode = new StringBuilder();
			foreach (string randomString in rng.NextStrings(AllowedChars, (10, 10), 1))
			{
				randomstringCode.Append(randomString);
			}
			//var randomstring = rng.NextStrings(AllowedChars, (10, 10), 1).;
			//var value = randomstring.;
			return randomstringCode.ToString();
		}


		private static IEnumerable<string> NextStrings(
	this Random rnd,
	string allowedChars,
	(int Min, int Max) length,
	int count)
		{
			ISet<string> usedRandomStrings = new HashSet<string>();
			(int min, int max) = length;
			char[] chars = new char[max];
			int setLength = allowedChars.Length;

			while (count-- > 0)
			{
				int stringLength = rnd.Next(min, max + 1);

				for (int i = 0; i < stringLength; ++i)
				{
					chars[i] = allowedChars[rnd.Next(setLength)];
				}

				string randomString = new string(chars, 0, stringLength);

				if (usedRandomStrings.Add(randomString))
				{
					yield return randomString;
				}
				else
				{
					count++;
				}
			}
		}
	}
}
