import {useState, useEffect} from "react";
import type {GameBoard} from "../models/GameBoard.ts";
import {gameService} from "../services/gameService.ts";
import {DIFFICULTIES, type Difficulty} from "../core/constants.ts";

export const useGame = () => {
    const [board, setBoard] = useState<GameBoard | null>(null);
    const [moves, setMoves] = useState(0);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [isProcessing, setIsProcessing] = useState(false);
    const [isAutoFlipping, setIsAutoFlipping] = useState(false);

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

    const flipCard = async (cardId: number, shouldCountAsMove: boolean = true) => {
        try {
            setIsProcessing(true);
            const result = await gameService.flipCard(cardId);
            setBoard(result);
            if (shouldCountAsMove) {
                setMoves(m => m + 1);
            }
        } catch {
            setError("Failed to flip card!");
        } finally {
            setIsProcessing(false);
        }
    };

    useEffect(() => {
        if (!board) return;

        const flipped = board.cards.filter(c => c.isFlipped && !c.isMatched);
        
        if (flipped.length === 2) {
            if (flipped[0].value !== flipped[1].value) {
                setIsAutoFlipping(true);
                const timer = setTimeout(async () => {
                    try {
                        await gameService.flipCard(flipped[0].id);
                        const updatedBoard = await gameService.flipCard(flipped[1].id);
                        setBoard(updatedBoard);
                        setIsAutoFlipping(false);
                    } catch {
                        setError("Failed to auto-flip cards!");
                        setIsAutoFlipping(false);
                    }
                }, 1500);

                return () => {
                    clearTimeout(timer);
                    setIsAutoFlipping(false);
                };
            }
        }
    }, [board]);

    const isComplete = () =>
        board?.cards.every(c => c.isMatched) ?? false;

    return {board, moves, loading, error, startGame, flipCard, isComplete, isProcessing: isProcessing || isAutoFlipping};
}