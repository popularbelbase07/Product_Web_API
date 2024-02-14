using Product_API_Version_6.Models.Pagination;

namespace Product_API_Version_6.Models.Filteration
{
    public class ProductQueryParameters : QueryParameters
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set;}
    }
}
