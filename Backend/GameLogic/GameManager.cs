using MemoryGame.Backend.Models;
using MemoryGame.Backend.Api.Contracts;

namespace MemoryGame.Backend.GameLogic;

/// <summary>
/// Default <see cref="IGameManager"/>: holds the single active game and enforces its rules.
/// </summary>
/// <remarks>Registered as a singleton, so all clients share one game.</remarks>
public class GameManager : IGameManager
{
    private readonly BoardFactory _boardFactory;
    private GameBoard _board;
    private DateTime _gameStartTime;
    private DateTime? _gameEndTime;
    private int _moveCount;

    /// <summary>
    /// Creates a manager with an empty board.
    /// </summary>
    /// <param name="boardFactory">Builds the board for each new game.</param>
    public GameManager(BoardFactory boardFactory)
    {
        _boardFactory = boardFactory;
        _board = new GameBoard(new List<Card>());
    }

    /// <inheritdoc />
    public GameStatus Status { get; private set; } = GameStatus.NotStarted;

    /// <inheritdoc />
    public int Moves => _moveCount;

    /// <inheritdoc />
    public GameSettings? Settings { get; private set; }

    /// <inheritdoc />
    public int ElapsedSeconds => Status == GameStatus.NotStarted
        ? 0
        : (int)((_gameEndTime ?? DateTime.UtcNow) - _gameStartTime).TotalSeconds;

    /// <inheritdoc />
    public GameBoard GetBoard() => _board;

    /// <inheritdoc />
    public void StartNewGame(GameSettings settings)
    {
        _moveCount = 0;
        _board = _boardFactory.Create(settings);
        _gameStartTime = DateTime.UtcNow;
        _gameEndTime = null;
        Settings = settings;
        Status = GameStatus.InProgress;
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
    /// An invalid reveal changes nothing, so it also leaves a pending mismatch visible.
    /// A valid reveal first hides a pending mismatch, then shows the card.
    /// The second face-up card counts one move and matches if the values are equal.
    /// </remarks>
    public RevealResult RevealCard(int cardId)
    {
        if (Status != GameStatus.InProgress)
            return new RevealResult(_board, false);

        var card = _board.Cards.FirstOrDefault(c => c.Id == cardId);
        if (card is null || card.IsMatched)
            return new RevealResult(_board, false);

        var faceUp = FaceUpCards();
        if (faceUp.Count == 2)
            _board = _board.HideCards(faceUp.Select(c => c.Id));
        else if (card.IsFlipped)
            return new RevealResult(_board, false);

        _board = _board.RevealCard(cardId);

        faceUp = FaceUpCards();
        if (faceUp.Count == 2)
        {
            _moveCount++;
            if (faceUp[0].Value == faceUp[1].Value)
                _board = _board.MatchCards(faceUp.Select(c => c.Id));
        }

        _board = new GameBoard(_board.Cards, _moveCount);

        if (!_board.Cards.All(c => c.IsMatched))
            return new RevealResult(_board, false);

        Status = GameStatus.Won;
        _gameEndTime = DateTime.UtcNow;
        return new RevealResult(_board, true);
    }

    /// <summary>Cards currently showing that are not matched yet.</summary>
    private List<Card> FaceUpCards() => _board.Cards.Where(c => c.IsFlipped && !c.IsMatched).ToList();
}
