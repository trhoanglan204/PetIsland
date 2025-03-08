namespace PetIsland.Models;

public class SearchViewModel
{
    public List<Pets>? LPets { get; set; }
    public List<Products>? LProducts { get; set; }
    public required string SearchKey { get; set; }
}
