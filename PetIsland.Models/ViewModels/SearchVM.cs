namespace PetIsland.Models.ViewModels
{
    public class SearchViewModel
    {
        public IEnumerable<PetModel> Pets { get; set; }
        public IEnumerable<ProductModel> Products { get; set; }
        public required string SearchKey { get; set; }
    }
}
