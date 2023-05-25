using QuizMania.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizMania.Models
{
    public class Report
    {
        public Quiz quiz { get; set; }
        public Result res { get; set; }
        public User user { get; set; }
    }
}