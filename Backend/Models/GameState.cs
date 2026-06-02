using MemoryGame.Backend.GameLogic;

namespace MemoryGame.Backend.Models;

public class GameState
{
    public IReadOnlyList<GameBoard> Boards { get; init; } = [];
    public bool IsGameOver { get; init; }
    public int Moves  { get; init; }
    public int Score  { get; init; }
}