namespace ProgressBar;

/// <summary>
/// Initializes a new instance of <see cref="Element{T}"/> class.
/// </summary>
/// <param name="value">Value of <see cref="Element{T}"/></param>
/// <param name="foregroundColor">Font color to assign to <see cref="Element{T}"/></param>
/// <param name="backgroundColor">Background color to assign to <see cref="Element{T}"/></param>
public class Element<T>(T value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
{
	/// <summary>
	/// Color of the element.
	/// </summary>
	public ConsoleColor? ForegroundColor { get; set; } = foregroundColor;

	/// <summary>
	/// Background color of the element.
	/// </summary>
	public ConsoleColor? BackgroundColor { get; set; } = backgroundColor;

	/// <summary>
	/// Element value.
	/// </summary>
	public T Value { get; set; } = value;
}
