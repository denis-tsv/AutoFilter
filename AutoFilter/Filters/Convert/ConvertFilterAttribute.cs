using System;

namespace AutoFilter.Filters.Convert
{
    public class ConvertFilterAttribute : FilterPropertyAttribute
    {
        public ConvertFilterAttribute(Type destinationType)
        {
            if (destinationType.GetInterface(nameof(IFilverValueConverter)) == null)
                throw new ArgumentException($"Not implemented {nameof(IFilverValueConverter)}");

            ConverterType = destinationType;
        }

        public Type ConverterType { get; }

        protected override object GetPropertyValue(object filterPropertyValue, object filter)
        {
            var converter = FilterValueConvertersCache.GetConverter(ConverterType);
            return converter.Convert(filterPropertyValue);
        }
    }
}
