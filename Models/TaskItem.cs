using System;
using System.Linq;
using System.Collections.Generic;

namespace XamarinForms
{
    public class TaskItem 
    {         
  
        public int Id { get; set; }              // Unique identifier
        public string Title { get; set; }        // Task title
        public string Description { get; set; }  // Task details
        public string Status {get; set;}
        public DateTime DueDate { get; set; }    // Deadline
        public DateTime CreatedOn {get; set;}
        public bool IsCompleted { get; set; }    // Status
    }	
    
}