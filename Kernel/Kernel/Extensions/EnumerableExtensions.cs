using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kernel.Extensions
{
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Aggregates the specified lines in one.
		/// </summary>
		/// <param name="source">The sourse.</param>
		/// <returns></returns>
		public static string Aggregate(this IEnumerable<string> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			var sb = new StringBuilder();

			var aggragatedMessage = source.Aggregate(sb, (b, next) => b.AppendLine(next)).ToString();

			return aggragatedMessage;
		}

		/// <summary>
		/// Fors the each.
		/// </summary>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="sequence">The sequence.</param>
		/// <param name="action">The action.</param>
		/// <exception cref="System.ArgumentNullException">sequence</exception>
		public static void ForEach<TItem>(this IEnumerable<TItem> sequence, Action<TItem> action)
		{
			if (sequence == null)
				throw new ArgumentNullException("sequence");

			if (action == null)
				return;

			foreach (TItem obj in sequence)
				action(obj);
		}
	}
}