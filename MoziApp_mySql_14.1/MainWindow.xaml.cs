using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySqlConnector;
using System.Data;

namespace MoziApp_mySql_14._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connString = "server=localhost;user=root;password=;database=mozi;";
        List<string> korhatar = new List<string>() {"Válassz...","KN","12","16","18"};
        List<string> mufaj = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            cbx_korhatar.ItemsSource = korhatar;
            cbx_korhatar.SelectedIndex = 0;
            mufajFeltolt();
            cbx_mufaj.ItemsSource = mufaj;
            cbx_mufaj.SelectedIndex = 0;
            Betoltes();
        }

        private void Betoltes()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string sqlBetolt = "SELECT * FROM filmek;";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(sqlBetolt,conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dg_filmek.ItemsSource = dt.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Hiba az adatok elérésekor!");
                }
            }
        }

        private void mufajFeltolt()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string sqlMufaj = "SELECT DISTINCT mufaj FROM `filmek` WHERE 1;";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(sqlMufaj,conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    mufaj.Add("Válassz...");
                    foreach (DataRow sor in dt.Rows)
                    {
                        mufaj.Add(sor["mufaj"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Hiba az adatok elérésekor!");
                }
            }
        }

        private void btn_hozzaad_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    if (tbx_cim.Text == "" || tbx_hossz.Text == "" )
                    {
                        MessageBox.Show("A mező kitöltése kötelező", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    else if (!szamE(tbx_hossz.Text) || int.Parse(tbx_hossz.Text) < 0 || int.Parse(tbx_hossz.Text) > 300)
                    {
                        MessageBox.Show("Hibás számadat!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    
                    if (cbx_korhatar.SelectedIndex == 0)
                    {
                        MessageBox.Show("Nem válsztottál korhatárt!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                        
                    
                    if (cbx_mufaj.SelectedIndex == 0)
                    {
                        MessageBox.Show("Nem válsztottál műfajt!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                        
                    conn.Open();
                    string sqlHozzaad = "INSERT INTO filmek(cim,hossz,korhatar,mufaj) VALUES (@c,@h,@k,@m);";
                    MySqlCommand cmdHozzaad = new MySqlCommand(sqlHozzaad, conn);
                    cmdHozzaad.Parameters.AddWithValue("@c", tbx_cim.Text);
                    cmdHozzaad.Parameters.AddWithValue("@h", tbx_hossz.Text);
                    cmdHozzaad.Parameters.AddWithValue("@k", cbx_korhatar.SelectedItem.ToString());
                    cmdHozzaad.Parameters.AddWithValue("@m", cbx_mufaj.SelectedItem.ToString());
                    cmdHozzaad.ExecuteNonQuery();
                    if (cmdHozzaad.ExecuteNonQuery()==0)
                    {
                        MessageBox.Show("Nem lett hozzáadva elem!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("A film hozzá lett adva az adatbázishoz!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        Betoltes();
                        mezokAlaphelyzetbe();
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Hiba az adatok elérésekor!");
                }
            }
        }

        private void mezokAlaphelyzetbe()
        {
            
        }

        private void btn_modosit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_torles_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_frissites_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rb_osszes_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void dg_filmek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dg_filmek.SelectedItem == null) 
            {
                return;
            }
            DataRowView drv = dg_filmek.SelectedItem as DataRowView;
            tbx_cim.Text = drv["cim"].ToString();
            tbx_hossz.Text = drv["hossz"].ToString();
            cbx_korhatar.SelectedItem = drv["korhatar"].ToString();
            cbx_mufaj.SelectedItem = drv["mufaj"].ToString();
        }

        private bool szamE(string szoveg)
        {
            return int.TryParse(szoveg, out int szam);
        }
    }
}
