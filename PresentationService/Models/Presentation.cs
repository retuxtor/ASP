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
        public int id { get; set; }
        public string name { get; set; }
        public DateTime time { get; set; }
        public StatusPresentation status { get; set; }
        public string description { get; set; }
        public int visitorsid { get; set; }

        public enum StatusPresentation
        {
            Open,
            Close
        }
    }
}
