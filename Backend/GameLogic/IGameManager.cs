using MemoryGame.Backend.Api.Contracts;
using MemoryGame.Backend.Models;

namespace MemoryGame.Backend.GameLogic;

public interface IGameManager
{
    GameBoard GetBoard();
    // TODO: Roll back from difficulty-based start once frontend is updated to use GameSettings
    // Properly implement difficulty-based part of backend
    void StartNewGame(GameSettings settings);
    void StartNewGame(StartGameRequest settings);
    GameBoard FlipCard(int cardId);
    bool IsComplete();
}