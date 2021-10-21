using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationService.Models
{
    public class Presentation
    {
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public StatusPresentation Status { get; set; }
        public string Description { get; set; }
        public List<Visitor> visitors = new List<Visitor>();
    }

    public enum StatusPresentation
    {
        Open,
        Close
    }
}
