using System.Collections.Generic;
using System.Linq;
using System;
namespace CardEngine{
	
	public class CardStorage{
		public CardCollection[] storage = new CardCollection[32];
		public CardStorage(){
			
		}
	}
	public abstract class CardCollection{
		public abstract IEnumerable<Card> AllCards();
		public abstract void Add(Card c);
		public abstract Card Remove();
		public abstract Card RemoveAt(int idx);
		public abstract void Remove(Card c);
	}	
	public class CardListCollection : CardCollection{
		public List<Card> cards = new List<Card>();
		public override IEnumerable<Card> AllCards(){
			return cards;
		}
		public override void Add(Card c){
			cards.Add(c);
		}
		public override Card Remove(){
			var ret = cards.Last();
			cards.RemoveAt(cards.Count - 1);
			return ret;
		}
		public override Card RemoveAt(int idx){
			var ret = cards[idx];
			cards.RemoveAt(idx);
			return ret;
		}
		public override void Remove(Card c){
			cards.Remove(c);
		}
	}
	public class CardStackCollection : CardCollection {
		public Stack<Card> cards = new Stack<Card>();
		public override IEnumerable<Card> AllCards(){
			return cards;
		}
		public override void Add(Card c){
			cards.Push(c);
		}
		public override Card Remove(){
			return cards.Pop();
		}
		public override Card RemoveAt(int idx){
			throw new NotImplementedException();
		}
		public override void Remove(Card c){
			throw new NotImplementedException();
		}
	}
	public class CardQueueCollection : CardCollection {
		public Queue<Card> cards = new Queue<Card>();
		
		public override IEnumerable<Card> AllCards(){
			return cards;
		}
		public override void Add(Card c){
			cards.Enqueue(c);
		}
		public override Card Remove(){
			return cards.Dequeue();
		}
		public override Card RemoveAt(int idx){
			throw new NotImplementedException();
		}
		public override void Remove(Card c){
			throw new NotImplementedException();
		}
	}
}