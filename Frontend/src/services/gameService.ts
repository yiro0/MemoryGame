import {API_BASE_URL} from "../core/config.ts";
import type {GameBoard} from "../Models/GameBoard.ts";

export const gameService = {
    start: async (values: string[]): Promise<GameBoard> => {
        const res = await fetch(`${API_BASE_URL}/game/start`, {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({values}),
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

// export const submitScore = async (playerName: string, moves: number, timeSeconds: number) => {
//     const res = await fetch(`${API}/game/score`, {
//         method: "POST",
//         headers: {"Content-Type": "application/json"},
//         body: JSON.stringify({playerName, moves, timeSeconds})
//     });
//     return res.json();
// };

// export const getTopScores = async (limit = 10) => {}
//     const res = await fetch(`${API}/score/top?limit=${limit}`);
//     return res.json();
// },