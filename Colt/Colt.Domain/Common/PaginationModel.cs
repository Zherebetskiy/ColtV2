namespace Colt.Domain.Common
{
    public class PaginationModel<T> where T: BaseEntity<int>
    {
        public int TotalCount { get; set; }
        public List<T> Collection { get; set; }
    }
}
