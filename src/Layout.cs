namespace ProgressBar;

/// <summary>
/// Define the global layout of the progress bar.
/// </summary>
public class Layout
{
	/// <summary>
	/// Principal text to display.
	/// </summary>
	public string Text { get; set; }

	/// <summary>
	/// Additional dynamic text to display.
	/// </summary>
	public string? AdditionalText { get; set; }

	/// <summary>
	/// FullFilled indicator <see cref="Element"/>.<br/>
	/// Define the completed style of the progress.
	/// </summary>
	public Element ProgressIndicator { get; set; }

	/// <summary>
	/// Remaining indicator <see cref="Element"/>.<br/>
	/// Define the remaining style of the progress.
	/// </summary>
	public Element PendingIndicator { get; set; }

	/// <summary>
	/// Initializes a new instance of <see cref="Layout"/> class.
	/// </summary>
	/// <param name="progressIndicator"><see cref="Element"/> indicator of completed part of the progress bar.</param>
	/// <param name="pendingIndicator"><see cref="Element"/> indicator of remaining part of the progress bar.</param>
	/// <param name="text">Principal text.</param>
	/// <param name="additionalText">Additional text.</param>
	public Layout(Element progressIndicator, Element pendingIndicator, string text, string? additionalText = null)
	{
		this.Text = text;
		this.ProgressIndicator = progressIndicator;
		this.PendingIndicator = pendingIndicator;
		this.AdditionalText = additionalText;
	}

	/// <summary>
	/// Default style <see cref="Layout"/>.
	/// </summary>
	public static Layout Default => new(
		new Element { ForegroundColor = ConsoleColor.Green, Value = '█' },
		new Element { ForegroundColor = ConsoleColor.DarkGreen, Value = '░' },
		"Progress"
	);

	/// <summary>
	/// Unix style <see cref="Layout"/>.
	/// </summary>
	public static Layout Unix => new(
		new Element { ForegroundColor = ConsoleColor.Green, Value = '#' },
		new Element { ForegroundColor = ConsoleColor.DarkGreen, Value = '.' },
		"Progress"
	);
}

/// <summary>
/// Define a console element with color.
/// </summary>
public class Element
{
	/// <summary>
	/// Text color of the element.
	/// </summary>
	public ConsoleColor ForegroundColor { get; set; }

	/// <summary>
	/// Element character.
	/// </summary>
	public char Value { get; set; }
}
