using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Xamarin.Forms;

namespace XamarinForms
{
	public partial class DashboardPage : ContentPage
	{
		public DashboardPage()
        {
           InitializeComponent();

         // Populate month filter
           var months = Enumerable.Range(1, 12)
           .Select(m => new DateTime(DateTime.Now.Year, m, 1).ToString("MMM yyyy"))
           .ToList();

            months.Insert(0, "All");
            DashboardMonthFilter.ItemsSource = months;

          // Default selections
            DashboardStatusFilter.SelectedIndex = 0;
            DashboardMonthFilter.SelectedIndex = 0;
        }
        
        protected override void OnAppearing()
        {
          base.OnAppearing();
          OnDashboardFilterChanged(null, null); // Apply filters
        }

       async void OnCancel(object sender, EventArgs e)
        {
        	await Navigation.PushModalAsync(new MainPage());
        }

       private void OnDashboardFilterChanged(object sender, EventArgs e)
       {
          if (DashboardStatusFilter.SelectedItem == null || DashboardMonthFilter.SelectedItem == null)
            return;

          string selectedStatus = DashboardStatusFilter.SelectedItem.ToString();
          string selectedMonth = DashboardMonthFilter.SelectedItem.ToString();

          var tasks = App.Database.GetTasks();

        // Filter by status
          if (selectedStatus != "All")
           tasks = tasks.Where(t => t.Status == selectedStatus).ToList();

        // Filter by month
          if (selectedMonth != "All")
          {
            DateTime monthDate = DateTime.ParseExact(selectedMonth, "MMM yyyy", null);

            tasks = tasks.Where(t =>
            t.DueDate.Month == monthDate.Month &&
            t.DueDate.Year == monthDate.Year).ToList();
          }

           // Update dashboard numbers
           TotalTasksLabel.Text = tasks.Count.ToString();
           CompletedTasksLabel.Text = tasks.Count(t => t.IsCompleted).ToString();
           OverdueTasksLabel.Text = tasks.Count(t => t.DueDate < DateTime.Today).ToString();
           ThisMonthTasksLabel.Text = tasks.Count(t =>
           t.DueDate.Month == DateTime.Now.Month &&
           t.DueDate.Year == DateTime.Now.Year).ToString();
          }

	} 

}