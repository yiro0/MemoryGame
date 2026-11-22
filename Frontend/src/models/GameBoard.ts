import type {Card} from "./Card.ts";

/**
 * Board state returned by every /game endpoint. Mirrors the backend GameBoard.
 */
export interface GameBoard {
    cards: Card[];
    moves: number;
}
