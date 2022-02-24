namespace Tests.Models
{
    public class StringTestItem
    {
        public int Id { get; set; }

        public string NoAttribute { get; set; }

        public string ContainsCase { get; set; }

        public string ContainsIgnoreCase { get; set; }

        public string StartsWithCase { get; set; }

        public string StartsWithIgnoreCase { get; set; }

        public string EndsWithCase { get; set; }

        public string EndsWithIgnoreCase { get; set; }

        public string EqualsCase { get; set; }

        public string EqualsIgnoreCase { get; set; }

        public string PropertyName { get; set; }

        public string TargetStringProperty { get; set; }

        public string PropertyNameStartsWith { get; set; }        
    }
}
