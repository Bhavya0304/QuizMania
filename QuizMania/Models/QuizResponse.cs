using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizMania.Models
{
    public class QuizResponseList
    {
        public int quesId { get; set; }
        public int optionId { get; set; }
    }

    public class QuizResponse
    {
        public int QuizId { get; set; }
        public List<QuizResponseList> ResponseLists { get; set; }
    }
}