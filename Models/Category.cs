namespace Product_API_Version_6.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // This property display the relationship between the entities and this particulary define the category entity has many products.

        /*
         Category(1) >---------------->Product(*)
        This configures a one-to-many relationship between Category and Product, indicating that one category can have many products.
         */
        public virtual List<Product> Products { get; set; }
    }
}