using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Antlr4.Runtime.Tree;
using CardEngine;

namespace ParseTreeIterator
{
	public class DeckIterator{
		public static Tree ProcessDeck(CardLanguageParser.DeckContext deck){
			var attributeCount = deck.ChildCount - 3;
			List<Node> childs = new List<Node>();
			for (int i = 0; i < attributeCount; ++i){
				childs.Add(new Node{
					Value="combo"+i,
					children=ProcessAttribute(deck.attribute(i))
				});
			}
			return new Tree{
				rootNode= new Node{
					Value="Attrs",
					children = childs
				}
			};
		}
		
		public static List<Node> ProcessAttribute(CardLanguageParser.AttributeContext attr){
			if (attr.attribute()[0].attribute().Count() == 0){
				//base case
				var terminalTitle = attr.trueany()[0];
				var subNode = attr.attribute()[0] as CardLanguageParser.AttributeContext;
				var ret = new List<Node>();
				var trueCount = (subNode.ChildCount - 3)/2 + 1;
				for (int i = 0; i < trueCount; ++i){
					ret.Add(new Node{
						Key=terminalTitle.GetText(),
						Value=subNode.trueany(i).GetText()
					});
				}
				return ret;
				
				
			}
			else{
				var ret = new List<Node>();
				var terminalTitle = attr.trueany()[0];
				var children = attr.attribute();
				
				foreach (var subNode in children){
					var childs = new List<Node>();
					foreach (var att in subNode.attribute()){
						childs.AddRange(ProcessAttribute(att));
					}
					ret.Add(new Node{
						Key=terminalTitle.GetText(),
						Value=subNode.trueany()[0].GetText(),
						children=childs
					});
					
				}
				return ret;
			}
		}
		
	}
}