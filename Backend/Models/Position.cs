namespace MemoryGame.Backend.Models;

/// <summary>
/// Zero-based row and column of a card in the board grid.
/// </summary>
public class Position
{
    /// <summary>
    /// Zero-based row index.
    /// </summary>
    public int Row { get; }

    /// <summary>
    /// Zero-based column index.
    /// </summary>
    public int Column { get; }

    /// <summary>
    /// Creates a grid position.
    /// </summary>
    /// <param name="row">Zero-based row index.</param>
    /// <param name="column">Zero-based column index.</param>
    public Position(int row, int column)
    {
        Row = row;
        Column = column;
    }
}
