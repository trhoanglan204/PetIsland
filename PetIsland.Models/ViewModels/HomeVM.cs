namespace PetIsland.Models.ViewModels;

public class HomeViewModel
{
    public IEnumerable<ProductModel> Products { get; set; }
    public IEnumerable<PetModel> Pets { get; set; }
}
