import { Routes, Route, Navigate } from "react-router-dom";
import { HomePage } from "./pages/HomePage.tsx";
import { GamePage } from "./pages/GamePage.tsx";
import { ScorePage } from "./pages/ScorePage.tsx";

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