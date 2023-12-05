using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreezeFrame;

namespace CardStockXam.FreezeFrame
{
    public class Stage
    {
        Queue<GameAction> gameActionCollection;
        // create EndCondition object (can execute and return boolean)
        // create class for GameActionCreator
        // create objects for types such as bool, int, cardlocr

        public void init()
        {
            // find out where to get game file info
            // then run each stage through Process
        }

        public GameAction ProcessGameAction()
        {
            return null;
        }

        public void ExecuteAll()
        {
            // execute all gameAction's using functions
        }
    }
}
