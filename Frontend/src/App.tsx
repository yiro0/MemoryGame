import { useState } from "react";
import { BrowserRouter, Routes, Route, Link, Navigate } from "react-router-dom";
import { gameService } from "./services/gameService.ts";
import { DEFAULT_CARD_VALUES } from "./core/config.ts";
import type { GameBoard } from "./models/GameBoard.ts";

function HomePage() {
  return (
    <div>
      <h1>Memory Game</h1>
      <p>Welcome! Play the classic memory matching game.</p>
      <nav>
        <Link to="/game">Play</Link>
        {" | "}
        <Link to="/score">Score</Link>
      </nav>
    </div>
  );
}

function GamePage() {
  const [board, setBoard] = useState<GameBoard | null>(null);

  const handleStart = async () => {
    if (board !== null) return;
    const result = await gameService.start(DEFAULT_CARD_VALUES);
    console.log("Board from API:", result);
    setBoard(result);
  };

  return (
    <div>
      <h2>Game</h2>
      <button onClick={handleStart}>Start Game</button>
      {board && <pre>{JSON.stringify(board, null, 2)}</pre>}
      <p>
        <Link to="/">Back to Home</Link>
      </p>
    </div>
  );
}

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
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/game" element={<GamePage />} />
        <Route path="/score" element={<ScorePage />} />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  );
}