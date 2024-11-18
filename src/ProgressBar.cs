namespace ProgressBar;

public class ProgressBar : IDisposable
{
	private readonly ConsoleWriter _stream;

	public ProgressBar()
	{
		_stream = new ConsoleWriter(Console.Out);
		Console.SetOut(_stream);
		Console.CursorVisible = false;
	}

	public void Dispose()
	{
		var standardOutput = new StreamWriter(Console.OpenStandardOutput())
		{
			AutoFlush = true
		};
		Console.SetOut(standardOutput);
		GC.SuppressFinalize(this);
	}

	public void Render(int i)
	{
		int topPosition = Console.GetCursorPosition().Top;
		Console.SetCursorPosition(0, Console.BufferHeight - 1);
		Console.Write(new string(' ', Console.BufferWidth));
		Console.SetCursorPosition(0, Console.BufferHeight - 1);
		Console.Write($"Action in progress... {i} / 100");
		Console.SetCursorPosition(0, topPosition);
	}
}
