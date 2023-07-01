using System.Collections.Generic;
using System.Linq;

namespace SimpleSolitaire.Engine.Cards;

public delegate bool OrderingStrategy<TCard>(TCard top, TCard bottom) where TCard : ICard;

public delegate bool AcceptStrategy<TCard>(List<TCard> pileCards, List<TCard> incoming) where TCard : ICard;

public delegate bool EmptyAcceptStrategy<TCard>(List<TCard> incoming) where TCard : ICard;

public delegate bool CardAvailabilityStrategy<TCard>(List<TCard> cards, int cardIndex) where TCard : ICard;

public static class OrderingStrategies
{
    public static readonly OrderingStrategy<ICard> AlternatingColorDecrementingRank
        = (ICard top, ICard bottom) => top.CardColor != bottom.CardColor && top.Rank - bottom.Rank == 1;
    
    public static readonly OrderingStrategy<ICard> SameSuitIncrementingRank
        = (ICard top, ICard bottom) => top.Suit == bottom.Suit && top.Rank - bottom.Rank == -1;
}


public static class EmptyAcceptStrategies
{
    public static readonly EmptyAcceptStrategy<ICard> Any 
        = (List<ICard> incoming) => true;
    
    public static readonly EmptyAcceptStrategy<ICard> KingsOnly 
        = (List<ICard> incoming) => incoming.FirstOrDefault().Rank == 13;

    public static readonly EmptyAcceptStrategy<ICard> AcesOnly
        = (List<ICard> incoming) => incoming.FirstOrDefault().Rank == 1;
}