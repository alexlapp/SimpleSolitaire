namespace SimpleSolitaire.MonoGame.Cards;

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

public struct Card
{
    public int Rank { get; }
    public Suit Suit { get; }
    public CardColor CardColor => Suit == Suit.Spades || Suit == Suit.Clubs ? CardColor.Black : CardColor.Red;

    public Card(int rank, Suit suit)
    {
        Rank = rank;
        Suit = suit;
    }
}