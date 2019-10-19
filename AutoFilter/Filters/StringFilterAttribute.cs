namespace AutoFilter.Filters
{
    public class StringFilterAttribute : FilterPropertyAttribute
    {
        public StringFilterAttribute(StringFilterCondition condition)
        {
            StringFilter = condition;
        }        
    }
}
