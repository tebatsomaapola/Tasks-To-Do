using System;
using System.IO;
using Xamarin.Forms;

namespace XamarinForms
{
	public partial class EditTaskPage : ContentPage
	{
		private TaskItem _task;

    public EditTaskPage(TaskItem task)
    {
        InitializeComponent();
        
        _task = task;
        
        BindingContext = _task;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // Update values from UI
        _task.Title = titleEntry.Text;
        _task.Description = descEditor.Text;
        _task.DueDate = dueDatePicker.Date;
        _task.Status = statusPicker.SelectedItem?.ToString() ?? "Pending";
        _task.IsCompleted = _task.Status == "Complete";

        // Save to DB
        App.Database.UpdateTask(_task);

        await DisplayAlert("Updated", "Task updated successfully.", "OK");
       
        await Navigation.PushModalAsync(new NavigationPage(new MainPage()));

    }
    
    async void OnCancel(object sender, EventArgs e)
        {
          await Navigation.PushModalAsync(new MainPage());
     	}
}
}