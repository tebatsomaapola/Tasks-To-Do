using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace XamarinForms
{
    public partial class MainPage : ContentPage
    {
        private List<TaskItem> allTasks;

        public MainPage()
        {
           InitializeComponent();

           // Load tasks
           allTasks = App.Database.GetTasks();
           tasksList.ItemsSource = allTasks;

          // Populate month filter
           var months = Enumerable.Range(1, 12)
           .Select(m => new DateTime(DateTime.Now.Year, m, 1).ToString("MMM yyyy"))
           .ToList();

           months.Insert(0, "All"); // Add "All" option
           MonthFilter.ItemsSource = months;

           // Default filters
           StatusFilter.SelectedIndex = 0;
           MonthFilter.SelectedIndex = 0;
        }
        
        protected override void OnAppearing()
        {
           
           // base.OnAppearing();

            //allTasks = App.Database.GetTasks();
          //  tasksList.ItemsSource = null;
           // tasksList.ItemsSource = allTasks;
           base.OnAppearing();

          allTasks = App.Database.GetTasks();
          OnFilterChanged(null, null); // 
        }

        private void OnFilterChanged(object sender, EventArgs e)
        {
          if (StatusFilter.SelectedItem == null || MonthFilter.SelectedItem == null)
            return;

          string selectedStatus = StatusFilter.SelectedItem.ToString();
          string selectedMonth = MonthFilter.SelectedItem.ToString();

          var filtered = allTasks;

    
         if (selectedStatus != "All")
         {
            filtered = filtered
            .Where(t => t.Status?.Trim().ToLower() == selectedStatus.Trim().ToLower())
            .ToList();
         }

    
         if (selectedMonth != "All")
         {
           DateTime monthDate = DateTime.ParseExact(selectedMonth, "MMM yyyy", null);

             filtered = filtered
            .Where(t => t.DueDate.Month == monthDate.Month &&
                        t.DueDate.Year == monthDate.Year)
            .ToList();
         }

            tasksList.ItemsSource = filtered;
        }


        
        private async void OnAddTaskClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TaskCreationPage());
        }   
    

         private async void OnTaskTapped(object sender, ItemTappedEventArgs e)
         {
           if (e.Item is TaskItem task)
              {
            string details =
            $"Title: {task.Title}\n" +
            $"Description: {task.Description}\n" +
            $"Status: {task.Status}\n" +
            $"Due: {task.DueDate:dd MMM yyyy}";

        // If expired, only allow Delete
        string action;
        if (task.DueDate.Date < DateTime.Today)
        {
            action = await DisplayActionSheet(details, "Cancel", null, "Delete");
        }
        else
        {
            action = await DisplayActionSheet(details, "Cancel", null, "Edit", "Delete");
        }

        if (action == "Edit")
        {    
            await Navigation.PushModalAsync(new EditTaskPage(task)); // pass full task
        }
        else if (action == "Delete")
        {
            bool confirm = await DisplayAlert("Confirm Delete",
                $"Delete task '{task.Title}'?", "Yes", "No");

            if (confirm)
            {
                App.Database.DeleteTask(task.Id); // delete by Id
                allTasks.Remove(task);
                tasksList.ItemsSource = null;
                tasksList.ItemsSource = allTasks;
            }
          }
        }

            ((ListView)sender).SelectedItem = null;
       }

        
    
   private async void OnDashboardClicked(object sender, EventArgs e)
   {
    //await Navigation.PushAsync(new DashboardPage());
    await Navigation.PushModalAsync(new NavigationPage(new DashboardPage()));

   }


}
 public class DueDateColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value == null)
            return Color.Default;

        if (value is DateTime dueDate)
        {
            if (dueDate.Date < DateTime.Today)
                return Color.Red;
            else if (dueDate.Date == DateTime.Today)
                return Color.Yellow;
            else
                return Color.Green;
        }

        return Color.Default;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


}
