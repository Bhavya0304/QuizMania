using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuizMania.DBContext
{
    [MetadataType(typeof(MetaClass))]
    public partial class User
    {
        internal class MetaClass
        {
            [Required]
            public string Username { get; set; }
            [Required]
            public string Password { get; set; }
            public string Name { get; set; }
        }
    }
}