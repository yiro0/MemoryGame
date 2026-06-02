using MemoryGame.Backend.Models;

namespace MemoryGame.Backend.GameLogic;

public interface IGameManager
{
    GameBoard GetBoard();
    void StartNewGame(GameSettings settings);
    GameBoard FlipCard(int cardId);
    bool IsComplete();
}