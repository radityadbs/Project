using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Dtos
{
    public class ResProfileDto
    {
        public string Nik { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Address { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string DesaId { get; set; } = "";
        public string DesaName { get; set; } = "";
        public string? PhotoPath { get; set; }
    }
}