using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using Antlr4.Runtime.Tree;
using CardEngine;

using Analytics;

namespace ParseTreeIterator
{
	public class StageIterator{
		public static void ProcessGame(RecycleParser.GameContext game){
			SetupIterator.ProcessSetup(game.setup()).ExecuteAll();
			
			for (int i = 3; i < game.ChildCount - 2; ++i){
				ProcessSubStage(game.GetChild(i));
			}
		}
		public static void ProcessStage(RecycleParser.StageContext stage){
			CardGame.Instance.PushPlayer();
			if (stage.endcondition().boolean() != null){
				TimeStep.Instance.timeStep.Push(0);
				while (!BooleanIterator.ProcessBoolean(stage.endcondition().boolean())){
					StageCount.Instance.IncCount(stage);
					TimeStep.Instance.timeStep.Push(TimeStep.Instance.timeStep.Pop() + 1);
					Debug.WriteLine("Current Player: " + CardGame.Instance.CurrentPlayer().idx);

                    CardGame.Instance.WriteToFile("t:" + CardGame.Instance.CurrentPlayer().idx);
					foreach (var player in CardGame.Instance.players) {
						Debug.WriteLine("HANDSIZE: " + player.cardBins["HAND"].Count);
					}
					for (int i = 4; i < stage.ChildCount - 1; ++i){
						TimeStep.Instance.treeLoc.Push(i - 4);
						Debug.WriteLine(TimeStep.Instance);
						ProcessSubStage(stage.GetChild(i));
						TimeStep.Instance.treeLoc.Pop();
					}
					if (stage.GetChild(2).GetText() == "player"){
						CardGame.Instance.CurrentPlayer().Next();
					}
					else if (stage.GetChild(2).GetText() == "team"){
						CardGame.Instance.CurrentTeam().Next();
					}
					
				}
				TimeStep.Instance.timeStep.Pop();
			}
			CardGame.Instance.PopPlayer();
		}
		public static void ProcessSubStage(IParseTree sub){
			if (sub is RecycleParser.MultiactionContext){
                var multiaction = sub as RecycleParser.MultiactionContext;
                if (multiaction.GetChild(1).GetText() == "choice") {
                    ProcessChoice(multiaction.condact());
                }
                else if (multiaction.GetChild(1).GetText() == "do") {
                    ActionIterator.ProcessDo(multiaction.condact());
                }
                else if (multiaction.agg() != null) {
                    VarIterator.ProcessAgg(multiaction.agg());
                }
                else if (multiaction.let() != null) {
                    VarIterator.ProcessLet(multiaction.let());
                }
                /*
                var dostatement = sub as RecycleParser.MultiactionContext;
                //TODO??
				for (int i = 0; i < multigameaction.ChildCount; ++i){
					//Console.WriteLine("gameaction");
					var gameaction = multigameaction.GetChild(i) as RecycleParser.GameactionContext;
					if (BooleanIterator.ProcessBoolean(gameaction.boolean())){
						//Console.WriteLine("bool true");
						ProcessMultiAction(gameaction.multiaction());
					}
				}*/
            }
			else if (sub is RecycleParser.StageContext){
				ProcessStage(sub as RecycleParser.StageContext);
			}
		}
		public static void ProcessChoice(RecycleParser.CondactContext[] choices){
            
			//var multigameaction = playerMoves.multigameaction()  as RecycleParser.MultigameactionContext;
			var allOptions = new List<GameActionCollection>();
			Dictionary<int, int> skips = new Dictionary<int,int>();
			for (int i = 0; i < choices.Length; ++i){
				//Console.WriteLine("gameaction");
				var gameaction = choices[i];
				//Console.WriteLine(gameaction.boolean().GetText());
				//Console.WriteLine(BooleanIterator.ProcessBoolean(gameaction.boolean()));
				int startingIdx = allOptions.Count;
				if (BooleanIterator.ProcessBoolean(gameaction.boolean())){
					allOptions.AddRange(ProcessCondactChoice(gameaction));
					for (int j = startingIdx; j < allOptions.Count; ++j) {
						skips.Add (j, i);
					}
				}

			}
			BranchingFactor.Instance.AddCount(allOptions.Count,CardGame.Instance.CurrentPlayer().idx);
			Boolean satisfied = false;
			int chosenIdx = ParseEngine.currentIterator.decisionBranch;
			int iteratorCount = ParseEngine.currentIterator.decisionIdx;
			while (!satisfied) {
				
				ParseEngine.currentIterator.decisionBranch = chosenIdx;
				ParseEngine.currentIterator.decisionIdx = iteratorCount;
				++iteratorCount;
				if (chosenIdx == -1) {
					if (allOptions.Count != 0) {
						Debug.WriteLine ("Choice count:" + allOptions.Count);
						chosenIdx = skips[CardGame.Instance.PlayerMakeChoice (allOptions, CardGame.Instance.CurrentPlayer ().idx)];
					} else {
						Debug.WriteLine ("NO Choice Available");

					}
				} else {
					allOptions.Clear ();
					allOptions.AddRange(ProcessMultiActionChoice((choices[chosenIdx] as RecycleParser.CondactContext).multiaction(),iteratorCount));
					if (allOptions.Count != 0) {
						Debug.WriteLine ("Choice count:" + allOptions.Count);
						CardGame.Instance.PlayerMakeChoice (allOptions, CardGame.Instance.CurrentPlayer ().idx);
					} else {
						Debug.WriteLine ("NO Choice Available");

					}
				}
				if (iteratorCount == (choices[chosenIdx] as RecycleParser.CondactContext).multiaction().ChildCount - 1) {
					satisfied = true;
					ParseEngine.currentIterator.decisionBranch = -1;
					ParseEngine.currentIterator.decisionIdx = -1;
				}

			}
		}

		public static List<GameActionCollection> ProcessMultiActionChoice(RecycleParser.MultiactionContext actions, int idx){
			var allOptions = new List<GameActionCollection>();
			int i = idx;
			//Console.WriteLine("action children");
			var options = ProcessActionChoice(actions.GetChild(i)  as RecycleParser.ActionContext);
			var temp = new List<GameActionCollection>();
			//if (allOptions.Count == 0){
			temp = options;
			/*}
			else{
				foreach (var additionalAction in options){
					foreach (var sourcePerm in allOptions){
						var copyPerm = new GameActionCollection();
						copyPerm.AddRange(sourcePerm);
						copyPerm.AddRange(additionalAction);
						temp.Add(copyPerm);
					}
				}
			}*/
			allOptions = temp;


			Debug.WriteLine("MultiActionChoiceCount:" + allOptions.Count);
			return allOptions;

		}

        public static List<GameActionCollection> ProcessCondactChoice(RecycleParser.CondactContext cond)
        {
            var allOptions = new List<GameActionCollection>();
            bool condition = true;
            if (cond.boolean() != null){
                condition = BooleanIterator.ProcessBoolean(cond.boolean());
            }
            if (condition){
                if (cond.multiaction() != null)
                {
                    return ProcessMultiActionChoice(cond.multiaction());
                }
                else if (cond.action() != null)
                {
                    return ProcessActionChoice(cond.action());
                }
            }
            //}
            return new List<GameActionCollection>();
        }
		public static List<GameActionCollection> ProcessMultiActionChoice(RecycleParser.MultiactionContext actions){
			var allOptions = new List<GameActionCollection> ();
			int i = 0;
			//Console.WriteLine("action children");
			var options = ProcessActionChoice (actions.GetChild (i)  as RecycleParser.ActionContext);
			var temp = new List<GameActionCollection> ();
			//if (allOptions.Count == 0){
			temp = options;
			/*}
				else{
					foreach (var additionalAction in options){
						foreach (var sourcePerm in allOptions){
							var copyPerm = new GameActionCollection();
							copyPerm.AddRange(sourcePerm);
							copyPerm.AddRange(additionalAction);
							temp.Add(copyPerm);
						}
					}
				}*/
			allOptions = temp;


			Debug.WriteLine ("MultiActionChoiceCount:" + allOptions.Count);
			return allOptions;
			
		}
		public static List<GameActionCollection> ProcessActionChoice(RecycleParser.ActionContext action){
			//Console.WriteLine("Execute:");
			//Console.WriteLine(action.GetText());
			var opts = ActionIterator.ProcessAction(action);
			//Console.WriteLine("orig:" + opts.Count);
			var flatten = new List<GameActionCollection>();
			foreach (var a in opts){
				if (a is FancyCardMoveAction){
					var fa = a as FancyCardMoveAction;
					if (fa.startLocation.locIdentifier == "any"){
						foreach (var card in fa.startLocation.cardList.AllCards()){
							flatten.Add(new GameActionCollection{
								new CardMoveAction(card,fa.startLocation.cardList,fa.endLocation.cardList)
							});
						}	
					}
					else{
						
						/*flatten.Add(new GameActionCollection{
							fa
						});*/
						flatten.Add(opts);
						break;
					}
				}
				else{
					flatten.Add(new GameActionCollection{a});
				}
			}
			Debug.WriteLine("Flatten:" + flatten.Count);
			return flatten;
		}
	}
}