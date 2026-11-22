import type { Card as CardModel } from '../models/Card';

/**
 * Props for {@link Card}.
 */
interface CardProps {
  card: CardModel;
  onClick: () => void;
  disabled?: boolean;
}

/**
 * A single memory card button: shows its value when flipped or matched, a placeholder otherwise.
 * Matched cards are always disabled.
 * (Not used yet - GamePage renders its own buttons.)
 */
export function Card({ card, onClick, disabled = false }: CardProps) {
  const isRevealed = card.isFlipped || card.isMatched;

  return (
    <button
      className={`card-button ${isRevealed ? 'revealed' : ''} ${card.isMatched ? 'matched' : ''}`}
      onClick={onClick}
      disabled={disabled || card.isMatched}
      aria-label={`Card at row ${card.position.row}, column ${card.position.column}`}
    >
      <div className="card-inner">
        {isRevealed ? (
          <span className="card-value">{card.value}</span>
        ) : (
          <span className="card-back">❓</span> // Default hidden face
        )}
      </div>
    </button>
  );
}