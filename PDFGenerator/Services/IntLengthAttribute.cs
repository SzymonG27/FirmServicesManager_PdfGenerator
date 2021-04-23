using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Services
{
    public class IntLengthAttribute : ValidationAttribute
    {
        public IntLengthAttribute(int min, int max, bool isRequired)
        {
            Min = min;
            Max = max;
            IsRequired = isRequired;
        }
        public int Min { get; }
        public int Max { get; }
        public bool IsRequired { get; }
        public override bool IsValid(object value)
        {
            if (IsRequired == false)
            {
                return true;
            }
            int minConvert = Min.ToString().Length;
            int maxConvert = Max.ToString().Length;
            int valConvert = value.ToString().Length;
            if (valConvert >= minConvert && maxConvert <= valConvert)
            {
                return true;
            }
            return false;
        }
    }
}
