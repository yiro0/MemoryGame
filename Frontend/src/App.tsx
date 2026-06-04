import { useState } from "react";
import { startGame } from "./services/gameService";

export default function App() {
  const [board, setBoard] = useState(null);

  const handleStart = async () => {
    const result = await startGame(["🐶","🐶","🐱","🐱","🦊","🦊"]);
    console.log("Board from API:", result);
    setBoard(result);
  };

  return (
    <div>
      <h1>Memory Game</h1>
      <button onClick={handleStart}>Start Game</button>
      {board && <pre>{JSON.stringify(board, null, 2)}</pre>}
    </div>
  );
}