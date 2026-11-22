import { Card } from './Card';
import type {GameBoard as GameBoardType } from "../models/GameBoard.ts";

/**
 * Props for {@link GameBoard}.
 */
interface gameBoardProps {
    board: GameBoardType | null;
    onCardClick: (cardId: number) => void;
    gameWon?: boolean;
}

/**
 * Grid of {@link Card} components for the given board, or a loading message while it is null.
 * (Not used yet - GamePage renders its own grid.)
 */
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
