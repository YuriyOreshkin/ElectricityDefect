using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectricityDefect.WebUI.Models
{
    public class TemplateViewModel
    {
        [Required]
        [DisplayName("Получатели")]
        public string[] recipients { get; set; }
        [DisplayName("Тема")]
        [Required]
        public string subject { get; set; }
        [DisplayName("Тело")]
        public string body { get; set; }
    }
}