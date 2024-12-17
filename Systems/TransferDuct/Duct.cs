namespace Techaria.Systems.TransferDuct;

/// <summary>
///     Represents a single transfer duct in the system.
/// </summary>
/// <param name="x">The horizontal position of the duct within the transfer network grid.</param>
/// <param name="y">The vertical position of the duct within the transfer network grid.</param>
/// <param name="isActive">Indicates whether the duct is active or inactive.</param>
public readonly struct Duct(int x, int y, bool isActive = false)
{
    /// <summary>
    ///     Gets the horizontal position of duct within the transfer network grid.
    /// </summary>
    public int X { get; } = x;

    /// <summary>
    ///     Gets the vertical position of duct within the transfer network grid.
    /// </summary>
    public int Y { get; } = y;

    /// <summary>
    ///     Gets a value indicating whether the duct is active.
    /// </summary>
    public bool IsActive { get; } = isActive;

    /// <summary>
    ///     Creates a new instance of <see cref="Duct"/> structure with the specified active status.
    /// </summary>
    /// <param name="isActive">The new active status of the duct.</param>
    /// <returns>A new <see cref="Duct"/> with updated active status.</returns>
    public Duct WithStatus(bool isActive)
        => new(X, Y, isActive);

    /// <summary>
    ///     Calculates the Euclidian distance between two ducts.
    /// </summary>
    /// <param name="a">The first duct.</param>
    /// <param name="b">The second duct.</param>
    /// <returns>The distance between two ducts.</returns>
    public static double Distance(in Duct a, in Duct b)
    {
        int dx = a.X - b.X;
        int dy = a.Y - b.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public override string ToString()
        => $"Duct [X: {X}, Y: {Y}, Active: {IsActive}]";
}