using MemoryGame.Backend.DataAccess.Interfaces;
using MemoryGame.Backend.Models;
using MemoryGame.Backend.Api.Contracts;

namespace MemoryGame.Backend.GameLogic;

public class GameManager : IGameManager
{
    private readonly IScoreRepository _scoreRepo;
    private readonly BoardFactory _boardFactory;
    private GameBoard _board;
    private DateTime _gameStartTime;
    private int _moveCount;

    public GameManager(IScoreRepository scoreRepo, BoardFactory boardFactory)
    {
        _scoreRepo = scoreRepo;
        _boardFactory = boardFactory;
        _board = new GameBoard(new List<Card>());
    }

    public GameBoard GetBoard() => _board;

    public void StartNewGame(GameSettings settings)
    {
        _moveCount = 0;
        _board = _boardFactory.Create(settings);
        _gameStartTime = DateTime.UtcNow;
    }

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

    public bool IsComplete() => _board.Cards.All(c => c.IsMatched);
}
