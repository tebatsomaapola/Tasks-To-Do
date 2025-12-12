using System;
using System.IO;
using Xamarin.Forms;

namespace XamarinForms
{
	public partial class App : Application
	{
		
         static TaskDataBase database;

    public static TaskDataBase Database
    {
        get
        {
            if (database == null)
            {
                var path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                    "tasks.db3"
                );
                database = new TaskDataBase(path);
            }
            return database;
        }
    } 
        
    
	public App()
	{ 
		InitializeComponent();
		MainPage = new NavigationPage(new MainPage())
    {
        BarBackgroundColor = Color.DarkBlue,
        BarTextColor = Color.White
    };
		

	}

}}