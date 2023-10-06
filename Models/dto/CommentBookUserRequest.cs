using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoLibrary.Models.dto
{
    public class CommentBookUserRequest
    {

      public  string userid { get; set; }
     public   int bookid { get; set; }

     public   string description { get; set; }
    }
}
