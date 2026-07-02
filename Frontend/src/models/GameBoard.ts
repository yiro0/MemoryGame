import type {Card} from "./Card.ts";

export interface GameBoard {
    cards: Card[];
    moves: number;
}
