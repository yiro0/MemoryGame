import { useState } from "react";
import {gameService} from "./services/gameService.ts";
import {DEFAULT_CARD_VALUES} from "./core/config.ts"
import type {GameBoard} from "./Models/GameBoard.ts";

export default function App() {
  const [board, setBoard] = useState<GameBoard | null>(null);

  const handleStart = async () => {
    if (board !== null) return;
    const result = await gameService.start(DEFAULT_CARD_VALUES);
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