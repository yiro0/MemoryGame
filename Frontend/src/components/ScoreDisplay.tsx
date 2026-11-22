import type { Score } from '../models/Score';

/**
 * Props for {@link ScoreDisplay}.
 */
interface ScoreDisplayProps {
  scores: Score[];
}

/**
 * Leaderboard table of scores in the given order, or an empty-state message.
 * (Not used yet - waiting for the real ScorePage.)
 */
export function ScoreDisplay({ scores }: ScoreDisplayProps) {
  if (!scores || scores.length === 0) {
    return (
      <div className="score-empty">
        <p>No scores available yet. Be the first to play!</p>
      </div>
    );
  }

  return (
    <div className="score-display">
      <h2>Leaderboard</h2>
      <table className="score-table">
        <thead>
          <tr>
            <th>Rank</th>
            <th>Player</th>
            <th>Moves</th>
            <th>Time</th>
            <th>Date</th>
          </tr>
        </thead>
        <tbody>
          {scores.map((score, index) => (
            <tr key={score.id}>
              <td>#{index + 1}</td>
              <td className="player-name">{score.playerName}</td>
              <td>{score.moves}</td>
              <td>{score.timeSeconds}s</td>
              <td>
                {score.createdAt
                  ? new Date(score.createdAt).toLocaleDateString()
                  : 'N/A'}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}