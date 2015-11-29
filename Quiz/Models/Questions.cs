using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public class Questions
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int IsCorrect { get; set; }

        public Questions(int Id, string Text, int IsCorrect)
        {
            this.Id = Id;
            this.Text = Text;
            this.IsCorrect = IsCorrect;
        }
    }
}
