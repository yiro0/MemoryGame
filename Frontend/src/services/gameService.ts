import {API_BASE_URL} from "../core/config.ts";
import type {GameBoard} from "../models/GameBoard.ts";

export const gameService = {
    start: async (difficulty: string): Promise<GameBoard> => {
        const res = await fetch(`${API_BASE_URL}/game/start`, {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({ difficulty }),
        });
        if (!res.ok) throw Error(`Start failed: ${res.status}`);
        return res.json();
    },

    getBoard: async (): Promise<GameBoard> => {
        const res = await fetch(`${API_BASE_URL}/game/state`);
        if (!res.ok) throw Error(`Get state failed: ${res.status}`);
        return res.json();
    },

    flipCard: async (cardId: number): Promise<GameBoard> => {
        const res = await fetch(`${API_BASE_URL}/game/reveal`, {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({cardId})
        });
        if (!res.ok) throw Error(`Flip failed: ${res.status}`);
        return res.json(
        )
    },

    submitScore: async (playerName: string, moves: number, timeSeconds: number): Promise<void> => {
        const res = await fetch(`${API_BASE_URL}/game/score`, {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({playerName, moves, timeSeconds}),
        });
        if (!res.ok) throw Error(`Submit score failed: ${res.status}`);
    },
};