using System.Text;

namespace ProgressBar;

public class ConsoleWriter(TextWriter @out) : TextWriter
{
	private readonly List<string> _lines = [];

	private readonly TextWriter _out = @out;

	private bool _firstOccurenceTriggered = false;

	public override Encoding Encoding => Encoding.Default;

	public override void Write(string? value)
		=> _out.Write(value);

	public override void WriteLine(string? value)
	{
		if (value != null)
			_lines.Add(value);

		if (Console.GetCursorPosition().Top == Console.BufferHeight - 1)
		{
			Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight + 1);

			//foreach (string line in _lines)
			//	_out.WriteLine(line);
		}
		_out.WriteLine(value);
	}

	//public override void Write(string? value)
	//{
	//	if (value != null)
	//	{
	//		int? index = null;
	//		if (_lines.Count > 0)
	//		{
	//			string lastLine = _lines.Last();
	//			if (lastLine.Length < Console.BufferWidth)
	//			{
	//				if (value.Length > Console.BufferWidth - lastLine.Length)
	//				{
	//					index = Console.BufferWidth - lastLine.Length;
	//					lastLine += value.Substring(0, index.Value);
	//				}
	//			}
	//		}
	//		_lines.Add(index != null ? value.Substring(index.Value, value.Length) : value);
	//	}
	//	_out.Write(value);
	//}

	public string[] GetLines()
		=> _lines.ToArray();
}
