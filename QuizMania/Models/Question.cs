using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizMania.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string QuestionName { get; set; }
        public List<Option> Options { get; set; }
        public string Answer { get; set; }
    }
}