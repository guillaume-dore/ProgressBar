namespace CliProgressBar;

/// <summary>
/// Define the global layout of the progress bar.
/// </summary>
public class Layout
{
	/// <summary>
	/// Initializes a new instance of <see cref="Layout"/> class.
	/// </summary>
	/// <param name="layout">Layout of the bar component to define.</param>
	public Layout(BarLayout layout)
	{
		ArgumentNullException.ThrowIfNull(layout, nameof(layout));
		this.Bar = layout;
	}

	/// <summary>
	/// Initializes a new instance of <see cref="Layout"/> class.<br/>
	/// </summary>
	/// <param name="layout">Layout of the bar component to define.</param>
	/// <param name="text">Principal text.</param>
	/// <param name="additionalText">Optional additional text.</param>
	public Layout(BarLayout layout, Element<string> text, Element<string>? additionalText = null) : this(layout)
	{
		ArgumentNullException.ThrowIfNull(text, nameof(text));
		ArgumentNullException.ThrowIfNullOrEmpty(text.Value, nameof(text.Value));
		this.Text = text;

		if (additionalText != null)
			this.AdditionalText = additionalText;
	}

	/// <summary>
	/// Layout of the bar component.
	/// </summary>
	public BarLayout Bar { get; }

	/// <summary>
	/// Principal text to display.
	/// </summary>
	public Element<string> Text { get; set; } = new Element<string>();

	/// <summary>
	/// Additional text to display.
	/// </summary>
	public Element<string> AdditionalText { get; set; } = new Element<string>();

	/// <summary>
	/// Get total length of the textual representation of the layout.
	/// </summary>
	/// <returns>Return total length as <see cref="int"/>.</returns>
	internal int GetTextualLengthWithSpacing()
	{
		// Include minimum spacing, further add one more space per text set.
		int textLength = 1;
		if (!string.IsNullOrEmpty(this.Text.Value))
			textLength += this.Text.Value.Length + 1;
		if (!string.IsNullOrEmpty(this.AdditionalText.Value))
			textLength += this.AdditionalText.Value.Length + 1;
		if (this.Bar.BracketOptions.HasValue && this.Bar.BracketOptions.Value.HasFlag(BracketLayout.Bar))
			textLength += 2;
		return textLength;
	}

	/// <summary>
	/// Default style <see cref="Layout"/>.
	/// </summary>
	public static Layout Default => new(
		new BarLayout(
			new Element<char>('█') { ForegroundColor = ConsoleColor.Green },
			new Element<char>('░') { ForegroundColor = ConsoleColor.DarkGreen }
		)
		{ Direction = BarDirection.Reverse },
		new Element<string>("Progress:")
	);

	/// <summary>
	/// Unix style <see cref="Layout"/>.
	/// </summary>
	public static Layout Unix => new(
		new BarLayout(
			new Element<char>('#') { ForegroundColor = ConsoleColor.Green },
			new Element<char>('.') { ForegroundColor = ConsoleColor.DarkGreen }
		)
		{ BracketOptions = BracketLayout.Percentage | BracketLayout.Bar },
		new Element<string>("Progress:")
	);
}
