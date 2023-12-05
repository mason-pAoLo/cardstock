using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CardEngine;
using CardStockXam.Scoring;
using FreezeFrame;
using Players;


// focus on game actions and other easy stuff
namespace CardStockXam.games.testing
{
    public class Enchere
    {
        public CardGame cg;
        public Transcript script;
        public GameActionCollection gameActionList;
        public CardLocReference locations;
        public RecycleVariables variables;
        public World gameWorld;

        public static void Main()
        {
            Console.WriteLine("Enchere Testing");

            // unit tests
            Enchere e = new Enchere();
            e.PlayersAndTeamsTest();
            e.DeckCreationTest();
            e.ShuffleTest();
            e.DealTest();
            e.MoveCardToAwardPileTest();
            e.PlayCardTest();
            e.CreatePrecedencePointMapTest();
            e.DetermineTrickWinner();
            e.CreateScoringPointMapTest();
            e.ScoringTest();

            Console.ReadLine();
        }

        public Enchere()
        {
            // setup
            variables = new RecycleVariables();
            gameActionList = new GameActionCollection();
            script = new Transcript(true, "games/testing/EnchereCS");
            cg = new CardGame();
            gameWorld = new World();

        }
            /*
            void game()
            {
                init();
                Shuffle();
                Deal();
                // DetermineTrickWinner();
                // StageGame();
                // StagePlayer();
                // Score();
            }
            */

            void PlayersAndTeamsTest()
            {
                // creating players
                Console.WriteLine("Creating players.");
                var numPlayers = 3;
                script.WriteToFile("nump:" + numPlayers);
                cg.AddPlayers(numPlayers, null);
                script.WriteToFile("t: " + cg.currentPlayer.Peek().CurrentName());
                gameWorld.numPlayers = numPlayers;
                
                // assigning players to their teams
                Console.WriteLine("Assigning players to teams");
                var teamList = new List<List<int>>();
                var numTeams = cg.players.Length;
                for (int i = 0; i < numTeams; i++)
                {
                    teamList.Add(new List<int>() { i });
                }
                gameActionList.Add(new TeamCreateAction(teamList, cg, script));
                
                Console.WriteLine("Executing game actions");
                gameActionList.ExecuteAll();
                gameActionList.Clear();
            }

            void DeckCreationTest()
            {
                // creating decks
                Console.WriteLine("Creating decks.");
                Tree cashDeck = new Tree
                {
                    rootNode = new Node
                    {
                        Key = "RANK",
                        children = new List<Node>()
                        {
                            new Node
                            {
                                Key = "RANK",
                                Value = "TWO",
                                children = new List<Node>()
                                {
                                    new Node
                                    {
                                        Key = "COLOR",
                                        Value = "RED",
                                        children = new List<Node>()
                                        {
                                            new Node
                                            {
                                                Key = "SUIT",
                                                Value = "HEARTS",
                                                children = {}
                                            },
                                            new Node
                                            {
                                                Key = "SUIT",
                                                Value = "DIAMONDS",
                                                children = {}
                                            }
                                        }
                                    },
                                    new Node
                                    {
                                        Key = "COLOR",
                                        Value = "BLACK",
                                        children = new List<Node>()
                                        {
                                           new Node
                                            {
                                                Key = "SUIT",
                                                Value = "SPADES",
                                                children = {}
                                            },
                                            new Node
                                            {
                                                Key = "SUIT",
                                                Value = "CLUBS",
                                                children = {}
                                            }
                                        }
                                    },
                                    new Node
                                    {
                                        Key = "RANK",
                                        Value = "THREE",
                                        children = new List<Node>()
                                        {
                                            new Node
                                            {
                                                Key = "COLOR",
                                                Value = "RED",
                                                children = new List<Node>()
                                                {
                                                    new Node
                                                    {
                                                        Key = "SUIT",
                                                        Value = "HEARTS",
                                                        children = {}
                                                    },
                                                    new Node
                                                    {
                                                        Key = "SUIT",
                                                        Value = "DIAMONDS",
                                                        children = {}
                                                    }
                                                }
                                            },
                                            new Node
                                            {
                                                Key = "COLOR",
                                                Value = "BLACK",
                                                children = new List<Node>()
                                                {
                                                   new Node
                                                    {
                                                        Key = "SUIT",
                                                        Value = "SPADES",
                                                        children = {}
                                                    },
                                                    new Node
                                                    {
                                                        Key = "SUIT",
                                                        Value = "CLUBS",
                                                        children = {}
                                                    }
                                                }
                                            },
                                            new Node
                                            {
                                                Key = "RANK",
                                                Value = "FOUR",
                                                children = new List<Node>()
                                                {
                                                    new Node
                                                    {
                                                        Key = "COLOR",
                                                        Value = "RED",
                                                        children = new List<Node>()
                                                        {
                                                            new Node
                                                            {
                                                                Key = "SUIT",
                                                                Value = "HEARTS",
                                                                children = {}
                                                            },
                                                            new Node
                                                            {
                                                                Key = "SUIT",
                                                                Value = "DIAMONDS",
                                                                children = {}
                                                            }
                                                        }
                                                    },
                                                    new Node
                                                    {
                                                        Key = "COLOR",
                                                        Value = "BLACK",
                                                        children = new List<Node>()
                                                        {
                                                           new Node
                                                            {
                                                                Key = "SUIT",
                                                                Value = "SPADES",
                                                                children = {}
                                                            },
                                                            new Node
                                                            {
                                                                Key = "SUIT",
                                                                Value = "CLUBS",
                                                                children = {}
                                                            }
                                                        }
                                                    },
                                                    new Node
                                                    {
                                                        Key = "RANK",
                                                        Value = "FIVE",
                                                        children = new List<Node>()
                                                        {
                                                            new Node
                                                            {
                                                                Key = "COLOR",
                                                                Value = "RED",
                                                                children = new List<Node>()
                                                                {
                                                                    new Node
                                                                    {
                                                                        Key = "SUIT",
                                                                        Value = "HEARTS",
                                                                        children = {}
                                                                    },
                                                                    new Node
                                                                    {
                                                                        Key = "SUIT",
                                                                        Value = "DIAMONDS",
                                                                        children = {}
                                                                    }
                                                                }
                                                            },
                                                            new Node
                                                            {
                                                                Key = "COLOR",
                                                                Value = "BLACK",
                                                                children = new List<Node>()
                                                                {
                                                                    new Node
                                                                    {
                                                                        Key = "SUIT",
                                                                        Value = "SPADES",
                                                                        children = {}
                                                                    },
                                                                    new Node
                                                                    {
                                                                        Key = "SUIT",
                                                                        Value = "CLUBS",
                                                                        children = {}
                                                                    }
                                                                }
                                                            },
                                                            new Node
                                                            {
                                                                Key = "RANK",
                                                                Value = "SIX",
                                                                children = new List<Node>()
                                                                {
                                                                    new Node
                                                                    {
                                                                        Key = "COLOR",
                                                                        Value = "RED",
                                                                        children = new List<Node>()
                                                                        {
                                                                            new Node
                                                                            {
                                                                                Key = "SUIT",
                                                                                Value = "HEARTS",
                                                                                children = {}
                                                                            },
                                                                            new Node
                                                                            {
                                                                                Key = "SUIT",
                                                                                Value = "DIAMONDS",
                                                                                children = {}
                                                                            }
                                                                        }
                                                                    },
                                                                    new Node
                                                                    {
                                                                        Key = "COLOR",
                                                                        Value = "BLACK",
                                                                        children = new List<Node>()
                                                                        {
                                                                           new Node
                                                                            {
                                                                                Key = "SUIT",
                                                                                Value = "SPADES",
                                                                                children = {}
                                                                            },
                                                                            new Node
                                                                            {
                                                                                Key = "SUIT",
                                                                                Value = "CLUBS",
                                                                                children = {}
                                                                            }
                                                                        }
                                                                    },
                                                                    new Node
                                                                    {
                                                                        Key = "RANK",
                                                                        Value = "SEVEN",
                                                                        children = new List<Node>()
                                                                        {
                                                                            new Node
                                                                            {
                                                                                Key = "COLOR",
                                                                                Value = "RED",
                                                                                children = new List<Node>()
                                                                                {
                                                                                    new Node
                                                                                    {
                                                                                        Key = "SUIT",
                                                                                        Value = "HEARTS",
                                                                                        children = {}
                                                                                    },
                                                                                    new Node
                                                                                    {
                                                                                        Key = "SUIT",
                                                                                        Value = "DIAMONDS",
                                                                                        children = {}
                                                                                    }
                                                                                }
                                                                            },
                                                                            new Node
                                                                            {
                                                                                Key = "COLOR",
                                                                                Value = "BLACK",
                                                                                children = new List<Node>()
                                                                                {
                                                                                   new Node
                                                                                    {
                                                                                        Key = "SUIT",
                                                                                        Value = "SPADES",
                                                                                        children = {}
                                                                                    },
                                                                                    new Node
                                                                                    {
                                                                                        Key = "SUIT",
                                                                                        Value = "CLUBS",
                                                                                        children = {}
                                                                                    }
                                                                                }
                                                                            },
                                                                            new Node
                                                                            {
                                                                                Key = "RANK",
                                                                                Value = "EIGHT",
                                                                                children = new List<Node>()
                                                                                {
                                                                                    new Node
                                                                                    {
                                                                                        Key = "COLOR",
                                                                                        Value = "RED",
                                                                                        children = new List<Node>()
                                                                                        {
                                                                                            new Node
                                                                                            {
                                                                                                Key = "SUIT",
                                                                                                Value = "HEARTS",
                                                                                                children = {}
                                                                                            },
                                                                                            new Node
                                                                                            {
                                                                                                Key = "SUIT",
                                                                                                Value = "DIAMONDS",
                                                                                                children = {}
                                                                                            }
                                                                                        }
                                                                                    },
                                                                                    new Node
                                                                                    {
                                                                                        Key = "COLOR",
                                                                                        Value = "BLACK",
                                                                                        children = new List<Node>()
                                                                                        {
                                                                                           new Node
                                                                                            {
                                                                                                Key = "SUIT",
                                                                                                Value = "SPADES",
                                                                                                children = {}
                                                                                            },
                                                                                            new Node
                                                                                            {
                                                                                                Key = "SUIT",
                                                                                                Value = "CLUBS",
                                                                                                children = {}
                                                                                            }
                                                                                        }
                                                                                    },
                                                                                    new Node
                                                                                    {
                                                                                        Key = "RANK",
                                                                                        Value = "NINE",
                                                                                        children = new List<Node>()
                                                                                        {
                                                                                            new Node
                                                                                            {
                                                                                                Key = "COLOR",
                                                                                                Value = "RED",
                                                                                                children = new List<Node>()
                                                                                                {
                                                                                                    new Node
                                                                                                    {
                                                                                                        Key = "SUIT",
                                                                                                        Value = "HEARTS",
                                                                                                        children = {}
                                                                                                    },
                                                                                                    new Node
                                                                                                    {
                                                                                                        Key = "SUIT",
                                                                                                        Value = "DIAMONDS",
                                                                                                        children = {}
                                                                                                    }
                                                                                                }
                                                                                            },
                                                                                            new Node
                                                                                            {
                                                                                                Key = "COLOR",
                                                                                                Value = "BLACK",
                                                                                                children = new List<Node>()
                                                                                                {
                                                                                                   new Node
                                                                                                    {
                                                                                                        Key = "SUIT",
                                                                                                        Value = "SPADES",
                                                                                                        children = {}
                                                                                                    },
                                                                                                    new Node
                                                                                                    {
                                                                                                        Key = "SUIT",
                                                                                                        Value = "CLUBS",
                                                                                                        children = {}
                                                                                                    }
                                                                                                }
                                                                                            },
                                                                                            new Node
                                                                                            {
                                                                                                Key = "RANK",
                                                                                                Value = "TEN",
                                                                                                children = new List<Node>()
                                                                                                {
                                                                                                    new Node
                                                                                                    {
                                                                                                        Key = "COLOR",
                                                                                                        Value = "RED",
                                                                                                        children = new List<Node>()
                                                                                                        {
                                                                                                            new Node
                                                                                                            {
                                                                                                                Key = "SUIT",
                                                                                                                Value = "HEARTS",
                                                                                                                children = {}
                                                                                                            },
                                                                                                            new Node
                                                                                                            {
                                                                                                                Key = "SUIT",
                                                                                                                Value = "DIAMONDS",
                                                                                                                children = {}
                                                                                                            }
                                                                                                        }
                                                                                                    },
                                                                                                    new Node
                                                                                                    {
                                                                                                        Key = "COLOR",
                                                                                                        Value = "BLACK",
                                                                                                        children = new List<Node>()
                                                                                                        {
                                                                                                           new Node
                                                                                                            {
                                                                                                                Key = "SUIT",
                                                                                                                Value = "SPADES",
                                                                                                                children = {}
                                                                                                            },
                                                                                                            new Node
                                                                                                            {
                                                                                                                Key = "SUIT",
                                                                                                                Value = "CLUBS",
                                                                                                                children = {}
                                                                                                            }
                                                                                                        }
                                                                                                    },
                                                                                                    new Node
                                                                                                    {
                                                                                                        Key = "RANK",
                                                                                                        Value = "JACK",
                                                                                                        children = new List<Node>()
                                                                                                        {
                                                                                                            new Node
                                                                                                            {
                                                                                                                Key = "COLOR",
                                                                                                                Value = "RED",
                                                                                                                children = new List<Node>()
                                                                                                                {
                                                                                                                    new Node
                                                                                                                    {
                                                                                                                        Key = "SUIT",
                                                                                                                        Value = "HEARTS",
                                                                                                                        children = {}
                                                                                                                    },
                                                                                                                    new Node
                                                                                                                    {
                                                                                                                        Key = "SUIT",
                                                                                                                        Value = "DIAMONDS",
                                                                                                                        children = {}
                                                                                                                    }
                                                                                                                }
                                                                                                            },
                                                                                                            new Node
                                                                                                            {
                                                                                                                Key = "COLOR",
                                                                                                                Value = "BLACK",
                                                                                                                children = new List<Node>()
                                                                                                                {
                                                                                                                   new Node
                                                                                                                    {
                                                                                                                        Key = "SUIT",
                                                                                                                        Value = "SPADES",
                                                                                                                        children = {}
                                                                                                                    },
                                                                                                                    new Node
                                                                                                                    {
                                                                                                                        Key = "SUIT",
                                                                                                                        Value = "CLUBS",
                                                                                                                        children = {}
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                };

                Tree prizeDeck = new Tree
                {
                    rootNode = new Node
                    {
                        Key = "RANK",
                        children = new List<Node>()
                        {
                            new Node
                            {
                                Key = "RANK",
                                Value = "QUEEN",
                                children = new List<Node>()
                                {
                                    new Node
                                    {
                                        Key = "COLOR",
                                        Value = "RED",
                                        children = new List<Node>()
                                        {
                                            new Node
                                            {
                                                Key = "SUIT",
                                                Value = "HEARTS",
                                                children = {}
                                            },
                                            new Node
                                            {
                                                Key = "SUIT",
                                                Value = "DIAMONDS",
                                                children = {}
                                            }
                                        }
                                    },
                                    new Node
                                    {
                                        Key = "COLOR",
                                        Value = "BLACK",
                                        children = new List<Node>()
                                        {
                                            new Node
                                            {
                                                Key = "SUIT",
                                                Value = "SPADES",
                                                children = {}
                                            },
                                            new Node
                                            {
                                                Key = "SUIT",
                                                Value = "CLUBS",
                                                children = {}
                                            }
                                        }
                                    },
                                    new Node
                                    {
                                        Key = "RANK",
                                        Value = "KING",
                                        children = new List<Node>()
                                        {
                                            new Node
                                            {
                                                Key = "COLOR",
                                                Value = "RED",
                                                children = new List<Node>()
                                                {
                                                    new Node
                                                    {
                                                        Key = "SUIT",
                                                        Value = "HEARTS",
                                                        children = {}
                                                    },
                                                    new Node
                                                    {
                                                        Key = "SUIT",
                                                        Value = "DIAMONDS",
                                                        children = {}
                                                    }
                                                }
                                            },
                                            new Node
                                            {
                                                Key = "COLOR",
                                                Value = "BLACK",
                                                children = new List<Node>()
                                                {
                                                   new Node
                                                    {
                                                        Key = "SUIT",
                                                        Value = "SPADES",
                                                        children = {}
                                                    },
                                                    new Node
                                                    {
                                                        Key = "SUIT",
                                                        Value = "CLUBS",
                                                        children = {}
                                                    }
                                                }
                                            },
                                            new Node
                                            {
                                                Key = "RANK",
                                                Value = "ACE",
                                                children = new List<Node>()
                                                {
                                                    new Node
                                                    {
                                                        Key = "COLOR",
                                                        Value = "RED",
                                                        children = new List<Node>()
                                                        {
                                                            new Node
                                                            {
                                                                Key = "SUIT",
                                                                Value = "HEARTS",
                                                                children = {}
                                                            },
                                                            new Node
                                                            {
                                                                Key = "SUIT",
                                                                Value = "DIAMONDS",
                                                                children = {}
                                                            }
                                                        }
                                                    },
                                                    new Node
                                                    {
                                                        Key = "COLOR",
                                                        Value = "BLACK",
                                                        children = new List<Node>()
                                                        {
                                                           new Node
                                                            {
                                                                Key = "SUIT",
                                                                Value = "SPADES",
                                                                children = {}
                                                            },
                                                            new Node
                                                            {
                                                                Key = "SUIT",
                                                                Value = "CLUBS",
                                                                children = {}
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
                
                
                Console.WriteLine("Creating card location references.");
                var fancy = new CardLocReference()
                {
                    cardList = cg.table[0].cardBins[CCType.INVISIBLE, "CASH"],
                    locIdentifier = "top",
                    name = ""
                };
                var createCashDeck = new InitializeAction(fancy.cardList, cashDeck, fancy.name, cg, script);
                gameActionList.Add(createCashDeck);
                
                fancy = new CardLocReference()
                {
                    cardList = cg.table[0].cardBins[CCType.INVISIBLE, "PRIZE"],
                    locIdentifier = "top",
                    name = ""
                };
                var createPrizeDeck = new InitializeAction(fancy.cardList, prizeDeck, fancy.name, cg, script);
                gameActionList.Add(createPrizeDeck);

                Console.WriteLine("Executing game actions.");
                gameActionList.ExecuteAll();
                gameActionList.Clear();
            }
            
            // shuffle decks
            void ShuffleTest()
            {
                Console.WriteLine("Shuffling decks.");
                var cashDeck = new CardLocReference()
                {
                    cardList = cg.table[0].cardBins[CCType.INVISIBLE, "CASH"],
                    locIdentifier = "top",
                    name = ""
                };
                
                /*
                var unshuffled = new CardCollection(CCType.VIRTUAL);
                foreach (Card c in fancy.cardList.AllCards())
                {
                    unshuffled.Add(c);
                }
                fancy.cardList.Shuffle();
                script.WriteToFile("O:" + fancy.cardList);
                */

                var prizeDeck = new CardLocReference()
                {
                    cardList = cg.table[0].cardBins[CCType.INVISIBLE, "PRIZE"],
                    locIdentifier = "top",
                    name = ""
                };
                
                /*
                unshuffled = new CardCollection(CCType.VIRTUAL);
                foreach (Card c in fancy.cardList.AllCards())
                {
                    unshuffled.Add(c);
                }
                fancy.cardList.Shuffle();
                script.WriteToFile("O:" + fancy.cardList);
                */

                gameActionList.Add(new ShuffleAction(cashDeck, script));
                gameActionList.Add(new ShuffleAction(prizeDeck, script));

                Console.WriteLine("Executing game actions");
                gameActionList.ExecuteAll();
                gameActionList.Clear();
            }

            // give each player 12 cards
            void DealTest()
            {
                Console.WriteLine("Dealing cards.");
                for (int p = 0; p < cg.players.Length; p++) // make lower level (use RepeatAction)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        var start = new CardLocReference()
                        {
                            cardList = cg.table[0].cardBins[CCType.INVISIBLE, "CASH"],
                            locIdentifier = "top",
                            name = ""
                        };

                        var end = new CardLocReference()
                        {
                            cardList = cg.players[p].cardBins[CCType.INVISIBLE, "HAND"],
                            locIdentifier = "top",
                            name = ""
                        };
                        
                        gameActionList.Add(new CardMoveAction(start, end, script));
                    }
                }

                Console.WriteLine("Executing game actions");
                gameActionList.ExecuteAll();
                gameActionList.Clear();
            }
            
            void MoveCardToAwardPileTest()
            {
                // move top card of prize pile to award pile
                var start = new CardLocReference()
                {
                    cardList = cg.table[0].cardBins[CCType.INVISIBLE, "PRIZE"],
                    locIdentifier = "top",
                    name = ""
                };
                
                var end = new CardLocReference()
                {
                    cardList = cg.table[0].cardBins[CCType.VISIBLE, "AWARD"],
                    locIdentifier = "top",
                    name = ""
                };

                gameActionList.Add(new CardMoveAction(start, end, script));
                
                Console.WriteLine("Executing game actions");
                gameActionList.ExecuteAll();
                gameActionList.Clear();
            }

            void PlayCardTest()
            {
                // players will all move their top card to the hiddentrick
                for (int p = 0; p < cg.players.Length; p++)
                {
                    var start = new CardLocReference()
                    {
                        cardList = cg.players[p].cardBins[CCType.INVISIBLE, "HAND"],
                        locIdentifier = "top",
                        name = ""
                    };

                    var end = new CardLocReference()
                    {
                        cardList = cg.players[p].cardBins[CCType.INVISIBLE, "HIDDENTRICK"],
                        locIdentifier = "top",
                        name = ""
                    };

                    gameActionList.Add(new CardMoveAction(start, end, script));
                    
                    Console.WriteLine("Executing game actions");
                    gameActionList.ExecuteAll();
                    gameActionList.Clear();
                }
            }

            void CreatePrecedencePointMapTest()
            {
                //get attrs of award card
                Console.WriteLine("Creating precedence point map.");
                var awardLoc = new CardLocReference()
                {
                    cardList = cg.table[0].cardBins[CCType.VISIBLE, "AWARD"],
                    locIdentifier = "top",
                    name = ""
                };
                Card award = awardLoc.Get();
                string awardSuit = award.ReadAttribute("SUIT");
                string awardColor = award.ReadAttribute("COLOR");
                
                // create precedence map
                List<ValueTuple<string, string, int>> map = new List<ValueTuple<string, string, int>>
                {
                    new ValueTuple<string, string, int>("SUIT", awardSuit, 2),
                    new ValueTuple<string, string, int>("COLOR", awardColor, 1),
                    new ValueTuple<string, string, int>("RANK", "JACK", 110),
                    new ValueTuple<string, string, int>("RANK", "TEN", 100),
                    new ValueTuple<string, string, int>("RANK", "NINE", 90),
                    new ValueTuple<string, string, int>("RANK", "EIGHT", 80),
                    new ValueTuple<string, string, int>("RANK", "SEVEN", 70),
                    new ValueTuple<string, string, int>("RANK", "SIX", 60),
                    new ValueTuple<string, string, int>("RANK", "FIVE", 50),
                    new ValueTuple<string, string, int>("RANK", "FOUR", 40),
                    new ValueTuple<string, string, int>("RANK", "THREE", 30),
                    new ValueTuple<string, string, int>("RANK", "TWO", 20),
                };

                var precedencePointMap = new PointStorageReference(cg.table[0].pointBins, "PRECEDENCE");
                new PointsAction(precedencePointMap.storage, precedencePointMap.key, new PointMap(map), script).Execute();
            }

            // make hidden cards visible and determine trick winner (move won card to WON & others to discard)
            void DetermineTrickWinner()
            {
                Console.WriteLine("Moving players' cards from HIDDENTRICK to TRICK");
                for (int p = 0; p< cg.players.Length; p++)
                {
                    var start = new CardLocReference()
                    {
                        cardList = cg.players[p].cardBins[CCType.INVISIBLE, "HIDDENTRICK"],
                        locIdentifier = "top",
                        name = ""
                    };

                    var end = new CardLocReference()
                    {
                        cardList = cg.players[p].cardBins[CCType.VISIBLE, "TRICK"],
                        locIdentifier = "top",
                        name = ""
                    };

                    gameActionList.Add(new CardMoveAction(start, end, script));
                    
                    Console.WriteLine("Executing game actions");
                    gameActionList.ExecuteAll();
                    gameActionList.Clear();
                }

                string name = "";
                CardCollection TC = new CardCollection(CCType.VIRTUAL);
                var ret = new List<object>();
                foreach (Player p in cg.players)
                {
                    Debug.WriteLine("Iterating over aggregation of: " + p.GetType());
                    variables.Put("'P", p);
                    var player = variables.Get("'P") as Player;
                    var fancy = new CardLocReference()
                    {
                        cardList = player.cardBins[CCType.VISIBLE, "TRICK"],
                        name = player.name + " " + CCType.VISIBLE + " " + "TRICK"
                    };
                    var post = fancy;
                    ret.Add(post);
                    variables.Remove("'P");
                }

                var coll = new List<CardLocReference>();
                foreach (object obj in ret)
                {
                    coll.Add((CardLocReference)obj);
                }

                foreach (var locs in coll)
                {
                    name += locs.name + " ";
                    foreach (var card in locs.cardList.AllCards())
                    {
                        TC.Add(card);
                    }
                }
                name.Remove(name.Length - 1);
                var clr = new CardLocReference()
                {
                    cardList = TC,
                    name = name + "{UNION}"
                };

                CardCollection maxList = new CardCollection(CCType.VIRTUAL);
                foreach (var card in clr.cardList.AllCards())
                {
                    // create union of each player's TRICK
                    name = "";
                    TC = new CardCollection(CCType.VIRTUAL);
                    ret = new List<object>();
                    foreach (Player p in cg.players)
                    {
                        Debug.WriteLine("Iterating over aggregation of: " + p.GetType());
                        variables.Put("'P", p);
                        var player = variables.Get("'P") as Player;
                        var fancy2 = new CardLocReference()
                        {
                            cardList = player.cardBins[CCType.VISIBLE, "TRICK"],
                            name = player.name + " " + CCType.VISIBLE + " " + "TRICK"
                        };
                        var post = fancy2;
                        ret.Add(post);
                        variables.Remove("'P");
                    }

                    coll = new List<CardLocReference>();
                    foreach (object obj in ret)
                    {
                        coll.Add((CardLocReference)obj);
                    }

                    foreach (var locs in coll)
                    {
                        name += locs.name + " ";
                        foreach (var trickCard in locs.cardList.AllCards())
                        {
                            TC.Add(trickCard);
                        }
                    }
                    name.Remove(name.Length - 1);
                    clr = new CardLocReference()
                    {
                        cardList = TC,
                        name = name + "{UNION}"
                    };

                    //find max
                    CardLocReference fancy = clr;
                    // score that card
                    var scoring = new PointStorageReference(cg.table[0].pointBins, "PRECEDENCE").Get();
                    // get max of union (using PRECEDENCE)
                    var max = -1;
                    Card maxCard = null;
                    foreach (var trickCard in TC.AllCards()) //Taken from ProcessCard in GameIterator
                    {
                        //MHG when equal, pick randomly
                        if (scoring.GetScore(trickCard) > max || (scoring.GetScore(trickCard) == max && ThreadSafeRandom.Next(0, 2) == 0))
                        {
                            max = scoring.GetScore(trickCard);
                            maxCard = trickCard;
                        }
                    }
                    var lst = new CardCollection(CCType.VIRTUAL);
                    lst.Add(maxCard);
                    fancy = new CardLocReference()
                    {
                        cardList = lst,
                        locIdentifier = "top",
                        name = TC.name + "{MAX}"
                    };
                    // figure out score of max card
                    var scor = new PointStorageReference(cg.table[0].pointBins, "PRECEDENCE").Get();
                    var maxScore = scor.GetScore(fancy.Get());

                    // compare the two scores
                    if (maxScore == scor.GetScore(card))
                    {
                        maxList.Add(card);
                    }
                }

                // check if size of filtered card collection is > 1
                if (maxList.Count > 1) {
                    // move award to min owner's won (GameAction, look at shuffling)
                    // create union of every player's TRICK
                    CardCollection trickUnion = new CardCollection(CCType.VIRTUAL);
                    foreach (var card in clr.cardList.AllCards())
                    {
                        // create union of each player's TRICK
                        name = "";
                        ret = new List<object>();
                        foreach (Player p in cg.players)
                        {
                            Debug.WriteLine("Iterating over aggregation of: " + p.GetType());
                            variables.Put("'P", p);
                            var player = variables.Get("'P") as Player;
                            var fancy3 = new CardLocReference()
                            {
                                cardList = player.cardBins[CCType.VISIBLE, "TRICK"],
                                name = player.name + " " + CCType.VISIBLE + " " + "TRICK"
                            };
                            var post = fancy3;
                            ret.Add(post);
                            variables.Remove("'P");
                        }

                        coll = new List<CardLocReference>();
                        foreach (object obj in ret)
                        {
                            coll.Add((CardLocReference)obj);
                        }

                        foreach (var locs in coll)
                        {
                            name += locs.name + " ";
                            foreach (var trickCard in locs.cardList.AllCards())
                            {
                                trickUnion.Add(trickCard);
                            }
                        }
                        name.Remove(name.Length - 1);
                        clr = new CardLocReference()
                        {
                            cardList = trickUnion,
                            name = name + "{UNION}"
                        };
                    }

                    // find min
                    CardLocReference fancy = new CardLocReference() { };
                    var lst = new CardCollection(CCType.VIRTUAL);
                    // score that card
                    var scoring = new PointStorageReference(cg.table[0].pointBins, "PRECEDENCE").Get();
                    // get min of union (using PRECEDENCE)
                    var min = 1000;
                    Card minCard = null;
                    foreach (var trickCard in trickUnion.AllCards()) //Taken from ProcessCard in GameIterator
                    {
                        //MHG when equal, pick randomly
                        if (scoring.GetScore(trickCard) < min || (scoring.GetScore(trickCard) == min && ThreadSafeRandom.Next(0, 2) == 0))
                        {
                            min = scoring.GetScore(trickCard);
                            minCard = trickCard;
                        }
                    }
                    lst = new CardCollection(CCType.VIRTUAL);
                    lst.Add(minCard);
                    fancy = new CardLocReference()
                    {
                        cardList = lst,
                        locIdentifier = "top",
                        name = TC.name + "{MIN}"
                    };

                    // find owner of min
                    int pId = ((Player)fancy.Get().owner.owner.owner).id;

                    // move top of AWARD to top of owner's WON
                    fancy = new CardLocReference()
                    {
                        cardList = cg.table[0].cardBins[CCType.VISIBLE, "AWARD"],
                        locIdentifier = "top",
                        name = "table " + CCType.VISIBLE + " " + "AWARD"
                    };

                    var fancy2 = new CardLocReference()
                    {
                        cardList = cg.players[pId].cardBins[CCType.VISIBLE, "WON"],
                        locIdentifier = "top",
                        name = "p" + pId + ":" + CCType.VISIBLE + " " + "WON"
                    };

                    gameActionList.Add(new CardMoveAction(fancy, fancy2, script));
                }


                name = "";
                TC = new CardCollection(CCType.VIRTUAL);
                ret = new List<object>();
                foreach (Player p in cg.players)
                {
                    Debug.WriteLine("Iterating over aggregation of: " + p.GetType());
                    variables.Put("'P", p);
                    var player = variables.Get("'P") as Player;
                    var fancy = new CardLocReference()
                    {
                        cardList = player.cardBins[CCType.VISIBLE, "TRICK"],
                        name = player.name + " " + CCType.VISIBLE + " " + "TRICK"
                    };
                    var post = fancy;
                    ret.Add(post);
                    variables.Remove("'P");
                }

                coll = new List<CardLocReference>();
                foreach (object obj in ret)
                {
                    coll.Add((CardLocReference)obj);
                }

                foreach (var locs in coll)
                {
                    name += locs.name + " ";
                    foreach (var card in locs.cardList.AllCards())
                    {
                        TC.Add(card);
                    }
                }
                name.Remove(name.Length - 1);
                clr = new CardLocReference()
                {
                    cardList = TC,
                    name = name + "{UNION}"
                };

                maxList = new CardCollection(CCType.VIRTUAL);
                foreach (var card in clr.cardList.AllCards())
                {
                    // create union of each player's TRICK
                    name = "";
                    TC = new CardCollection(CCType.VIRTUAL);
                    ret = new List<object>();
                    foreach (Player p in cg.players)
                    {
                        Debug.WriteLine("Iterating over aggregation of: " + p.GetType());
                        variables.Put("'P", p);
                        var player = variables.Get("'P") as Player;
                        var fancy2 = new CardLocReference()
                        {
                            cardList = player.cardBins[CCType.VISIBLE, "TRICK"],
                            name = player.name + " " + CCType.VISIBLE + " " + "TRICK"
                        };
                        var post = fancy2;
                        ret.Add(post);
                        variables.Remove("'P");
                    }

                    coll = new List<CardLocReference>();
                    foreach (object obj in ret)
                    {
                        coll.Add((CardLocReference)obj);
                    }

                    foreach (var locs in coll)
                    {
                        name += locs.name + " ";
                        foreach (var trickCard in locs.cardList.AllCards())
                        {
                            TC.Add(trickCard);
                        }
                    }
                    name.Remove(name.Length - 1);
                    clr = new CardLocReference()
                    {
                        cardList = TC,
                        name = name + "{UNION}"
                    };

                    //find max
                    CardLocReference fancy = clr;
                    // score that card
                    var scoring = new PointStorageReference(cg.table[0].pointBins, "PRECEDENCE").Get();
                    // get max of union (using PRECEDENCE)
                    var max = -1;
                    Card maxCard = null;
                    foreach (var trickCard in TC.AllCards()) //Taken from ProcessCard in GameIterator
                    {
                        //MHG when equal, pick randomly
                        if (scoring.GetScore(trickCard) > max || (scoring.GetScore(trickCard) == max && ThreadSafeRandom.Next(0, 2) == 0))
                        {
                            max = scoring.GetScore(trickCard);
                            maxCard = trickCard;
                        }
                    }
                    var lst = new CardCollection(CCType.VIRTUAL);
                    lst.Add(maxCard);
                    fancy = new CardLocReference()
                    {
                        cardList = lst,
                        locIdentifier = "top",
                        name = TC.name + "{MAX}"
                    };
                    // figure out score of max card
                    var scor = new PointStorageReference(cg.table[0].pointBins, "PRECEDENCE").Get();
                    var maxScore = scor.GetScore(fancy.Get());

                    // compare the two scores
                    if (maxScore == scor.GetScore(card))
                    {
                        maxList.Add(card);
                    }
                }

                // check if size of filtered card collection is > 1
                if (maxList.Count <= 1)
                {
                    // move award to max owner's won (GameAction, look at shuffling)
                    // create union of every player's TRICK
                    CardCollection trickUnion = new CardCollection(CCType.VIRTUAL);
                    foreach (var card in clr.cardList.AllCards())
                    {
                        // create union of each player's TRICK
                        name = "";
                        ret = new List<object>();
                        foreach (Player p in cg.players)
                        {
                            Debug.WriteLine("Iterating over aggregation of: " + p.GetType());
                            variables.Put("'P", p);
                            var player = variables.Get("'P") as Player;
                            var fancy3 = new CardLocReference()
                            {
                                cardList = player.cardBins[CCType.VISIBLE, "TRICK"],
                                name = ""
                            };
                            var post = fancy3;
                            ret.Add(post);
                            variables.Remove("'P");
                        }

                        coll = new List<CardLocReference>();
                        foreach (object obj in ret)
                        {
                            coll.Add((CardLocReference)obj);
                        }

                        foreach (var locs in coll)
                        {
                            name += locs.name + " ";
                            foreach (var trickCard in locs.cardList.AllCards())
                            {
                                trickUnion.Add(trickCard);
                            }
                        }
                        name.Remove(name.Length - 1);
                        clr = new CardLocReference()
                        {
                            cardList = trickUnion,
                            name = name + "{UNION}"
                        };
                    }

                    // find max
                    // import scoring map
                    var scoring = new PointStorageReference(cg.table[0].pointBins, "PRECEDENCE").Get();
                    // get max of union (using PRECEDENCE)
                    var max = -1;
                    Card maxCard = null;
                    foreach (var trickCard in trickUnion.AllCards()) //Taken from ProcessCard in GameIterator
                    {
                        //MHG when equal, pick randomly
                        if (scoring.GetScore(trickCard) > max || (scoring.GetScore(trickCard) == max && ThreadSafeRandom.Next(0, 2) == 0))
                        {
                            max = scoring.GetScore(trickCard);
                            maxCard = trickCard;
                        }
                    }
                    var lst = new CardCollection(CCType.VIRTUAL);
                    lst.Add(maxCard);
                    var fancy = new CardLocReference()
                    {
                        cardList = lst,
                        locIdentifier = "top",
                        name = TC.name + "{MAX}"
                    };

                    // find owner of max
                    int pId = ((Player)fancy.Get().owner.owner.owner).id;

                    // move top of AWARD to top of owner's WON
                    fancy = new CardLocReference()
                    {
                        cardList = cg.table[0].cardBins[CCType.VISIBLE, "AWARD"],
                        locIdentifier = "top",
                        name = "table " + CCType.VISIBLE + " " + "AWARD"
                    };

                    var fancy2 = new CardLocReference()
                    {
                        cardList = cg.players[pId].cardBins[CCType.VISIBLE, "WON"],
                        locIdentifier = "top",
                        name = "p" + pId + ":" + CCType.VISIBLE + " " + "WON"
                    };

                    gameActionList.Add(new CardMoveAction(fancy, fancy2, script));

                    // move other played cards to DISCARD
                    var discard = new CardLocReference()
                    {
                        cardList = cg.table[0].cardBins[CCType.VISIBLE, "DISCARD"],
                        locIdentifier = "top",
                        name = "table " + CCType.VISIBLE + " " + "DISCARD"
                    };

                    for (int p = 0; p < cg.players.Length; p++)
                    {
                        var player_trick = new CardLocReference()
                        {
                            cardList = cg.players[p].cardBins[CCType.VISIBLE, "TRICK"],
                            locIdentifier = "top",
                            name = "p" + p + ":" + CCType.VISIBLE + " " + "TRICK"
                        };

                        gameActionList.Add(new CardMoveAction(player_trick, discard, script));
                    }
                }
                gameActionList.ExecuteAll();
            }

            void CreateScoringPointMapTest()
            {
                List<ValueTuple<string, string, int>> v = new List<ValueTuple<string, string, int>>
                {
                    new ValueTuple<string, string, int>("RANK", "ACE", 4),
                    new ValueTuple<string, string, int>("RANK", "KING", 2),
                    new ValueTuple<string, string, int>("RANK", "QUEEN", 1),
                };

                var scoringPointMap = new PointStorageReference(cg.table[0].pointBins, "SCOREPOINTS");
                new PointsAction(scoringPointMap.storage, scoringPointMap.key, new PointMap(v), script).Execute();
            }

            // determine game winner using scorepoints
            void ScoringTest()
            {
                var ret = new List<Tuple<int, int>>();

                cg.PushPlayer();
                cg.CurrentPlayer().SetMember(0);
                for (int p = 0; p < cg.players.Length; p++)
                {
                    var scoring = new PointStorageReference(cg.table[0].pointBins, "SCOREPOINTS").Get();
                    var coll = new CardLocReference()
                    {
                        cardList = cg.players[p].cardBins[CCType.VISIBLE, "WON"],
                        locIdentifier = "top",
                        name = "table " + CCType.VISIBLE + " " + "WON"
                    };
                    int total = 0;
                    foreach (var c in coll.cardList.AllCards())
                    {
                        total += scoring.GetScore(c);
                    }
                    Debug.WriteLine("Sum:" + total);

                    var working = total;
                    script.WriteToFile("s:" + working + " " + p);
                    ret.Add(new Tuple<int, int>(working, p));
                    cg.CurrentPlayer().Next();
                    script.WriteToFile("t: " + cg.CurrentPlayer().CurrentName());
                }
                cg.PopPlayer();

                ret.Sort();
                ret.Reverse();
            }
        }
    }