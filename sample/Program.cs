namespace ProgressBar;

internal class Program
{
	static void Main(string[] args)
	{
		using (var progressBar = new ProgressBar(false))
		{
			for (int i = 0; i < 100; i++)
			{
				if (i == 35)
				{
					progressBar.Start();
				}
				if (i == 51)
					progressBar.Stop();

				if (i == 68)
					progressBar.Start();

				progressBar.WriteLine($"Line {i + 1}. Console left: {Console.GetCursorPosition().Left}, Console top: {Console.GetCursorPosition().Top}, Buffer Height: {Console.BufferHeight}, Window Height: {Console.WindowHeight}");
				progressBar.Render(i, $"step {i} of 100");
				Thread.Sleep(200);
			}
		}


		Console.WriteLine("test");
		Console.WriteLine("test");
		Console.WriteLine("test");
		Console.WriteLine("test");
		Console.WriteLine("test");

		//Console.CursorVisible = false;
		//string loading = "#..........";
		//var loadIndicator = '\\';
		//var cts = new CancellationTokenSource();
		//Task.Run(() =>
		//{
		//	//for (int i = 0; i < 100; i++)
		//	//{
		//	//	var height = Console.WindowHeight;
		//	//	Console.WriteLine($"Task running line {i + 1} in execution...");
		//	//}
		//	int cpt = 0;
		//	while (true)
		//	{
		//		if (cpt > 0)
		//		{
		//			Console.SetCursorPosition(0, Console.WindowHeight - 1);
		//			Console.Write(new string(' ', Console.BufferWidth));
		//			Console.SetCursorPosition(0, Console.WindowHeight - 1);
		//		}
		//		else
		//			Console.SetCursorPosition(0, Console.WindowHeight - 1);

		//		Console.Write($"Task running line {cpt + 1} in execution...");
		//		Console.SetCursorPosition(0, Console.CursorTop);
		//		Console.Write($"[{loading}] {loadIndicator} {cpt * 10}%");


		//		if (cpt == loading.ToCharArray().Length - 1)
		//			return;
		//		cpt++;
		//		loadIndicator = loadIndicator == '\\' ? '/' : '\\';
		//		var charArray = loading.ToCharArray();
		//		charArray[cpt] = '#';
		//		loading = new string(charArray);
		//		Thread.Sleep(1000);
		//	}
		//}, cts.Token).Wait();
	}
}
