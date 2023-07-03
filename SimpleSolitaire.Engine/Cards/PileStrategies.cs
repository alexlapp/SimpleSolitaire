using System.Collections.Generic;
using System.Linq;

namespace SimpleSolitaire.Engine.Cards;

public delegate bool OrderingStrategy(Card top, Card bottom);

public delegate bool AcceptStrategy(IEnumerable<Card> pileCards, IEnumerable<Card> incoming);

public delegate bool EmptyAcceptStrategy(IEnumerable<Card> incoming);

public delegate bool CardAvailabilityStrategy(IEnumerable<Card> cards, int cardIndex);

public static class OrderingStrategies
{
    public static readonly OrderingStrategy AlternatingColorDecrementingRank
        = (Card top, Card bottom) => top.CardColor != bottom.CardColor && top.Rank - bottom.Rank == 1;
    
    public static readonly OrderingStrategy SameSuitIncrementingRank
        = (Card top, Card bottom) => top.Suit == bottom.Suit && top.Rank - bottom.Rank == -1;
}


public static class EmptyAcceptStrategies
{
    public static readonly EmptyAcceptStrategy Any 
        = (IEnumerable<Card> incoming) => true;
    
    public static readonly EmptyAcceptStrategy KingsOnly 
        = (IEnumerable<Card> incoming) => incoming.FirstOrDefault()?.Rank == 13;

    public static readonly EmptyAcceptStrategy AcesOnly
        = (IEnumerable<Card> incoming) => incoming.FirstOrDefault()?.Rank == 1;
}