import { Routes, Route, Link, Navigate } from "react-router-dom";
import { HomePage } from "./pages/HomePage.tsx";
import { GamePage } from "./pages/GamePage.tsx";

function ScorePage() {
  // Placeholder TODO: replace with real scores fetching when backend endpoint available
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

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/game" element={<GamePage />} />
      <Route path="/score" element={<ScorePage />} />
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}