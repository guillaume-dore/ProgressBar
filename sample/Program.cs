namespace CliProgressBar.Sample;

internal class Program
{
	static void Main(string[] args)
	{
		DefaultLayout();
		UnixLayout();
		SetProgressWithPercentage();
		AdditionalText();
		PauseAndRestartProgressBar();
		WithConsoleOutputText();
		WithConsoleOutputTextRedirected();
	}

	static void DefaultLayout()
	{
		using var progressBar = new ProgressBar();
		for (int i = 0; i < 100; i++)
		{
			progressBar.AddSteps(1);
			Thread.Sleep(100);
		}
	}

	static void UnixLayout()
	{
		using var progressBar = new ProgressBar(Layout.Unix);
		for (int i = 0; i < 100; i++)
		{
			progressBar.AddSteps(1);
			Thread.Sleep(100);
		}
	}

	static void SetProgressWithPercentage()
	{
		using var progressBar = new ProgressBar();
		for (int i = 0; i < 100; i++)
		{
			progressBar.Report(i / 100f);
			Thread.Sleep(100);
		}
	}

	static void AdditionalText()
	{
		using var progressBar = new ProgressBar();
		for (int i = 0; i < 100; i++)
		{
			if (i % 2 == 0)
				progressBar.Report(i / 100f, $"step {i} of 100");
			else
				progressBar.AddSteps(1, $"step {i} of 100");
			Thread.Sleep(100);
		}
	}

	static void PauseAndRestartProgressBar()
	{
		using var progressBar = new ProgressBar();
		for (int i = 0; i < 100; i++)
		{
			if (i == 20)
				progressBar.Start();
			if (i == 51)
				progressBar.Stop();

			if (i == 68)
				progressBar.Start();

			progressBar.Report(i / 100f);
			Thread.Sleep(100);
		}
	}

	static void WithConsoleOutputText()
	{
		using var progressBar = new ProgressBar();
		for (int i = 0; i < 100; i++)
		{
			progressBar.WriteLine($"Line {i + 1}. Console left: {Console.GetCursorPosition().Left}, Console top: {Console.GetCursorPosition().Top}, Buffer Height: {Console.BufferHeight}, Window Height: {Console.WindowHeight}");
			progressBar.Report(i / 100f, $"step {i} of 100");
			Thread.Sleep(100);
		}
	}

	static void WithConsoleOutputTextRedirected()
	{
		using var progressBar = new ProgressBar(redirectConsoleOutput: true);
		for (int i = 0; i < 100; i++)
		{
			Console.WriteLine($"Line {i + 1}. Console left: {Console.GetCursorPosition().Left}, Console top: {Console.GetCursorPosition().Top}, Buffer Height: {Console.BufferHeight}, Window Height: {Console.WindowHeight}");
			progressBar.Report(i / 100f, $"step {i} of 100");
			Thread.Sleep(100);
		}
	}
}
