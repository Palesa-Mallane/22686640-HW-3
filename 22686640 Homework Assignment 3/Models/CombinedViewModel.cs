using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using PagedList.Mvc;


namespace _22686640_Homework_Assignment_3.Models
{
    public class CombinedViewModel
    {
        public IPagedList<author> authors { get; set; }
        public IPagedList<book> books { get; set; }
        public IPagedList<student> students { get; set; }
        public IPagedList<type> types { get; set; }
        public IPagedList<borrow> borrows { get; set; }

        public List<PopularBookViewModel> popularBooks { get; set; }
        public List<report> reports { get; set; }
    }
}