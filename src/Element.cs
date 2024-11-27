namespace CliProgressBar;

/// <summary>
/// Define an element of the progress bar with his value and his color.
/// </summary>
public class Element<T>
{
	/// <summary>
	/// Default Element ctor.
	/// </summary>
	public Element() { }

	/// <summary>
	/// Initializes a new instance of <see cref="Element{T}"/> class.
	/// </summary>
	/// <param name="value">Value of <see cref="Element{T}"/></param>
	/// <param name="foregroundColor">Font color to assign to <see cref="Element{T}"/></param>
	/// <param name="backgroundColor">Background color to assign to <see cref="Element{T}"/></param>
	public Element(T value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
	{
		this.Value = value;
		this.ForegroundColor = foregroundColor;
		this.BackgroundColor = backgroundColor;
	}

	/// <summary>
	/// Color of the element.
	/// </summary>
	public ConsoleColor? ForegroundColor { get; set; }

	/// <summary>
	/// Background color of the element.
	/// </summary>
	public ConsoleColor? BackgroundColor { get; set; }

	/// <summary>
	/// Element value.
	/// </summary>
	public T? Value { get; set; }
}
