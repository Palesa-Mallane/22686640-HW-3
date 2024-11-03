using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _22686640_Homework_Assignment_3.Models
{
    public class report
    {
        public int reportId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }  
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
    
    }
}