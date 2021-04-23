using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Models.ClientModels
{
    public class Fix
    {
        [Key]
        public int ID { get; set; }
        public string ClientID { get; set; }
        public string EmpWhoAcceptID { get; set; }
        public DateTime DateOfAdmission { get; set; } = DateTime.UtcNow;
        public bool IsReleased { get; set; }
        public DateTime? DateOfRelease { get; set; }
        public string ItemToFix { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string WhatAccesory { get; set; } //Converter split (,)
        public string WhatToFix { get; set; } //Converter
        public float CostOfRepair { get; set; }
        public string PublicComments { get; set; } //Converter
        public string PrivateComments { get; set; } //Converter
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string PasswordIfExist { get; set; }
        public string Barcode { get; set; } //10 string length
    }
}
