using System.Runtime.CompilerServices;

namespace Techaria.Systems.TransferDuct;

public class TransferNetwork
{
    private readonly HashSet<Duct> _ducts = [];

    /// <summary>
    ///     Adds or removes a duct based on its current existence in the network.
    ///     If the duct exists, it will be removed; otherwise, it will be added.
    /// </summary>
    /// <param name="duct">The duct to toggle.</param>
    /// <returns>True if the duct was added; false if it was removed.</returns>
    public bool Toggle(in Duct duct)
    {
        if (!_ducts.Add(duct))
        {
            _ducts.Remove(duct);
            return false;
        }
        return true;
    }

    /// <summary>
    ///     Finds the nearest active duct to the specified position.
    /// </summary>
    /// <param name="x">The horizontal position to search from.</param>
    /// <param name="y">The vertical position to search from.</param>
    /// <returns>The nearest active duct, or null if no active ducts exist.</returns>
    public Duct? FindNearest(int x, int y)
    {
        Duct? nearestDuct = null;
        double minDistanceSquared = double.MaxValue;

        foreach (var duct in _ducts)
        {
            if (!duct.IsActive) continue;

            int dx = duct.X - x;
            int dy = duct.Y - y;
            double distanceSquared = dx * dx + dy * dy;

            if (distanceSquared < minDistanceSquared)
            {
                minDistanceSquared = distanceSquared;
                nearestDuct = duct;
            }
        }
        return nearestDuct;
    }

    /// <summary>
    ///     Attempts to add a duct to the network without redunancy checks.
    ///     This is useful for bulk operations where duplicates are already filtered.
    /// </summary>
    /// <param name="duct">The duct to add.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddUnchecked(in Duct duct)
        => _ducts.Add(duct);

    /// <summary>
    ///     Removes all inactive ducts from the network.
    /// </summary>
    public void CleanInactive()
        => _ducts.RemoveWhere(d => !d.IsActive);

    /// <summary>
    ///     Gets total number of ducts in the network.
    /// </summary>
    public int Count => _ducts.Count;

    /// <summary>
    ///     Gets an enumerable collection of all active ducts.
    /// </summary>
    public IEnumerable<Duct> ActiveDucts
        => _ducts.Where(d => d.IsActive);

    public HashSet<Duct>.Enumerator GetEnumerator() => _ducts.GetEnumerator();
}