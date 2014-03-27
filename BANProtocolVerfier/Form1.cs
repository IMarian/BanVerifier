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
            }

            /** Read the file line by line and put its content to the text box */
            using (StreamReader sr = new StreamReader(path))
            {
                string fileContent = sr.ReadToEnd();
                txtProtocol.Text = fileContent;
            }
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            protocol = new Protocol();

            if (txtProtocol.Text != "")
            {
                /** Parse the contents of the json-formatted protocol */
                dynamic jsonProtocol = JsonConvert.DeserializeObject(txtProtocol.Text);
                dynamic jsonAgents = jsonProtocol.agents;
                dynamic jsonPhases = jsonProtocol.phases;

                foreach (dynamic jsonAgent in jsonAgents)
                {
                    Agent newAgent = new Agent();
                    JObject jAgent = new JObject(jsonAgent);
                    newAgent.Id = jAgent.Properties().Select(p => p.Name).ElementAt(0);
                    JObject jAgentValue = (JObject)jAgent.Properties().Select(p => p.Value).ElementAt(0);

                    if (jAgentValue.Property("nonces") != null)
                    {
                        JObject jNonces = new JObject(jAgentValue.Property("nonces"));
                        JArray noncesArray = (JArray)jNonces.Properties().Select(p => p.Value).ElementAt(0);

                        foreach (var nonce in noncesArray)
                        {
                            newAgent.nonces.Add(nonce.ToString());
                        }
                    }

                    if (jAgentValue.Property("keys") != null)
                    {
                        JObject jKeys = new JObject(jAgentValue.Property("keys"));
                        JArray keysArray = (JArray)jKeys.Properties().Select(p => p.Value).ElementAt(0);

                        foreach (var key in keysArray)
                        {
                            newAgent.keys.Add(key.ToString());
                        }
                    }

                    protocol.Agents.Add(newAgent);
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
                        Agent foundAgent = (Agent)protocol.Agents.Select(a => a.Id == id);

                        phase.Sender = foundAgent;
                    }

                    if (jPhaseValue.Property("receiver") != null)
                    {
                        JObject jReceiver = new JObject(jPhaseValue.Property("receiver"));
                        string id = (string)jReceiver.Properties().Select(p => p.Value).ElementAt(0);
                        Agent foundAgent = (Agent)protocol.Agents.Select(a => a.Id == id);

                        phase.Receiver = foundAgent;
                    }

                    if (jPhaseValue.Property("message") != null)
                    {
                        JObject jMessage = new JObject(jPhaseValue.Property("message"));
                        JObject jMessageValue = (JObject)jMessage.Properties().Select(p => p.Value).ElementAt(0);

                        Message newMessage = new Message();

                        if (jMessageValue.Property("ids") != null)
                        {
                            JObject jIds = new JObject(jPhaseValue.Property("ids"));
                            JArray idsArray = (JArray)jIds.Properties().Select(p => p.Value).ElementAt(0);
                            foreach (string id in idsArray)
                            {
                                newMessage.Agents.Add((Agent)protocol.Agents.Select(a => a.Id == id));
                            }
                        }

                        if (jMessageValue.Property("nonces") != null)
                        {
                            JObject jNonces = new JObject(jPhaseValue.Property("nonces"));
                            JArray noncesArray = (JArray)jNonces.Properties().Select(p => p.Value).ElementAt(0);
                            foreach (string nonce in noncesArray)
                            {
                                newMessage.Nonces.Add(nonce);
                            }
                        }

                        //if (jMessageValue.Property("encrypted") != null)
                        //{
                        //    JObject jEncrypted = new JObject(jPhaseValue.Property("encrypted"));
                        //    string encryptionKey = (string)jEncrypted.Properties().Select(p => p.Value).ElementAt(0);
                        //    newMessage.Encrypted = encryptionKey;
                        //}

                        //if (jMessageValue.Property("keys") != null)
                        //{
                        //    JObject jKeys = new JObject(jPhaseValue.Property("keys"));
                        //    JArray keysArray = (JArray)jKeys.Properties().Select(p => p.Value).ElementAt(0);
                        //    foreach (string key in keysArray)
                        //    {
                        //        newMessage.Keys.Add(key);
                        //    }
                        //}
                    }
                }
            }
            else
            {
                throw new Exception("Protocol not loaded, please load a protocol json-formatted file");
            }
        }
    }
}
