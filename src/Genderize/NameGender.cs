using System;
using System.Collections.Generic;
using System.Text;

namespace Genderize
{
    public class NameGender
    {
        public string Name {get; set; }
        public Gender? Gender { get; set; } 
        public float Probability { get; set; }
        public int Count { get; set; }
    }
}
