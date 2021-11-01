using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationService.Models
{
    [Table("presentation")]
    public class Presentation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public StatusPresentation Status { get; set; }
        public string Description { get; set; }
        public Visitor visitors { get; set; }

        public enum StatusPresentation
        {
            Open,
            Close
        }
    }
}
