using System;

namespace Visum.Services.Mobile.Entities
{
    public class CompatibleResults
    {
        public CompatibleResults()
        {

        }
        public int APILowVersion { get; set; }
        public int APIHighVersion { get; set; }
        public Version FocalPtVersion { get; set; }
        public string LocalIP { get; set; }
    }
}
