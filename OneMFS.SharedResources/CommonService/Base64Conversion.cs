using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OneMFS.SharedResources.CommonService
{
	public class Base64Conversion
	{
		public  bool IsBase64(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return false;
			}
			if ((str.Length % 4) != 0)
			{
				return false;
			}			
			try
			{
				string decoded = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(str));
				string encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(decoded));
				if (str.Equals(encoded, StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
			}
			catch { }
			return false;
		}
		public string DecodeBase64(string encodedString)
		{
			byte[] data = Convert.FromBase64String(encodedString);
			string decodedString = Encoding.UTF8.GetString(data);
			return decodedString;
		}
		public  string EncodeBase64(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}
		public  bool IsLetterEnglish(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return false;
			}
			str = str.Replace(" ", string.Empty);
			foreach (char ch in str)
			{
				if (!(ch >= 'A' && ch <= 'Z') && !(ch >= 'a' && ch <= 'z') && !(ch >= '0' && ch <= '9'))
				{
					return false;
				}
			}
			return true;
		}
	}
}
