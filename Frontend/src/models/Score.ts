/**
 * A leaderboard entry. Mirrors the backend Score.
 */
export interface Score {
    id: number;
    playerName: string;
    moves: number;
    timeSeconds: number;
    createdAt?: string;
}
