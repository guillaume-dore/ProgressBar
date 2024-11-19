namespace ProgressBar;

public class ProgressBar : IDisposable
{
	public ProgressBar()
	{
		Console.CursorVisible = false;
	}

	public void Dispose()
	{
		Console.CursorVisible = true;
		GC.SuppressFinalize(this);
	}

	public void WriteLine(string? value)
	{
		if (value == null)
			return;

		int valueTop = Console.GetCursorPosition().Top;
		if (valueTop == Console.BufferHeight - 1)
		{
			Console.SetCursorPosition(0, Console.BufferHeight - 1);
			Console.Write(Environment.NewLine);
			//Console.WriteLine(new string(' ', Console.BufferWidth));
			var cursorPosition = Console.GetCursorPosition();
			Console.SetCursorPosition(0, valueTop - 1);
			Console.Write(value);
		}
		else
			Console.WriteLine(value);
	}

	public void Render(int i)
	{
		int topPosition = Console.GetCursorPosition().Top;
		Console.SetCursorPosition(0, Console.BufferHeight - 1);
		//Console.Write(new string(' ', Console.BufferWidth));
		Console.SetCursorPosition(0, Console.BufferHeight - 1);
		Console.Write($"Action in progress... {i} / 100");
		Console.SetCursorPosition(0, topPosition);
	}
}
