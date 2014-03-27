using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BANProtocolVerfier
{
    public enum ActionType { believes, sees, said, controls }
    public class Action : IStatement
    {
        public Agent agent { get; set; }

        public ActionType actionType { get; set; }

        public IStatement statement { get; set; }

        public bool Fresh { get; set; }

        public Action(Agent newAgent, ActionType newActionType, IStatement newStatement)
        {
            agent = newAgent;
            actionType = newActionType;
            statement = newStatement;
        }
    }
}
