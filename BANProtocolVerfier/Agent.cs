using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BANProtocolVerfier
{
    public class Agent
    {
        public Agent()
        {
            nonces = new List<string>();
            keys = new List<Key>();
            actions = new List<Action>();
        }

        public string Id { get; set; }
        public List<String> nonces { get; set; }
        public List<Key> keys { get; set; }

        public List<Action> actions  { get; set; }

        private void addKnowledge(Agent a, ActionType actionType, IStatement statement)
        {
            foreach (Action action in actions)
                if (!action.agent.Equals(a) || !action.actionType.Equals(actionType) || !action.statement.Equals(statement))
                    actions.Add(new Action(a, actionType, statement));
                    // Overwrite equals in all classes which implement IStatement
        }

        /// <summary>
        /// Message-meaning rule
        /// </summary>
        /// <param name="keyQP"></param>
        private void messageMeaningRule(Key keyQP)
        {
            foreach (Action action in actions)
                if (action.actionType == ActionType.sees && action.statement is Message)
                    if (((Message)action.statement).Encrypted.Equals(keyQP))
                            addKnowledge(this, ActionType.believes, action.statement);
            // Same message but without encryption
        }

        /// <summary>
        /// Nonce-verification rule
        /// </summary>
        /// <param name="statement"></param>
        private void nonceVerificationRule(IStatement statement)
        {
            foreach (Action action in actions)
                if (action.actionType == ActionType.believes && action.statement is Action)
                    if (!((Action)action.statement).agent.Equals(this) && ((Action)action.statement).actionType.Equals(ActionType.believes))
                        addKnowledge(this, ActionType.believes, new Action(((Action)action.statement).agent, ActionType.believes, ((Action)action.statement).statement));
        }

        /// <summary>
        /// Jurisdiction rule
        /// </summary>
        /// <param name="action"></param>
        private void jurisdiction(Action action)
        {
            foreach (Action otherAction in actions)
                if (otherAction.actionType == ActionType.believes && action.statement is Action)
                    if (((Action)action.statement).agent.Equals(action.agent) && ((Action)action.statement).actionType.Equals(ActionType.believes)
                        && ((Action)action.statement).statement.Equals(action.statement))
                        addKnowledge(this, ActionType.believes, action.statement);
        }

        /// <summary>
        /// Breaking a formula in multiple components
        /// </summary>
        /// <param name="keyQP"></param>
        private void breakInComponents(Key keyQP)
        {
            foreach (Action action in actions)
                if (action.actionType == ActionType.sees && action.statement is Message)
                    if (((Message)action.statement).Encrypted.Equals(keyQP))
                        foreach (IStatement statement in ((Message)action.statement).statements)
                            addKnowledge(this, ActionType.believes, statement);
        }

        /// <summary>
        /// Searches for messages containing X which P believes is fresh.
        /// </summary>
        /// <param name="statemenet"></param>
        private void checkMessagesForFreshness(IStatement statemenet)
        {

        }

        public void expandKnowledge()
        {
            // I - Message-meaning
            foreach (Action action in actions)
                if (action.actionType == ActionType.believes && action.statement is Key)
                    messageMeaningRule((Key)action.statement);

            // II - Nonce-verification
            foreach (Action action in actions)
                if (action.actionType == ActionType.believes && action.statement.Fresh.Equals(true))
                    nonceVerificationRule(action.statement);

            // III - Jurisdiction
            foreach (Action action in actions)
                if (action.actionType == ActionType.believes && action.statement is Action)
                    if (((Action)action.statement).actionType.Equals(ActionType.controls))
                        jurisdiction((Action)action.statement);

            // IV - Components of formula (Similar logic to I)
            foreach (Action action in actions)
                if (action.actionType == ActionType.believes && action.statement is Key)
                    breakInComponents((Key)action.statement);

            // V - Formula with fresh part
            foreach (Action action in actions)
                if (action.actionType == ActionType.believes && action.statement.Fresh.Equals(true))
                    breakInComponents((Key)action.statement);

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("------------ Agent " + this.Id + (" ------------"));
            sb.Append("\nNonces: ");
            foreach (string nonce in this.nonces)
            {
                sb.Append(nonce + " ");
            }

            sb.Append("\nKeys: ");
            foreach (Key key in this.keys)
            {
                sb.Append(key.ToString() + " ");
            }

            return sb.ToString() + "\n";
        }
    }
}
