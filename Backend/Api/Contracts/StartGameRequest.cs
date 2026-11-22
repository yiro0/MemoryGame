namespace MemoryGame.Backend.Api.Contracts;

/// <summary>
/// Request body of <c>POST /game/start</c>.
/// </summary>
/// <param name="Settings">Optional detailed settings; its values take precedence over <paramref name="Difficulty"/>.</param>
/// <param name="Difficulty">Shorthand difficulty (<c>easy</c>, <c>medium</c>, <c>hard</c>) used when <paramref name="Settings"/> has none.</param>
public record StartGameRequest(
    GameSettingsRequest? Settings = null,
    string? Difficulty = null
);

/// <summary>
/// Detailed game configuration sent by the client inside <see cref="StartGameRequest"/>.
/// Every field is optional; missing or invalid values fall back to defaults.
/// </summary>
/// <param name="Difficulty">Difficulty name (<c>easy</c>, <c>medium</c>, <c>hard</c>).</param>
/// <param name="PairCount">Explicit number of pairs; overrides the difficulty when in range.</param>
/// <param name="Values">Custom card values; topped up from the built-in symbol pool if too few.</param>
/// <param name="TimeLimitSeconds">Time limit for the game (not enforced yet).</param>
/// <param name="PlayerName">Name stored with the score (not used yet).</param>
public record GameSettingsRequest(
    string? Difficulty = null,
    int? PairCount = null,
    IReadOnlyList<string>? Values = null,
    int? TimeLimitSeconds = null,
    string? PlayerName = null //TODO: login feature
);
