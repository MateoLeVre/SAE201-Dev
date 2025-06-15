using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppliNicolas.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppliNicolas.Fenetre;
using Npgsql;
using System.Data;

namespace AppliNicolas.Classes.Tests
{
    [TestClass()]
    public class ClientTests
    {
        [TestMethod()]
        public void RecupereClientDansBDDTest()
        {
            ConnexionBD.Instance.ConfigurerConnexion("home", "home");

            Client client = new Client();

            List<Client> lesClients = client.RecupereClientDansBDD();
            Assert.IsInstanceOfType(lesClients, typeof(List<Client>), "Le try block devrait retourner une List<Client>");
        }

        [TestMethod()]
        public void insererClientDansBDDTest()
        {
            ConnexionBD.Instance.ConfigurerConnexion("home", "home");

            Client clientNormal = new Client("Moreaux", "noah", "moreaux.noah@email.com");
            Client clientMailIdentique = new Client("bobo", "popo", "sophie.martin@email.com");
            ConnexionBD.Instance.ConfigurerConnexion("home", "home");

            clientNormal.InsererNouveauClient(clientNormal.NomClient, clientNormal.PrenomClient, clientNormal.MailClient);
            Assert.ThrowsException<ArgumentException>(() => clientMailIdentique.InsererNouveauClient(clientMailIdentique.NomClient, clientMailIdentique.PrenomClient, clientMailIdentique.MailClient));

            using (NpgsqlCommand verifClientNormal = new NpgsqlCommand("SELECT COUNT(*) FROM client WHERE mailclient = @email"))
            {
                verifClientNormal.Parameters.AddWithValue("@email", clientNormal.MailClient);
                DataTable dt1 = ConnexionBD.Instance.ExecuteSelect(verifClientNormal);
                int count1 = Convert.ToInt32(dt1.Rows[0][0]);
                Assert.AreEqual(1, count1, "Le client normal devrait être trouvé en base");
            }
        }

        [TestMethod()]
        public void SupprimerClientDansBDDTest()
        {
            Client clientNormal = new Client("Moreaux", "noah", "moreaux.noah@email.com");
            ConnexionBD.Instance.ConfigurerConnexion("home", "home");

            // Récupérer l'ID du client
            int numeroClient = 0;
            using (NpgsqlCommand recupID = new NpgsqlCommand("SELECT numclient FROM client WHERE nomclient = @nom"))
            {
                recupID.Parameters.AddWithValue("@nom", clientNormal.NomClient);
                DataTable dt1 = ConnexionBD.Instance.ExecuteSelect(recupID);
                numeroClient = Convert.ToInt32(dt1.Rows[0]["numclient"]);
            }

            // Supprimer le client
            clientNormal.SupprimerClientDeLaBase(numeroClient);

            // Vérifier que le client a bien été supprimé
            using (NpgsqlCommand verifClientSupp = new NpgsqlCommand("SELECT COUNT(*) FROM client WHERE numclient = @numclient"))
            {
                verifClientSupp.Parameters.AddWithValue("@numclient", numeroClient);
                DataTable dt2 = ConnexionBD.Instance.ExecuteSelect(verifClientSupp);
                int count = Convert.ToInt32(dt2.Rows[0][0]);

                Assert.AreEqual(0, count, "Le client devrait être supprimé (aucun enregistrement trouvé)");
            }
        }

        [TestMethod()]
        public void ModifierClientDansBDDTest()
        {
            Client clientNormal = new Client("Moreaux", "noah", "moreaux.noah@email.com");
            ConnexionBD.Instance.ConfigurerConnexion("home", "home");
            int numeroClient = 0;

            using (NpgsqlCommand recupID = new NpgsqlCommand("SELECT numclient FROM client WHERE nomclient = @nom"))
            {
                recupID.Parameters.AddWithValue("@nom", clientNormal.NomClient);
                DataTable dt1 = ConnexionBD.Instance.ExecuteSelect(recupID);
                numeroClient = Convert.ToInt32(dt1.Rows[0]["numclient"]);
            }

            clientNormal.ModifierClient(numeroClient, "koko", clientNormal.PrenomClient, clientNormal.MailClient);

            using (NpgsqlCommand verifClientModifier = new NpgsqlCommand("SELECT nomclient FROM client WHERE nomclient = @nom"))
            {
                verifClientModifier.Parameters.AddWithValue("@nom", "koko");
                DataTable dt1 = ConnexionBD.Instance.ExecuteSelect(verifClientModifier);

                Assert.AreEqual("koko", dt1.Rows[0]["nomclient"].ToString(), "Le nom du client devrait être 'koko'");
            }
        }
    }
}