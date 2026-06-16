export const DIFFICULTIES = {
    easy: {pairs: 5, label: "Easy "},
    medium: {pairs: 9, label: "Medium "},
    hard: {pairs: 14, label: "Hard "},
} as const;

export type Difficulty = keyof typeof DIFFICULTIES;