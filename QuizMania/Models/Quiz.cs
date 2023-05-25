using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuizMania.DBContext
{
    [MetadataType(typeof(MetaClass))]
    public partial class Quiz
    {
        internal class MetaClass
        {
            
           [Required]
            public string QuizName { get; set; }
        }
    }
}