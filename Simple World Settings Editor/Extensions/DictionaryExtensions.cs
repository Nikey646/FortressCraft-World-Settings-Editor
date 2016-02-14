using System;
using System.Collections.Generic;

namespace Simple.World.Settings.Editor.Extensions
{
	public static class DictionaryExtensions
	{
		public static void Add<T1, T2>(this Dictionary<T1, T2> dict, T1 value1, T2 value2, Boolean unique)
		{
			if (unique)
			{
				if (!dict.ContainsKey(value1))
					dict.Add(value1, value2);
				else dict[value1] = value2;
			}
			else
			{
				dict.Add(value1, value2);
			}
		}
	}
}
