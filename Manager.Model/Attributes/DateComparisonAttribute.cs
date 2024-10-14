using System;
using System.ComponentModel.DataAnnotations;

namespace Manager.Model.Attributes
{
    public class DateComparisonAttribute : ValidationAttribute
    {
        private readonly string _startDate;
        public DateComparisonAttribute(string startDate)
        {
            _startDate = startDate;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var endDate = validationContext.ObjectType.GetProperty(validationContext.DisplayName);
            if (endDate == null)
                return new ValidationResult($"Unknown property: {validationContext.MemberName}");

            var startDateProperty = validationContext.ObjectType.GetProperty(_startDate);
            if (startDateProperty == null)
                return new ValidationResult($"Unknown property: {_startDate}");

            var startDateValue = (DateTime?)startDateProperty.GetValue(validationContext.ObjectInstance);
            var endDateValue = (DateTime?)endDate.GetValue(validationContext.ObjectInstance);

            if (startDateValue == null || endDateValue == null)
                return ValidationResult.Success;

            if (startDateValue > endDateValue)
                return new ValidationResult("Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu");

            return ValidationResult.Success;
        }
    }
}
