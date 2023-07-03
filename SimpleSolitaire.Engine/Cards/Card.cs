namespace SimpleSolitaire.Engine.Cards;

public enum Suit
{
    Spades,
    Diamonds,
    Clubs,
    Hearts
}

public enum CardColor
{
    Red,
    Black
}

public class Card
{
    public int Rank { get; }
    public Suit Suit { get; }
    
    public CardColor CardColor => Suit == Suit.Spades || Suit == Suit.Clubs ? CardColor.Black : CardColor.Red;

    public Card(int rank, Suit suit)
    {
        Rank = rank;
        Suit = suit;
    }

    public override string ToString()
    {
        char rankChar = '_';
        switch (Rank)
        {
            case 13: 
                rankChar = 'K';
                break;
            case 12: 
                rankChar = 'Q';
                break;
            case 11:
                rankChar = 'J';
                break;
            case 1:
                rankChar = 'A';
                break;
            case 10:
                rankChar = 'T';
                break;
            default:
                rankChar = Rank.ToString()[0];
                break;
        }

        char suitChar = '_';
        switch (Suit)
        {
            case Suit.Spades:
                suitChar = 'S';
                break;
            case Suit.Diamonds:
                suitChar = 'D';
                break;
            case Suit.Clubs:
                suitChar = 'C';
                break;
            case Suit.Hearts:
                suitChar = 'H';
                break;
        }

        return $"{rankChar}{suitChar}";
    }
}