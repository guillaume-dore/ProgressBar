namespace CliProgressBar;

internal static class StringExtensions
{
	internal static IEnumerable<string> SplitByLength(this string str, int length)
	{
		for (int index = 0; index < str.Length; index += length)
		{
			yield return str.Substring(index, Math.Min(length, str.Length - index));
		}
	}
}
