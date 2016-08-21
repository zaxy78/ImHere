using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ImHereNonProfit.UI.Utils
{
    public class IntegerValidationRule : ValidationRule
    {
        public int Min { get; set; } = int.MinValue;

        public int Max { get; set; } = int.MaxValue;

        public string FieldName { get; set; }

        public string CustomMessage { get; set; }


        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int num = 0;

            if (!int.TryParse(value.ToString(), out num))
                return new ValidationResult(false, $"{FieldName} must contain an integer value.");

            if (num < Min || num > Max)
            {
                if (!String.IsNullOrEmpty(CustomMessage))
                    return new ValidationResult(false, CustomMessage);

                return new ValidationResult(false, $"{FieldName} must be between {Min} and {Max}.");
            }

            return new ValidationResult(true, null);
        }
    }
}
