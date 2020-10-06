using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class BackgroundData
    {
        public string Description { get; set; }
        public bool IsRunning { get; set; }

        public BackgroundData(string description, bool isRunning)
        {
            Description = description;
            IsRunning = isRunning;
        }
    }
}
