import {useState} from "react";
import type {GameBoard} from "../models/GameBoard.ts";
import {gameService} from "../services/gameService.ts";
import {DIFFICULTIES, type Difficulty} from "../core/constants.ts";

export const useGame = () => {
    const [board, setBoard] = useState<GameBoard | null>(null);
    const [moves, setMoves] = useState(0);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const startGame = async (difficulty: Difficulty) => {
        try {
            setLoading(true);
            setMoves(0);
            const pairs = DIFFICULTIES[difficulty].pairs;
            const symbols = [
                "dog", "cat", "fox", "frog",
                "panda", "lion", "tiger", "horse",
                "koala", "butterfly", "snake", "bird",
                "turtle", "cow", "pig"
            ];
            const values = symbols.slice(0, pairs).flatMap(s => [s, s]);
            const result = await gameService.start(values);
            setBoard(result);
        } catch {
            setError("Failed to start game!");
        } finally {
            setLoading(false);
        }
    };

    const flipCard = async (cardId: number) => {
        try {
            const result = await gameService.flipCard(cardId);
            setBoard(result);
            setMoves(m => m + 1);
        } catch {
            setError("Failed to flip card!");
        }
    }

    const isComplete = () =>
        board?.cards.every(c => c.isMatched) ?? false;

    return {board, moves, loading, error, startGame, flipCard, isComplete};
}