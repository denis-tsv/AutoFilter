namespace AutoFilter.Filters.Convert
{
    public class NullableIntToNullableBooleanConverter : IFilverValueConverter
    {
        public object Convert(object v)
        {
            var value = (int?)v;
            if (!value.HasValue) return null;

            return value.Value == 1;
        }
    }
}
