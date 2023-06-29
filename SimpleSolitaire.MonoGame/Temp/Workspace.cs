using System.Collections.Generic;
using System.Linq;

namespace SimpleSolitaire.MonoGame.Temp;

// Sample card struct for examples below
public struct Card
{
    public int Rank { get; }
    public int Suite { get; }

    public Card(int rank, int suite)
    {
        Rank = rank;
        Suite = suite;
    }
}

// Define delegate types for each strategy the Pile will need to implement
public delegate bool OrderingFunction(Card top, Card bottom);
public delegate bool AcceptFunction(List<Card> pileCards, List<Card> incoming);

// Define concrete strategies within a static class, like so
public static class OrderingStrategies
{
    public static readonly OrderingFunction SomeDefaultImplementation = (t, b) => t.Rank - b.Rank == 1;
}

// This is a sample implementation of the Pile class
public class Pile
{
    // Define private backing fields for cards and strategies
    private List<Card> _cards;
    private OrderingFunction _orderingFunction;
    private AcceptFunction _acceptFunction;

    // Expose the public interface, not the backing functions directly. This let's us pass the internal set of cards when
    //  relevant, instead of needing to do something like this: `pile.AcceptFunction(pile.Cards, other.Cards);`. 
    //  We can do the same in a cleaner way: `pile.CanAccept(other)`;
    public List<Card> Cards => this._cards;
    public bool CanAccept(Pile incomingPile) => _acceptFunction(_cards, incomingPile.Cards);

    // In the constructor we can set the parameters as nullable when there is room for a default implementation, and
    //  set the default implementation based off the required strategies
    public Pile(
        OrderingFunction orderingFunction, 
        AcceptFunction? acceptFunction = null)
    {
        _cards = new List<Card>();
        _orderingFunction = orderingFunction;
        _acceptFunction = acceptFunction ?? ((pileCards, incoming) => _orderingFunction(pileCards.Last(), incoming.First()));
    }
}

// Sample PileBuilder class, can be used to make many Piles with certain parameters
public class PileBuilder
{
    private OrderingFunction _orderingFunction;
    private AcceptFunction _acceptFunction;

    public PileBuilder()
    {
        Reset();
    }

    public void Reset()
    {
        _orderingFunction = OrderingStrategies.SomeDefaultImplementation;
        _acceptFunction = (pile, incoming) => _orderingFunction(pile.Last(), incoming.First());
    }

    public Pile Build()
    {
        return new Pile(
            orderingFunction: _orderingFunction,
            acceptFunction: _acceptFunction);
    }
}

// Then the end result: We can create piles with the strategies we care about, and use the public interface to write game logic
public static class Test
{
    public static void RunSomething()
    {
        var pileBuilder = new PileBuilder();
        var pileOne = pileBuilder.Build();
        var pileTwo = pileBuilder.Build();

        pileOne.CanAccept(pileTwo);
    }
}