import { Link } from "react-router-dom";

export function ScorePage() {
  // Placeholder TODO: replace with real scores fetching (scoreService + ScoreDisplay)
  return (
    <div>
      <h2>Score</h2>
      <p>No scores yet.</p>
      <p>
        <Link to="/">Back to Home</Link>
      </p>
    </div>
  );
}
