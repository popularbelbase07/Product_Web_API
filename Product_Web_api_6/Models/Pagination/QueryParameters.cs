namespace Product_API_Version_6.Models.Pagination
{
    public class QueryParameters
    {
        const int _maxSize = 100;
        private int _size = 50;

        // This is a default page
        public int Page { get; set; } = 1;

        public int Size { get { return _size; }
            set
            {
                _size = Math.Min(_maxSize, value);  
            }
        }

        //Sorting 

        public string SortBy { get; set; } = "Id";
        private string _sortOrder = "asc";
        public string SortOrder { get { return _sortOrder; }
                                  set { 
                                    if(value == "asc" || value == "desc")
                                    {
                                        _sortOrder = value;
                                    }
                                  }

        }
    }
}
