/**
 * Difficulty levels shown on the home page, with their pair counts.
 * Must match the backend's difficulty map in BoardFactory.
 */
export const DIFFICULTIES = {
    easy: {pairs: 6, label: "Easy "},
    medium: {pairs: 8, label: "Medium "},
    hard: {pairs: 10, label: "Hard "},
} as const;

/**
 * Difficulty key: 'easy' | 'medium' | 'hard'.
 */
export type Difficulty = keyof typeof DIFFICULTIES;