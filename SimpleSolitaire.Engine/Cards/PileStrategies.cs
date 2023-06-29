using System.Collections.Generic;
using System.Linq;

namespace SimpleSolitaire.Engine.Cards;

public delegate bool OrderingStrategy(Card top, Card bottom);

public delegate bool AcceptStrategy(List<Card> pileCards, List<Card> incoming);

public delegate bool EmptyAcceptStrategy(List<Card> incoming);

public delegate bool CardAvailabilityStrategy(List<Card> cards, int cardIndex);

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
        = (List<Card> incoming) => true;
    
    public static readonly EmptyAcceptStrategy KingsOnly 
        = (List<Card> incoming) => incoming.FirstOrDefault().Rank == 13;

    public static readonly EmptyAcceptStrategy AcesOnly
        = (List<Card> incoming) => incoming.FirstOrDefault().Rank == 1;
}