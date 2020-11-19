using HelloWPF;
using HelloWPF.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TextileApp
{
    /// <summary>
    /// Interaction logic for DashboardControl.xaml
    /// </summary>
    public partial class DashboardControl : UserControl
    {
        public DashboardControl()
        {
            InitializeComponent();

            populateTableDashboard(DateTime.Now.ToString("dd/M/yyyy"));
            populateWeekAndMonthTable();
        }

        private void populateTableDashboard(String date)
        {
            List<Invoice> invoices = new List<Invoice>();
            using (SQLiteConnection dbConnection = new SQLiteConnection(App.productDatabasePath))
            {
                dbConnection.CreateTable<Invoice>();
                invoices = dbConnection.Table<Invoice>().ToList();
                var sql_cmd = dbConnection.CreateCommand("SELECT * FROM 'Invoice' WHERE Date=='" + date + "'");
                invoices = sql_cmd.ExecuteQuery<Invoice>();
            }

            daily_sales_text.Text = invoices.Count.ToString();
            invoices = invoices.FindAll(invoice => invoice.Total != null);
            int sum = invoices.Sum(invoice => int.Parse(invoice.Total));
            daily_amount_text.Text = sum.ToString();
        }

        private void populateWeekAndMonthTable()
        {
            List<Invoice> weeklyInvoices = new List<Invoice>();
            List<Invoice> monthlyInvoices = new List<Invoice>();
            DateTime day = DateTime.Now;
            String weeklyQueryString = "";
            String monthlyQueryString = "";
            do
            {
                weeklyQueryString += "Date == '"+day.ToString("dd/M/yyyy")+"'";
                if (!day.DayOfWeek.Equals(DayOfWeek.Monday))
                {
                    weeklyQueryString += " OR ";
                }
                day = day.AddDays(-1);
            } while (!day.AddDays(+1).DayOfWeek.Equals(DayOfWeek.Monday));

            do
            {
                monthlyQueryString += "Date == '" + day.ToString("dd/M/yyyy") + "'";
                if (day.Day != 1)
                {
                    monthlyQueryString += " OR ";
                }
                day = day.AddDays(-1);
            } while (day.AddDays(+1).Day != 1);

            using (SQLiteConnection dbConnection = new SQLiteConnection(App.productDatabasePath))
            {
                dbConnection.CreateTable<Invoice>();
                weeklyInvoices = dbConnection.Table<Invoice>().ToList();
                var sql_cmd = dbConnection.CreateCommand("SELECT * FROM 'Invoice' WHERE "+ weeklyQueryString);
                weeklyInvoices = sql_cmd.ExecuteQuery<Invoice>();

                monthlyInvoices = dbConnection.Table<Invoice>().ToList();
                sql_cmd = dbConnection.CreateCommand("SELECT * FROM 'Invoice' WHERE " + monthlyQueryString);
                monthlyInvoices = sql_cmd.ExecuteQuery<Invoice>();
            }

            weekly_sales_text.Text = weeklyInvoices.Count.ToString();
            weeklyInvoices = weeklyInvoices.FindAll(invoice => invoice.Total != null);
            int sum = weeklyInvoices.Sum(invoice => int.Parse(invoice.Total));
            weekly_amount_text.Text = sum.ToString();

            monthly_sales_text.Text = monthlyInvoices.Count.ToString();
            monthlyInvoices = monthlyInvoices.FindAll(invoice => invoice.Total != null);
            sum = monthlyInvoices.Sum(invoice => int.Parse(invoice.Total));
            monthly_amount_text.Text = sum.ToString();
        }

        private void dateChangedEvent(Object sender, SelectionChangedEventArgs e)
        {
            DateTime date = (DateTime)dateBox.SelectedDate;
            populateTableDashboard(date.ToString("dd/M/yyyy"));
        }
    }
}
