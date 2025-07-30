namespace MemoryGame.Backend.gameLogic;

public class GameManager
{
    public List<Card> cardGrid;

    public GameManager(int rows, int columns)
    {
        cardGrid = new List<Card>();
        // for (int i =0; i < rows ; i++)
        // {
        //     List<Card> row = new List<Card>();
        //     for (int j = 0;j<columns;j++) {
        //         row.Add(new Card() {id = i * columns + j, value = "A", isFlipped = false, isMatched = false});
        //     }
        //     cardGrid.Add(row);
        // 
    }
}