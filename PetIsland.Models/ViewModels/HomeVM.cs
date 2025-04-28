namespace PetIsland.Models.ViewModels;

public class HomeViewModel
{
    public IEnumerable<ProductModel>? Products { get; set; }
    public bool MoreProduct { get; set; }
    public IEnumerable<PetModel>? Pets { get; set; }
    public bool MorePet { get; set; }

}
