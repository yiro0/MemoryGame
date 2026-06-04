import {API_BASE_URL} from "../core/config.ts";
import type {Score} from "../Models/Score.ts";

export const scoreService = {
    getTop: async (limit = 10): Promise<Score[]> => {
        const res = await fetch(`${API_BASE_URL}/score/top?limit=${limit}`);
        if (!res.ok) throw new Error(`Ldeaderboard failed: ${res.status}`);
        return res.json();
    }
}