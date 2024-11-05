namespace ProgressBar;

internal class Program
{
	static void Main(string[] args)
	{
		Console.CursorVisible = false;
		Console.WriteLine(Console.WindowHeight);
		Console.WriteLine(Console.BufferHeight);
		Console.WriteLine("Start loader...");
		var cts = new CancellationTokenSource();
		string loading = "#..........";
		var loadIndicator = '\\';
		Task.Run(() =>
		{
			int cpt = 0;
			while (true)
			{
				if (cpt > 0)
				{
					Console.SetCursorPosition(0, Console.WindowHeight - 1);
					Console.Write(new string(' ', Console.BufferWidth));
					Console.SetCursorPosition(0, Console.WindowHeight - 1);
				}
				else
					Console.SetCursorPosition(0, Console.WindowHeight - 1);

				Console.Write($"[{loading}] {loadIndicator} {cpt * 10}%");

				if (cpt == loading.ToCharArray().Length - 1)
					return;
				cpt++;
				loadIndicator = loadIndicator == '\\' ? '/' : '\\';
				var charArray = loading.ToCharArray();
				charArray[cpt] = '#';
				loading = new string(charArray);
				Thread.Sleep(1000);
			}
		}, cts.Token).Wait();
	}
}
