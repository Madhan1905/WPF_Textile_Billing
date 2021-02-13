using HelloWPF;
using HelloWPF.Classes;
using MongoDB.Driver;
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
            MongoClient dbClient = new MongoClient("mongodb://Thinkershut:Dev123@localhost:27017/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false");
            var database = dbClient.GetDatabase("main_db");
            List<String> names = database.ListCollectionNames().ToList<String>();
            if (!names.Contains("Invoices"))
            {
                database.CreateCollection("Invoices");
            }
            var collection = database.GetCollection<Invoice>("Invoices");
            var filter = Builders<Invoice>.Filter.Eq(i => i.Date, date);
            invoices = collection.Find<Invoice>(filter).Limit(15).ToList<Invoice>();

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
            List<String> weekDates = new List<String>();
            List<String> monthDates = new List<String>();
            do
            {
                weekDates.Add(day.ToString("dd/M/yyyy"));
                day = day.AddDays(-1);
            } while (!day.AddDays(+1).DayOfWeek.Equals(DayOfWeek.Monday));

            day = DateTime.Now;
            do
            {
                monthDates.Add(day.ToString("dd/M/yyyy"));
                day = day.AddDays(-1);
            } while (day.AddDays(+1).Day != 1);

            MongoClient dbClient = new MongoClient("mongodb://Thinkershut:Dev123@localhost:27017/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false");
            var database = dbClient.GetDatabase("main_db");
            List<String> names = database.ListCollectionNames().ToList<String>();
            if (!names.Contains("Invoices"))
            {
                database.CreateCollection("Invoices");
            }
            var collection = database.GetCollection<Invoice>("Invoices");
            var filter = Builders<Invoice>.Filter.In("Date", weekDates);
            weeklyInvoices = collection.Find<Invoice>(filter).ToList<Invoice>();

            filter = Builders<Invoice>.Filter.In("Date", monthDates);
            monthlyInvoices = collection.Find<Invoice>(filter).ToList<Invoice>();

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
