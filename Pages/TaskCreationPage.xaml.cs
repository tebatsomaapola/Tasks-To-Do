using System;
using System.IO;
using Xamarin.Forms;


namespace XamarinForms
{
	public partial class TaskCreationPage : ContentPage
	{
		
		public bool IsEdit;
		public TaskCreationPage()
		{
			
		 InitializeComponent();
			
		}
			
		async void OnSaveClicked(object sender, EventArgs e)
        {
            // Create a new task from user input
            string selectedStatus = statusPicker.SelectedItem?.ToString() ?? "Pending";
            var task = new TaskItem
            {
                Title = titleEntry.Text,
                Description = descEditor.Text,
                DueDate = dueDatePicker.Date,
                Status = selectedStatus,
                IsCompleted = false
            };
            bool isV= Validate(task);
            if(isV) 
            {
            	//tDb = new TaskDataBase();
              App.Database.SaveTask(task);
              
	          await Navigation.PushModalAsync(new MainPage());
            }
            
            
           
        }
        async void OnCancel(object sender, EventArgs e)
        {
        	await Navigation.PushModalAsync(new MainPage());
        }
        public bool Validate(TaskItem task)
        {
        	if(task.Title == string.Empty)
        	{
        		DisplayAlert("Error","Task name required.","Ok");
        		return false;
        	}
        	else 
        	if(task.Description == string.Empty)
        	{
        		
        	    DisplayAlert("Error","Task description required","Ok");
     	    	return false;
        	}
        	else 
        	{
        		return true;
        	}
        	
        }
        
        
	}
}