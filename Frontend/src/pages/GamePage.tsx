import { useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { useGame } from '../hooks/useGame.ts';
import type { Difficulty } from '../core/constants.ts';
import styles from './GamePage.module.css';

export function GamePage() {
  const location = useLocation();
  const navigate = useNavigate();
  const { board, moves, loading, error, startGame, flipCard, isComplete, isProcessing } = useGame();

  const difficulty: Difficulty = (location.state?.difficulty) || 'medium';

  useEffect(() => {
    startGame(difficulty);
  }, [difficulty]);

  const handleCardClick = (cardId: number) => {
    const clickedCard = board?.cards.find(card => card.id === cardId);

    if (!clickedCard || clickedCard.isFlipped || clickedCard.isMatched || isProcessing) {
      return;
    }
    flipCard(cardId);
  };

  const handlePlayAgain = () => {
    startGame(difficulty);
  };

  const handleBackHome = () => {
    navigate('/');
  };

  if (loading && !board) {
    return (
      <div className={styles.container}>
        <div className={styles.loadingMessage}>Loading game...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className={styles.container}>
        <div className={styles.errorMessage}>
          <p>❌ {error}</p>
          <button className={styles.button} onClick={handlePlayAgain}>
            Try Again
          </button>
          <button className={styles.secondaryButton} onClick={handleBackHome}>
            Back Home
          </button>
        </div>
      </div>
    );
  }

  const gameWon = isComplete();

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h1 className={styles.title}>Memory Game</h1>
        <div className={styles.stats}>
          <div className={styles.statItem}>
            <span className={styles.label}>Moves:</span>
            <span className={styles.value}>{moves}</span>
          </div>
          <div className={styles.statItem}>
            <span className={styles.label}>Matched:</span>
            <span className={styles.value}>
              {board?.cards.filter(c => c.isMatched).length || 0} / {board?.cards.length || 0}
            </span>
          </div>
        </div>
      </div>

      {gameWon && (
        <div className={styles.victoryMessage}>
          <h2>🎉 You Won!</h2>
          <p>You completed the game in {moves} moves!</p>
        </div>
      )}

      <div className={styles.boardContainer}>
        <div className={styles.board}>
          {board?.cards.map((card) => (
            <button
              key={card.id}
              className={`${styles.card} ${card.isFlipped || card.isMatched ? styles.revealed : ''} ${
                card.isMatched ? styles.matched : ''
              }`}
              onClick={() => handleCardClick(card.id)}
              disabled={card.isMatched || gameWon}
              aria-label={`Card ${card.id + 1}: ${card.isFlipped || card.isMatched ? card.value : 'hidden'}`}
            >
              <span className={styles.cardContent}>
                {(card.isFlipped || card.isMatched) && card.value}
              </span>
            </button>
          ))}
        </div>
      </div>

      <div className={styles.actions}>
        {gameWon ? (
          <>
            <button className={styles.button} onClick={handlePlayAgain}>
              Play Again
            </button>
            <button className={styles.secondaryButton} onClick={handleBackHome}>
              Home
            </button>
          </>
        ) : (
          <>
            <button className={styles.secondaryButton} onClick={handlePlayAgain}>
              Reset Game
            </button>
            <button className={styles.secondaryButton} onClick={handleBackHome}>
              Back Home
            </button>
          </>
        )}
      </div>
    </div>
  );
}
