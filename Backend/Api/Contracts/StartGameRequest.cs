namespace MemoryGame.Backend.Api.Contracts;

public record StartGameRequest(
    GameSettingsRequest? Settings = null,
    string? Difficulty = null
);

public record GameSettingsRequest(
    string? Difficulty = null,
    int? PairCount = null,
    IReadOnlyList<string>? Values = null,
    int? TimeLimitSeconds = null,
    string? PlayerName = null
);