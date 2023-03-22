namespace AutoFilter;

public interface IRange
{
    object? From { get; }
    object? To { get; }
}

public class Range<T> : IRange
    where T : struct
{
    public T? From { get; set; }
    public T? To { get; set; }

    object? IRange.From => From;
    object? IRange.To => To;
}