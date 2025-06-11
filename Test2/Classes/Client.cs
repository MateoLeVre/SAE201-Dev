using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace AppliNicolas.Classes
{
    public class Client
    {

        public Client()
        {

        }

        public Client(int numClient, string nomClient, string prenomClient, string mailClient)
        {
            NumClient = numClient;
            NomClient = nomClient;
            PrenomClient = prenomClient;
            MailClient = mailClient;
        }


        private int numClient;
        public int NumClient
        {
            get { return numClient; }
            set { numClient = value; }
        }

        private string nomClient;
        public string NomClient
        {
            get { return nomClient; }
            set { nomClient = value; }
        }

        private string prenomClient;
        public string PrenomClient
        {
            get { return prenomClient; }
            set { prenomClient = value; }
        }

        private string mailClient;
        public string MailClient
        {
            get { return mailClient; }
            set { mailClient = value; }
        }

        public override bool Equals(object? obj)
        {
            return obj is Client client &&
                   NumClient == client.NumClient;
        }

        public List<Client> RecupereClientDansBDD()
        {
            List<Client> lesClients = new List<Client>();
            using (NpgsqlCommand commandeSelect = new NpgsqlCommand("select * from Client;"))
            {
                DataTable dt = ConnexionBD.Instance.ExecuteSelect(commandeSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesClients.Add(new Client((Int32)dr["numclient"], (string)dr["nomclient"],
                        (string)dr["prenomclient"], (string)dr["mailclient"]));
                }
            }
            return lesClients;
        }
    }
}
