using System;
using System.Collections.Generic;
using System.Data;
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
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public NpgsqlConnection GetConnection()
        {
            NpgsqlConnection con = new NpgsqlConnection("Host=localhost;Port=5432;Database=test;Username=postgres;Password=1234");
            try
            {
                con.Open();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Error");
                con.Close();
            }
            return con;
        }
        public void InitializeDataTable()
        {
            string sql = $"SELECT * FROM test";
            NpgsqlCommand command = new NpgsqlCommand(sql, GetConnection());
            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader(CommandBehavior.CloseConnection));
            DataGrid1.DataContext = dt;
            DataGrid1.ItemsSource = dt.AsDataView();
        }
        public MainWindow()
        {
           InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           InitializeDataTable();
        }
        private void click(object sender, RoutedEventArgs e)
        {
            string sql = "INSERT INTO test(name, email, age) VALUES (@name, @email, @age)";
            NpgsqlCommand command = new NpgsqlCommand(sql, GetConnection());
            command.Parameters.Add(new NpgsqlParameter("@name", textBox_name.Text));
            command.Parameters.Add(new NpgsqlParameter("@email", textBox_email.Text));
            command.Parameters.Add(new NpgsqlParameter("@age", Convert.ToInt32(textBox_age.Text)));
            command.ExecuteNonQuery();
            InitializeDataTable();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string sql = "TRUNCATE test RESTART IDENTITY;";
            NpgsqlCommand command = new NpgsqlCommand(sql, GetConnection());
            command.ExecuteNonQuery();
            InitializeDataTable();
        }
    }
}
