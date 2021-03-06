﻿using System;
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
            knowledges = new List<Knowledge>();
        }

        public string Id { get; set; }

        public List<Knowledge> knowledges { get; set; }

        public void addKnowledge(Knowledge newKnowledge)
        {
            bool isNewKnowledge = true;

            foreach (Knowledge knowledge in knowledges)
                if (knowledge.Equals(newKnowledge))
                    isNewKnowledge = false;

            if (isNewKnowledge)
                knowledges.Add(newKnowledge);
        }

        /// <summary>
        /// Message-meaning rule
        /// </summary>
        /// <param name="keyQP"></param>
        private void messageMeaningRule(Key keyQP)
        {
            for (int i = 0; i < knowledges.Count; i++)
                if (knowledges[i].knowledgeType == KnowledgeType.sees && knowledges[i].statement is Message)
                    if (((Message)knowledges[i].statement).Encrypted.Equals(keyQP))
                    {
                        Message uncryptedMessage = (Message)knowledges[i].statement;
                        uncryptedMessage.refreshStatementsList();

                        foreach (IStatement statement in uncryptedMessage.statements)
                            addKnowledge(new Knowledge(this, KnowledgeType.believes, new Knowledge(keyQP.getOpposed(this), KnowledgeType.said, statement)));
                    }
        }

        /// <summary>
        /// Nonce-verification rule
        /// </summary>
        /// <param name="statement"></param>
        private void nonceVerificationRule(IStatement statement)
        {
            for (int i = 0; i < knowledges.Count; i++)
                if (knowledges[i].knowledgeType == KnowledgeType.believes && knowledges[i].statement is Knowledge)
                { 
                    Knowledge knowledge = (Knowledge)knowledges[i].statement;

                    if (!knowledge.agent.Equals(this) && knowledge.knowledgeType.Equals(KnowledgeType.said) && 
                            knowledge.statement.Equals(statement))
                        addKnowledge(new Knowledge(this, KnowledgeType.believes,
                            new Knowledge(((Knowledge)knowledges[i].statement).agent, KnowledgeType.believes, ((Knowledge)knowledges[i].statement).statement)));
                }
        }

        /// <summary>
        /// Jurisdiction rule
        /// </summary>
        /// <param name="knowledge"></param>
        private void jurisdiction(Knowledge knowledge)
        {
            for (int i = 0; i < knowledges.Count; i++)
                if (knowledges[i].knowledgeType == KnowledgeType.believes && knowledges[i].statement is Knowledge)
                {
                    Knowledge secondKnowledge = (Knowledge)knowledges[i].statement;

                    if (secondKnowledge.agent.Equals(knowledge.agent) && secondKnowledge.knowledgeType.Equals(KnowledgeType.believes)
                        && secondKnowledge.statement.Equals(knowledge.statement))
                        addKnowledge(new Knowledge(this, KnowledgeType.believes, knowledge.statement));
                }
        }

        /// <summary>
        /// Breaking a formula in multiple components
        /// </summary>
        /// <param name="keyQP"></param>
        private void breakInComponents(Key keyQP)
        {
            for (int i = 0; i < knowledges.Count; i++)
                if (knowledges[i].knowledgeType == KnowledgeType.sees && knowledges[i].statement is Message)
                    if (((Message)knowledges[i].statement).Encrypted.Equals(keyQP))
                        foreach (IStatement statement in ((Message)knowledges[i].statement).statements)
                            addKnowledge(new Knowledge(this, KnowledgeType.sees, statement));
        }

        /// <summary>
        /// Searches for messages containing X which P believes is fresh.
        /// </summary>
        /// <param name="statemenet"></param>
        private void checkMessagesForFreshness(IStatement freshStatement)
        {
            for (int i = 0; i < knowledges.Count; i++)
                if (knowledges[i].statement is Message)
                {
                    Message message = (Message)knowledges[i].statement;
                    if (message.contains(freshStatement))
                    {
                        message.refreshStatementsList();

                        foreach (IStatement statement in message.statements)
                            addKnowledge(new Knowledge(this, KnowledgeType.believes, statement));
                    }
                }
        }

        public void expandKnowledge()
        {
            int initialKnowledges = -1;

            while (initialKnowledges < knowledges.Count())
            {
                initialKnowledges = knowledges.Count();
                // I - Message-meaning
                for (int i = 0; i < knowledges.Count; i++)
                    if (knowledges[i].knowledgeType == KnowledgeType.believes && knowledges[i].statement is Key)
                        messageMeaningRule((Key)knowledges[i].statement);

                // II - Nonce-verification
                for (int i = 0; i < knowledges.Count; i++)
                    if (knowledges[i].knowledgeType == KnowledgeType.believes && knowledges[i].statement.Fresh.Equals(true))
                        nonceVerificationRule(knowledges[i].statement);

                // III - Jurisdiction
                for (int i = 0; i < knowledges.Count; i++)
                    if (knowledges[i].knowledgeType == KnowledgeType.believes && knowledges[i].statement is Knowledge)
                        if (((Knowledge)knowledges[i].statement).knowledgeType.Equals(KnowledgeType.controls))
                            jurisdiction((Knowledge)knowledges[i].statement);

                // IV - Components of formula (Similar logic to I)
                for (int i = 0; i < knowledges.Count; i++)
                    if (knowledges[i].knowledgeType == KnowledgeType.believes && knowledges[i].statement is Key)
                        breakInComponents((Key)knowledges[i].statement);

                // V - Formula with fresh part
                for (int i = 0; i < knowledges.Count; i++)
                    if (knowledges[i].knowledgeType == KnowledgeType.believes && knowledges[i].statement.Fresh.Equals(true))
                        checkMessagesForFreshness(knowledges[i].statement);

                // VI See Messages in unencrypted messages
                for (int i = 0; i < knowledges.Count; i++)
                    if (knowledges[i].knowledgeType == KnowledgeType.sees && knowledges[i].statement is Message)
                        if (((Message)knowledges[i].statement).Encrypted.agents.Count == 0)
                        {
                            Message unencryptedMessage = (Message)knowledges[i].statement;

                            unencryptedMessage.refreshStatementsList();

                            foreach (IStatement statement in unencryptedMessage.statements)
                                addKnowledge(new Knowledge(this, KnowledgeType.sees, statement));
                        }
            }
        }

        private string getTabs(int number)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < number; i++)
                sb.Append("\t");
            return sb.ToString();
        }

        public override string ToString()
        {
            return ToString(1);
        }

        public string ToString(int numberOfTabs)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("------------ Agent " + this.Id + (" ------------\r\n"));
            sb.Append("\r\nKnowledge(s):\r\n");
            foreach (Knowledge knowledge in this.knowledges)
                if (knowledge is Message)
                    sb.Append(getTabs(numberOfTabs) + knowledge.ToString(numberOfTabs + 1) + "\r\n");
                else
                    sb.Append(getTabs(numberOfTabs) + knowledge.ToString() + "\r\n");

            return sb.ToString() + "\r\n";
        }
    }
}
