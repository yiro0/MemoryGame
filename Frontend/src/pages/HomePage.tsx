import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { DIFFICULTIES, type Difficulty } from '../core/constants.ts';
import styles from './HomePage.module.css';

export function HomePage() {
  const navigate = useNavigate();
  const [selectedDifficulty, setSelectedDifficulty] = useState<Difficulty>('medium');

  const handleStart = () => {
    navigate('/game', { state: { difficulty: selectedDifficulty } });
  };

  return (
    <div className={styles.container}>
      <div className={styles.content}>
        <h1 className={styles.title}>🎮 Memory Game</h1>
        <p className={styles.subtitle}>Match pairs of cards to win!</p>

        <div className={styles.difficultySection}>
          <h2 className={styles.sectionTitle}>Select Difficulty</h2>
          
          <div className={styles.difficultyOptions}>
            {(Object.entries(DIFFICULTIES) as [Difficulty, typeof DIFFICULTIES[Difficulty]][]).map(
              ([key, difficulty]) => (
                <button
                  key={key}
                  className={`${styles.difficultyButton} ${
                    selectedDifficulty === key ? styles.selected : ''
                  }`}
                  onClick={() => setSelectedDifficulty(key)}
                >
                  <span className={styles.difficultyLabel}>{difficulty.label}</span>
                  <span className={styles.difficultyInfo}>({difficulty.pairs * 2} cards)</span>
                </button>
              )
            )}
          </div>
        </div>

        <div className={styles.infoSection}>
          <p className={styles.infoText}>
            {selectedDifficulty === 'easy' && `${DIFFICULTIES.easy.pairs} pairs of cards - Perfect for beginners!`}
            {selectedDifficulty === 'medium' && `${DIFFICULTIES.medium.pairs} pairs of cards - A good challenge!`}
            {selectedDifficulty === 'hard' && `${DIFFICULTIES.hard.pairs} pairs of cards - For the experts!`}
          </p>
        </div>

        <button className={styles.startButton} onClick={handleStart}>
          Start Game
        </button>

        <div className={styles.rulesSection}>
          <h3 className={styles.rulesTitle}>How to Play</h3>
          <ul className={styles.rulesList}>
            <li>Click on a card to reveal its symbol</li>
            <li>Click on another card to find its match</li>
            <li>Matched pairs stay revealed</li>
            <li>Match all pairs to win!</li>
          </ul>
        </div>
      </div>
    </div>
  );
}
