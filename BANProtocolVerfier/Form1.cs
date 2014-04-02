using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BANProtocolVerfier
{
    public partial class Form1 : Form
    {
        private String fileContent;
        private Protocol protocol;

        public Form1()
        {
            InitializeComponent();
        }

        private void loadBtn_Click(object sender, EventArgs e)
        {
            /** Get info about loaded file: location, fullname */
            string filename = "", path = "";
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filename = Path.GetDirectoryName(ofd.FileName);
                path = Path.GetFullPath(ofd.FileName);


                /** Read the file line by line and put its content to the text box */
                using (StreamReader sr = new StreamReader(path))
                    txtProtocol.Text = fileContent = sr.ReadToEnd();
            }
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            if (fileContent != null)
            {
                Protocol protocol = this.Parse(fileContent);
                txtProtocol.Text = protocol.ToString();
            }
            else
                MessageBox.Show("Protocol not loaded, please load a protocol json-formatted file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private Protocol Parse(string jProtocol)
        {
            protocol = new Protocol();

            /** Parse the contents of the json-formatted protocol */
            dynamic jsonProtocol = JsonConvert.DeserializeObject(jProtocol);
            dynamic jsonAgents = jsonProtocol.agents;
            dynamic jsonPhases = jsonProtocol.phases;
            dynamic jsonKeys = jsonProtocol.keys;
            dynamic jsonKnowledges = jsonProtocol.knowledges;

            foreach (dynamic jsonAgent in jsonAgents)
            {
                Agent newAgent = new Agent();
                JObject jAgent = new JObject(jsonAgent);
                newAgent.Id = jAgent.Properties().Select(p => p.Name).ElementAt(0);
                JObject jAgentValue = (JObject)jAgent.Properties().Select(p => p.Value).ElementAt(0);
                protocol.Agents.Add(newAgent);
            }

            foreach (dynamic jsonKey in jsonKeys)
            {
                Key key = new Key();

                Agent a1 = protocol.Agents.Where(p => p.Id == jsonKey.a1.ToString()).ElementAt(0);
                Agent a2 = protocol.Agents.Where(p => p.Id == jsonKey.a2.ToString()).ElementAt(0);

                key.agents.Add(a1);
                key.agents.Add(a2);

                protocol.Keys.Add(key);
            }

            foreach (dynamic jsonKnowledge in jsonKnowledges)
            {
                Knowledge knowledge = ParseKnowledge(jsonKnowledge);
                knowledge.agent.addKnowledge(knowledge);
            }

            foreach (dynamic jsonPhase in jsonPhases)
            {
                Phase phase = new Phase();

                JObject jPhase = new JObject(jsonPhase);
                JObject jPhaseValue = (JObject)jPhase.Properties().Select(p => p.Value).ElementAt(0);

                if (jPhaseValue.Property("sender") != null)
                {
                    JObject jSender = new JObject(jPhaseValue.Property("sender"));
                    string id = (string)jSender.Properties().Select(p => p.Value).ElementAt(0);
                    Agent foundAgent = protocol.Agents.Where(p => p.Id == id).ElementAt(0);

                    phase.Sender = foundAgent;
                }

                if (jPhaseValue.Property("receiver") != null)
                {
                    JObject jReceiver = new JObject(jPhaseValue.Property("receiver"));
                    string id = (string)jReceiver.Properties().Select(p => p.Value).ElementAt(0);
                    Agent foundAgent = protocol.Agents.Where(a => a.Id == id).ElementAt(0);

                    phase.Receiver = foundAgent;
                }

                if (jPhaseValue.Property("message") != null)
                {
                    JObject jMessage = new JObject(jPhaseValue.Property("message"));
                    phase.message = this.ParseMessage(jMessage);
                }

                protocol.Phases.Add(phase);
            }

            return protocol;
        }

        private Message ParseMessage(JObject jMessageValue)
        {
            //JObject jMessage = new JObject(jsonMessage);
            //JObject jMessageValue = (JObject)jMessage.Properties().Select(p => p.Value).ElementAt(0);

            Message newMessage = new Message();

            if (jMessageValue.Property("ids") != null)
            {
                JObject jIds = new JObject(jMessageValue.Property("ids"));
                JArray idsArray = (JArray)jIds.Properties().Select(p => p.Value).ElementAt(0);
                foreach (string id in idsArray)
                {
                    newMessage.Agents.Add(protocol.Agents.Where(a => a.Id == id).ElementAt(0));
                }
            }

            if (jMessageValue.Property("nonces") != null)
            {
                JObject jNonces = new JObject(jMessageValue.Property("nonces"));
                JArray noncesArray = (JArray)jNonces.Properties().Select(p => p.Value).ElementAt(0);
                foreach (string nonceId in noncesArray)
                {
                    newMessage.Nonces.Add(new Nonce(nonceId));
                }
            }

            if (jMessageValue.Property("encrypted") != null)
            {
                JObject jEncrypted = new JObject(jMessageValue.Property("encrypted"));
                JObject jEncryptedValue = (JObject)jEncrypted.Properties().Select(p => p.Value).ElementAt(0);

                string a1 = jEncryptedValue.Property("a1").Value.ToString();
                string a2 = jEncryptedValue.Property("a2").Value.ToString();
                Key newKey = new Key();

                Agent ag1 = protocol.Agents.Where(p => p.Id == a1).ElementAt(0);
                Agent ag2 = protocol.Agents.Where(p => p.Id == a2).ElementAt(0);

                newKey.agents.Add(ag1);
                newKey.agents.Add(ag2);

                newMessage.Encrypted = newKey;
            }

            if (jMessageValue.Property("keys") != null)
            {
                JObject jKeys = new JObject(jMessageValue.Property("keys"));
                JArray jKeysValue = (JArray)jKeys.Properties().Select(p => p.Value).ElementAt(0);
                foreach (var jKey in jKeysValue)
                {
                    JObject keyValue = (JObject)jKey;
                    string agt1 = keyValue.Property("a1").Value.ToString();
                    string agt2 = keyValue.Property("a2").Value.ToString();

                    Key newKey = new Key();

                    Agent ag1 = protocol.Agents.Where(p => p.Id == agt1).ElementAt(0);
                    Agent ag2 = protocol.Agents.Where(p => p.Id == agt2).ElementAt(0);

                    newKey.agents.Add(ag1);
                    newKey.agents.Add(ag2);

                    if (protocol.Keys.Where(p => p.agents.ElementAt(0).Id == agt1 &&
                                                            p.agents.ElementAt(1).Id == agt2) != null)
                    {
                        Key foundKey = protocol.Keys.Where(p => p.agents.ElementAt(0).Id == agt1 &&
                                                            p.agents.ElementAt(1).Id == agt2).ElementAt(0);
                        newMessage.Keys.Add(foundKey);
                    }
                    else
                    {
                        newMessage.Keys.Add(newKey);
                    }
                }
            }

            if (jMessageValue.Property("messages") != null)
            {
                JObject jMessages = new JObject(jMessageValue.Property("messages"));
                JArray jMessagesValue = (JArray)jMessages.Properties().Select(p => p.Value).ElementAt(0);

                foreach (JProperty message in jMessages.Properties())
                {
                    JObject jMsg = new JObject(message);
                    MessageBox.Show(jMsg.ToString());

                    newMessage.Messages.Add(this.ParseMessage(jMsg));
                }
            }

            return newMessage;
        }

        private Knowledge ParseKnowledge(JObject jsonKnowledge)
        {

			Agent agent = null;
            KnowledgeType knowledgeType = KnowledgeType.believes; // Random initializaiton.
            IStatement statement = null;

            if (jsonKnowledge.Property("id") != null)
            {
                JObject jSender = new JObject(jsonKnowledge.Property("id"));
                string id = (string)jSender.Properties().Select(p => p.Value).ElementAt(0);
                agent = protocol.Agents.Where(p => p.Id == id).ElementAt(0);
            }

            if (jsonKnowledge.Property("type") != null)
            {
                JObject jSender = new JObject(jsonKnowledge.Property("type"));
                string type = (string)jSender.Properties().Select(p => p.Value).ElementAt(0);
                knowledgeType = (KnowledgeType)Enum.Parse(typeof(KnowledgeType), type);
            }

			if (jsonKnowledge.Property("key") != null)
            {
                JObject jKey = new JObject(jsonKnowledge.Property("key"));
                JObject jKeyValue = (JObject)jKey.Properties().Select(p => p.Value).ElementAt(0);

                string agt1 = jKeyValue.Property("a1").Value.ToString();
                string agt2 = jKeyValue.Property("a2").Value.ToString();

                Key newKey = new Key();

                Agent ag1 = protocol.Agents.Where(p => p.Id == agt1).ElementAt(0);
                Agent ag2 = protocol.Agents.Where(p => p.Id == agt2).ElementAt(0);

                newKey.agents.Add(ag1);
                newKey.agents.Add(ag2);

                if ((protocol.Keys.Where(p => p.agents.ElementAt(0).Id == agt1 && p.agents.ElementAt(1).Id == agt2)).Count() != 0)
                {
                    Key foundKey = protocol.Keys.Where(p => p.agents.ElementAt(0).Id == agt1 && p.agents.ElementAt(1).Id == agt2).ElementAt(0);
                    statement = foundKey;
                }
                else
                {
                    protocol.Keys.Add(newKey);
                    statement = newKey;
                }
            }

			if (jsonKnowledge.Property("nonce") != null)
            {
                JObject jNonce = new JObject(jsonKnowledge.Property("nonce"));
                JObject jNonceValue = (JObject)jNonce.Properties().Select(p => p.Value).ElementAt(0);

                string id = jNonceValue.Property("id").Value.ToString();
                bool fresh = Boolean.Parse(jNonceValue.Property("fresh").Value.ToString());

                Nonce newNonce = new Nonce(id);

                newNonce.Fresh = fresh;

                statement = newNonce;
            }
			
            if (jsonKnowledge.Property("knowledge") != null)
            {
                JObject jKnowledge = new JObject(jsonKnowledge.Property("knowledge"));
                statement = ParseKnowledge((JObject)jKnowledge.Properties().Select(p => p.Value).ElementAt(0));
            }

				
            if (agent != null)
                return new Knowledge(agent, knowledgeType, statement);
			else	
				return null;
        }

        private void analyzeButton_Click(object sender, EventArgs e)
        {
            btnVerify_Click(null, null);
            txtProtocol.Text = protocol.runProtocol();
        }
    }
}
