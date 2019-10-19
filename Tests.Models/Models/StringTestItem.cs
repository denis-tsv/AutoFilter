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

        public string PropertyName { get; set; }

        public string TargetStringProperty { get; set; }

        public string PropertyNameStartsWith { get; set; }        
    }
}
