namespace ProgressBar;

/// <summary>
/// 
/// </summary>
public class Layout
{
	/// <summary>
	/// 
	/// </summary>
	public string Text { get; set; }

	/// <summary>
	/// 
	/// </summary>
	public string? AdditionalText { get; set; }

	/// <summary>
	/// 
	/// </summary>
	public Element ProgressIndicator { get; set; }

	/// <summary>
	/// 
	/// </summary>
	public Element PendingIndicator { get; set; }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="progressIndicator"></param>
	/// <param name="pendingIndicator"></param>
	/// <param name="text"></param>
	/// <param name="additionalText"></param>
	public Layout(Element progressIndicator, Element pendingIndicator, string text, string? additionalText = null)
	{
		this.Text = text;
		this.ProgressIndicator = progressIndicator;
		this.PendingIndicator = pendingIndicator;
		this.AdditionalText = additionalText;
	}

	/// <summary>
	/// 
	/// </summary>
	public static Layout Default => new(
		new Element { ForegroundColor = ConsoleColor.Green, Value = '█' },
		new Element { ForegroundColor = ConsoleColor.DarkGreen, Value = '░' },
		"Progress"
	);

	/// <summary>
	/// 
	/// </summary>
	public static Layout Unix => new(
		new Element { ForegroundColor = ConsoleColor.Green, Value = '#' },
		new Element { ForegroundColor = ConsoleColor.DarkGreen, Value = '.' },
		"Progress"
	);
}

/// <summary>
/// 
/// </summary>
public class Element
{
	/// <summary>
	/// 
	/// </summary>
	public ConsoleColor ForegroundColor { get; set; }

	/// <summary>
	/// 
	/// </summary>
	public char Value { get; set; }
}
