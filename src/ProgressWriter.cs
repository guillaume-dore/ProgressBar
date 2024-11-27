using System.Text;

namespace CliProgressBar;

internal class ProgressWriter : TextWriter
{
	private readonly ProgressBar _progressBar;

	internal ProgressWriter(ProgressBar progressBar)
	{
		this._progressBar = progressBar;
	}

	public override Encoding Encoding => Encoding.Default;

	public override void WriteLine()
		=> this._progressBar.WriteLine(null);

	public override void WriteLine(string? value)
		=> this._progressBar.WriteLine(value);
}
