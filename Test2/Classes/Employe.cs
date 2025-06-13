using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppliNicolas.Classes
{
    public class Employe
    {
        public int NumEmploye { get; set; }
        public int NumRole { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Login { get; set; }
        public string MotDePasse { get; set; }

        public ObservableCollection<VinRecherche> HistoriqueRecherches { get; set; } = new ObservableCollection<VinRecherche>();

        public bool EstResponsable => NumRole == 2;

        public static Employe Connexion(string login, string password)
        {
            string sql = "SELECT * FROM employe WHERE login = @login";
            using (NpgsqlCommand cmd = new NpgsqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@login", login);

                DataTable dt = ConnexionBD.Instance.ExecuteSelect(cmd);

                if (dt.Rows.Count >= 1)
                {
                    DataRow row = dt.Rows[0];
                    return new Employe
                    {
                        NumEmploye = (int)row["numemploye"],
                        NumRole = (int)row["numrole"],
                        Nom = (string)row["nom"],
                        Prenom = (string)row["prenom"],
                        Login = (string)row["login"],
                        MotDePasse = (string)row["mdp"]
                    };
                }
            }

            return null;
        }
    }


}
