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
    /// Current stage of the game.
    /// </summary>
    GameStatus Status { get; }

    /// <summary>
    /// Moves made in the current game.
    /// </summary>
    int Moves { get; }

    /// <summary>
    /// Settings of the current game; <c>null</c> before the first game.
    /// </summary>
    GameSettings? Settings { get; }

    /// <summary>
    /// Seconds since the game started, frozen once it is won.
    /// </summary>
    int ElapsedSeconds { get; }

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
    /// Reveals a card and applies the game rules. Invalid reveals change nothing.
    /// </summary>
    /// <param name="cardId">Id of the card to reveal.</param>
    RevealResult RevealCard(int cardId);
}
