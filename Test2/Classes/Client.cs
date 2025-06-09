using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace AppliNicolas.Classes
{
    public class Client
    {
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
    }
}
