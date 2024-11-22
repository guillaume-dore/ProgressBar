namespace ProgressBar;

/// <summary>
/// Initializes a new instance of <see cref="BarLayout"/> class.<br/>
/// Define the progression layout.
/// </summary>
/// <param name="progressIndicator"><see cref="Element{Char}"/> indicator of completed part of the progress bar.</param>
/// <param name="pendingIndicator"><see cref="Element{Char}"/> indicator of remaining part of the progress bar.</param>
public class BarLayout(Element<char> progressIndicator, Element<char> pendingIndicator)
{
	/// <summary>
	/// FullFilled indicator <see cref="Element{Char}"/>.<br/>
	/// Define the completed style of the progress.
	/// </summary>
	/// <exception cref="ArgumentNullException">Throw when the value is null.</exception>
	public Element<char> ProgressIndicator { get; init; } = progressIndicator ?? throw new ArgumentNullException(nameof(progressIndicator));

	/// <summary>
	/// Remaining indicator <see cref="Element{Char}"/>.<br/>
	/// Define the remaining style of the progress.
	/// </summary>
	/// <exception cref="ArgumentNullException">Throw when the value is null.</exception>
	public Element<char> PendingIndicator { get; init; } = pendingIndicator ?? throw new ArgumentNullException(nameof(pendingIndicator));

	/// <summary>
	/// Direction of the bar, defining display of percentage before or after displaying the progression.
	/// </summary>
	public BarDirection Direction { get; init; } = BarDirection.Forward;

	/// <summary>
	/// Position of the bar in the parent layout.
	/// </summary>
	public LayoutPosition Position { get; init; } = LayoutPosition.Center;

	/// <summary>
	/// Define the brackets to display around the bar and or percentage.<br/>
	/// If value is <c>null</c> there is no brackets displayed.
	/// </summary>
	public BracketLayout? BracketOptions { get; init; } = null;
}

/// <summary>
/// Position Enumeration.
/// </summary>
public enum LayoutPosition { Left, Center, Right }

/// <summary>
/// Direction Enumeration.
/// </summary>
public enum BarDirection { Forward, Reverse }

/// <summary>
/// Brackets Enumeration.
/// </summary>
[Flags]
public enum BracketLayout { Percentage = 1, Bar = 2 }