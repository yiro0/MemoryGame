export interface Position {
    row: number;
    column: number;
}

export interface Card {
    id: number;
    value: string;
    isFlipped: boolean;
    isMatched: boolean;
    position: Position;
}

