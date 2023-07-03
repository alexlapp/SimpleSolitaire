using System.Collections.Generic;
using System.Drawing;

namespace SimpleSolitaire.Engine.Cards;

public enum PileFlow
{
    Stack,
    Downwards,
    Rightwards,
}

public class Pile<TCard> where TCard : Card
{
    protected List<TCard> _cards;
    private OrderingStrategy _orderingStrategy;
    private AcceptStrategy _acceptFunction;
    private EmptyAcceptStrategy _emptyAcceptStrategy;
    private CardAvailabilityStrategy _cardAvailabilityStrategy;

    public List<TCard> Cards => _cards;
    
    public PileFlow PileFlow { get; }

    public bool CanAcceptPile(Pile<TCard> incoming)  =>
        _cards.Any() ? _acceptFunction(_cards, incoming.Cards) : _emptyAcceptStrategy(incoming.Cards);

    public bool IsCardAvailable(int cardIndex) => _cardAvailabilityStrategy(_cards, cardIndex);

    public Pile(OrderingStrategy orderingStrategy, PileFlow pileFlow = PileFlow.Stack, AcceptStrategy? acceptFunction = null, EmptyAcceptStrategy? emptyAcceptStrategy = null, CardAvailabilityStrategy? cardAvailabilityStrategy = null)
    {
        _cards = new List<TCard>();

        PileFlow = pileFlow;
        _orderingStrategy = orderingStrategy;
        _emptyAcceptStrategy = emptyAcceptStrategy ?? ((IEnumerable<Card> card) => true);
        _acceptFunction = acceptFunction ?? ((IEnumerable<Card> pileCards, IEnumerable<Card> incoming) =>
        {
            if (!(pileCards.Any() || !incoming.Any())) return false;

            return _orderingStrategy(pileCards.Last(), incoming.First());
        });
        _cardAvailabilityStrategy = cardAvailabilityStrategy ?? ((IEnumerable<Card> cards, int cardIndex) =>
        {
            if (cardIndex < 0 || cardIndex >= cards.Count()) return false;
            if (cardIndex == cards.Count() - 1) return true;

            for (int i = cardIndex; i < cards.Count() - 1; i++)
            {
                if (!_orderingStrategy(cards.ElementAt(i), cards.ElementAt(i + 1)))
                    return false;
            }

            return true;
        });
    }

    public Pile(Pile<TCard> other)
    {
        _cards = other.Cards;
        PileFlow = other.PileFlow;
        _orderingStrategy = other._orderingStrategy;
        _emptyAcceptStrategy = other._emptyAcceptStrategy;
        _acceptFunction = other._acceptFunction;
        _cardAvailabilityStrategy = other._cardAvailabilityStrategy;
    }

    public virtual void AddCard(TCard card)
    {
        Cards.Add(card);
    }

    public List<TCard> TakeCardsFromIndex(int index)
    {
        if (index < 0 || index >= _cards.Count) throw new ArgumentException("Index out of range");

        var result = new List<TCard>();

        var cardCount = _cards.Count - index;
        foreach (var card in _cards.GetRange(index, cardCount))
        {
            result.Add(card);
        }
        
        _cards.RemoveRange(index, cardCount);

        return result;
    }

    public virtual Pile<TCard> PileFromIndex(int index)
    {
        if (index < 0 || index >= _cards.Count) throw new ArgumentException("Index out of range");

        var resultPile = new Pile<TCard>(this);
        resultPile._cards = TakeCardsFromIndex(index);

        return resultPile;
    }
}