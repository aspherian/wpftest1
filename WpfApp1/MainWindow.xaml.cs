using System;
using System.Data;
using System.Windows;
using Npgsql;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public NpgsqlConnection Connection
        {
            get
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
                return con;
            }
        }
        public void InitializeDataTable()
        {
            NpgsqlCommand command = new NpgsqlCommand($"SELECT * FROM test", Connection);
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
            NpgsqlCommand command = new NpgsqlCommand("INSERT INTO test(name, email, age) VALUES (@name, @email, @age)", Connection);
            command.Parameters.Add(new NpgsqlParameter("@name", textBox_name.Text));
            command.Parameters.Add(new NpgsqlParameter("@email", textBox_email.Text));
            command.Parameters.Add(new NpgsqlParameter("@age", Convert.ToInt32(textBox_age.Text)));
            command.ExecuteNonQuery();
            InitializeDataTable();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlCommand command = new NpgsqlCommand("TRUNCATE test RESTART IDENTITY;", Connection);
            command.ExecuteNonQuery();
            InitializeDataTable();
        }
    }
}
