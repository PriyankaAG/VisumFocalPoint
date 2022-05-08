using System;
using System.Collections.Generic;
using System.Text;

namespace Visum.Services.Mobile.Entities
{
    class MenuItem
    {
        public string Label { get; set; }
        public string Description { get; set; }
        public Type Activity { get; set; }
        public int Icon { get; set; }
    }
}
