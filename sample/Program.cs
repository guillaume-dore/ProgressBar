namespace ProgressBar;

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
			var percentProgress = (i / 100.0) * 100;
			if (i % 2 == 0)
				progressBar.Percentage = percentProgress;
			else
				progressBar.Report(percentProgress);
			Thread.Sleep(100);
		}
	}

	static void AdditionalText()
	{
		using var progressBar = new ProgressBar();
		for (int i = 0; i < 100; i++)
		{
			var percentProgress = (i / 100.0) * 100;
			if (i % 2 == 0)
				progressBar.Report(percentProgress, $"step {i} of 100");
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
			{
				progressBar.Start();
			}
			if (i == 51)
				progressBar.Stop();

			if (i == 68)
				progressBar.Start();

			var percentProgress = (i / 100.0) * 100;
			if (i % 2 == 0)
				progressBar.Percentage = percentProgress;
			else
				progressBar.Report(percentProgress);
			Thread.Sleep(100);
		}
	}

	static void WithConsoleOutputText()
	{
		using var progressBar = new ProgressBar();
		for (int i = 0; i < 100; i++)
		{
			var percentProgress = (i / 100.0) * 100;
			progressBar.WriteLine($"Line {i + 1}. Console left: {Console.GetCursorPosition().Left}, Console top: {Console.GetCursorPosition().Top}, Buffer Height: {Console.BufferHeight}, Window Height: {Console.WindowHeight}");
			progressBar.Report(percentProgress, $"step {i} of 100");
			Thread.Sleep(100);
		}
	}
}
