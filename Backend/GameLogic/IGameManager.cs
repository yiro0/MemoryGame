using MemoryGame.Backend.Api.Contracts;
using MemoryGame.Backend.Models;

namespace MemoryGame.Backend.GameLogic;

/// <summary>
/// Runs a single game: starts it, applies card reveals and reports progress.
/// The only component allowed to change game state.
/// </summary>
public interface IGameManager
{
    /// <summary>
    /// Returns the current board (empty before the first game starts).
    /// </summary>
    GameBoard GetBoard();

    // TODO: Roll back from difficulty-based start once frontend is updated to use GameSettings
    // Properly implement difficulty-based part of backend

    /// <summary>
    /// Starts a new game, replacing any game in progress.
    /// </summary>
    /// <param name="settings">Configuration for the new board.</param>
    void StartNewGame(GameSettings settings);

    /// <summary>
    /// Starts a new game from an API request, mapping it to <see cref="GameSettings"/>.
    /// </summary>
    /// <param name="settings">The request sent to <c>POST /game/start</c>.</param>
    void StartNewGame(StartGameRequest settings);

    /// <summary>
    /// Flips a card and applies the matching rules; returns the updated board.
    /// </summary>
    /// <param name="cardId">Id of the card to flip.</param>
    GameBoard FlipCard(int cardId);

    /// <summary>
    /// Returns <c>true</c> when every card on the board is matched.
    /// </summary>
    bool IsComplete();
}
