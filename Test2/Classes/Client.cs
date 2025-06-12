using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Markup;

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
            set 
            { 
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("le nom ne peut pas être vide");
                }

                nomClient = value;
            }
        }

        private string prenomClient;
        public string PrenomClient
        {
            get { return prenomClient; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("le prénom ne peut pas être vide");
                }

                prenomClient = value;
            }
        }

        private string mailClient;
        public string MailClient
        {
            get { return mailClient; }
            set 
            {
                if (!Regex.IsMatch(value, @"^[a-z0-9]{1,25}(\.[a-z0-9]{1,25})?@[a-z0-9.-]{1,20}\.[a-z]{2,6}$"))
                {
                    throw new ArgumentException("mail invalide");
                }
                mailClient = value;
            }
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
