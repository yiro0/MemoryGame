# MemoryGame

MemoryGame is a work-in-progress memory matching game built with:

- **Backend:** ASP.NET Core Web API (.NET 10)
- **Frontend:** React + TypeScript + Vite

The goal of the game is simple: flip cards, find matching pairs, and complete the board in as few moves and/or as little time as possible.

## Game idea

The player sees a grid of hidden cards. Each card has a matching partner somewhere on the board. The player flips cards one by one and tries to remember where each image is located.

The visual themes/cards will likely use:
- a fixed drawn image set
- or free available online images
- or a mix of both idk...

This project is currently under construction, so the architecture and UI may change over time.

## Project structure

- `Backend/` &mdash; ASP.NET Core Web API with the game logic and score storage
- `Frontend/` &mdash; React + TypeScript + Vite app for the UI

From the repository root:

```bash
dotnet run --project Backend
```
For Frontend:
refer to [Frontend/README.md](https://github.com/yiro0/MemoryGame/blob/master/Frontend/README.md) for instructions on how to run the React app