namespace MemoryGame.Backend.GameLogic;

/// <summary>
/// Outcome of a card reveal.
/// </summary>
/// <param name="Board">The board after the reveal (unchanged if the reveal was ignored).</param>
/// <param name="JustWon"><c>true</c> only for the reveal that cleared the board.</param>
public record RevealResult(GameBoard Board, bool JustWon);
