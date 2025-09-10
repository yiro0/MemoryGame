using MemoryGame.Backend.models;

namespace MemoryGame.Backend.gameLogic;

public class IGameManager
{
    GameBoard GetBoard();
    void StartNewGame(GameSettings settings);
    GameBoard FlipCard(int cardId);
    bool IsComplete();
}