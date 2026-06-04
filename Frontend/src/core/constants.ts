export const DIFFICULTIES = {
    easy: {pairs: 6, label: "Easy "},
    medium: {pairs: 10, label: "Medium "},
    hard: {pairs: 15, label: "Hard "},
} as const;

export type Difficulty = keyof typeof DIFFICULTIES;