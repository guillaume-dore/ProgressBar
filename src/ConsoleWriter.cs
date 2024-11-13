using System.Text;

namespace ProgressBar;

public class ConsoleWriter(TextWriter @out) : TextWriter
{
	private List<string?> _lines = [];

	private readonly TextWriter _out = @out;

	public override Encoding Encoding => Encoding.Default;

	public override void WriteLine(string? value)
	{
		_lines.Add(value);
		_out.WriteLine(value);
	}

	public string?[] GetLines()
		=> _lines.ToArray();
}
