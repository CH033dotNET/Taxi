using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Tools
{
	public static class Extention
	{
		public static IQueryable<T> TakeLast<T>(this IQueryable<T> source, int N)
		{
			var count = source.Count();
			if (N > count)
			{
				return source;
			}
			else
			{
				return source.Skip(Math.Max(0, source.Count() - N)).Take(N);
			}
			
		}
	}
}
