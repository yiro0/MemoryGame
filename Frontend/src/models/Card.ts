/**
 * Zero-based grid position of a card. Mirrors the backend Position.
 */
export interface Position {
    row: number;
    column: number;
}

/**
 * A card as returned by the backend. Mirrors the backend Card.
 */
export interface Card {
    id: number;
    value: string;
    isFlipped: boolean;
    isMatched: boolean;
    position: Position;
}

