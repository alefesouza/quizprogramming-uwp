using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public class Home
    {
        public int Id { get; set; }
        public String Title { get; set; }

        public Home(int Id, String Title)
        {
            this.Id = Id;
            this.Title = Title;
        }
    }
}
