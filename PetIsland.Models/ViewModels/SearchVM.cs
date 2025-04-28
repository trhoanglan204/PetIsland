namespace PetIsland.Models.ViewModels;

public class SearchVM
{
    public IEnumerable<PetModel>? Pets { get; set; }
    public IEnumerable<ProductModel>? Products { get; set; }
}
