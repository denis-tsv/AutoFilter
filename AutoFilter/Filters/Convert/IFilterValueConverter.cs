namespace AutoFilter.Filters.Convert;

public interface IFilterValueConverter
{
    object? Convert(object? value);
}