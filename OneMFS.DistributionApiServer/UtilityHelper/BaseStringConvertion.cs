using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMFS.DistributionApiServer.UtilityHelper
{
	public interface IBaseStringConvertion
	{
		bool IsBase64String(string base64);
	}
	public class BaseStringConvertion:IBaseStringConvertion
	{
		public bool IsBase64String(string base64)
		{
			Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
			return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
		}
	}
}
