using MemoryGame.Backend.DataAccess.Interfaces;
using MemoryGame.Backend.Models;
using MemoryGame.Backend.Api.Contracts;

namespace MemoryGame.Backend.GameLogic;

/// <summary>
/// Default <see cref="IGameManager"/>: holds the single active game, counts moves,
/// matches pairs and saves the score when the board is cleared.
/// </summary>
/// <remarks>
/// Registered as a singleton, so all clients share one game.
/// Known gaps (fixed in Phase 4, step 3): flipping toggles instead of revealing,
/// and the score is saved again on every flip after a win.
/// </remarks>
public class GameManager : IGameManager
{
    private readonly IScoreRepository _scoreRepo;
    private readonly BoardFactory _boardFactory;
    private GameBoard _board;
    private DateTime _gameStartTime;
    private int _moveCount;

    /// <summary>
    /// Creates a manager with an empty board.
    /// </summary>
    /// <param name="scoreRepo">Where finished-game scores are saved.</param>
    /// <param name="boardFactory">Builds the board for each new game.</param>
    public GameManager(IScoreRepository scoreRepo, BoardFactory boardFactory)
    {
        _scoreRepo = scoreRepo;
        _boardFactory = boardFactory;
        _board = new GameBoard(new List<Card>());
    }

    /// <inheritdoc />
    public GameBoard GetBoard() => _board;

    /// <inheritdoc />
    public void StartNewGame(GameSettings settings)
    {
        _moveCount = 0;
        _board = _boardFactory.Create(settings);
        _gameStartTime = DateTime.UtcNow;
    }

    /// <inheritdoc />
    /// <remarks>Nested <see cref="StartGameRequest.Settings"/> values win over the top-level difficulty.</remarks>
    public void StartNewGame(StartGameRequest settings)
    {
        var requestSettings = settings?.Settings;
        var mappedSettings = new GameSettings(
            requestSettings?.Difficulty ?? settings?.Difficulty ?? BoardFactory.DefaultDifficulty)
        {
            PairCount = requestSettings?.PairCount,
            Values = requestSettings?.Values,
            TimeLimitSeconds = requestSettings?.TimeLimitSeconds,
            PlayerName = requestSettings?.PlayerName
        };

        StartNewGame(mappedSettings);
    }

    /// <inheritdoc />
    /// <remarks>
    /// When two unmatched cards are face up, one move is counted and equal values are matched.
    /// Clearing the board saves a score.
    /// </remarks>
    public GameBoard FlipCard(int cardId)
    {
        _board = _board.FlipCard(cardId);
        var flipped = _board.Cards.Where(c => c.IsFlipped && !c.IsMatched).ToList();
        if (flipped.Count ==2)
        {
            _moveCount++;
            if (flipped[0].Value == flipped[1].Value)
            {
                _board = _board.MatchCards(flipped.Select(c => c.Id));
            }
        }

        if (IsComplete())
        {
            _scoreRepo?.SaveScore(new Models.Score
            {
                // TODO: Login feature and login credentials
                PlayerName = "Yerho",
                Moves = _moveCount,
                TimeSeconds = (int)(DateTime.UtcNow - _gameStartTime).TotalSeconds,
            });
        }

        _board = new GameBoard(_board.Cards, _moveCount);

        return _board;
    }

    /// <inheritdoc />
    /// <remarks>Also <c>true</c> for an empty board, i.e. before the first game.</remarks>
    public bool IsComplete() => _board.Cards.All(c => c.IsMatched);
}
