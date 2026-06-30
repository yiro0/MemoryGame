export const DIFFICULTIES = {
    easy: {pairs: 6, label: "Easy "},
    medium: {pairs: 8, label: "Medium "},
    hard: {pairs: 10, label: "Hard "},
} as const;

export type Difficulty = keyof typeof DIFFICULTIES;