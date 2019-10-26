using AutoFilter;

namespace Tests.Models
{
    public class NotAutoFilteredFilter
    {
        [NotAutoFiltered]
        public int? NotFiltered { get; set; }

        public int? Filtered { get; set; }
    }
}
