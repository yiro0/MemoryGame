const API = "http://localhost:5000";

export const startGame = async (values: string[]) => {
  const res = await fetch(`${API}/game/start`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ values })
  });
  return res.json();
};

export const getGameState = async () => {
  const res = await fetch(`${API}/game/state`);
  return res.json();
};

export const revealCard = async (cardId: number) => {
  const res = await fetch(`${API}/game/reveal`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ cardId })
  });
  return res.json();
};

export const submitScore = async (playerName: string, moves: number, timeSeconds: number) => {
  const res = await fetch(`${API}/game/score`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ playerName, moves, timeSeconds })
  });
  return res.json();
};

export const getTopScores = async (limit = 10) => {
  const res = await fetch(`${API}/score/top?limit=${limit}`);
  return res.json();
};