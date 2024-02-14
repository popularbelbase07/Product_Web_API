using Product_API_Version_6.Models.Pagination;

namespace Product_API_Version_6.Models.Filteration
{
    public class ProductQueryParameters : QueryParameters
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set;}

        // For Searching Purpose
        public string Sku {  get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        // For Advance searching mechanism
        public string SearchTerm {  get; set; } = string.Empty; 

    }
}
