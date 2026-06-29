import { Card } from './Card';
import type {GameBoard as GameBoardType } from "../models/GameBoard.ts";

interface gameBoardProps {
    board: GameBoardType | null;
    onCardClick: (cardId: number) => void;
    gameWon?: boolean;
}

export function GameBoard({ board, onCardClick, gameWon }: gameBoardProps) {
if (!board) {
    return (
      <div className="board-loading">
        <p>Loading cards...</p>
        {/* CSS spinner or sth...*/}
      </div>
    );
  }

  return (
    <div className="board-grid">
      {board.cards.map((card) => (
        <Card
          key={card.id}
          card={card}
          onClick={() => onCardClick(card.id)}
          disabled={card.isMatched || gameWon} 
        />
      ))}
    </div>
  );
}
