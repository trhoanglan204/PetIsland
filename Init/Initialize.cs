namespace PetIsland.Init;
public class Initialize
{
    public enum Category
    {
        Others,
        Foods,
        Toys,
        Medicines,
        Accessories,
        Hygiene, //Grooming & Sanitation
    }

    public enum Sexual
    {
        Boy,
        Girl
    }

    public enum TypePet
    {
        Other,
        Dog,
        Cat,
        Bird,
        Fish,
        Rabbit,
        Horse,
        Hamster,
        Reptile, //Snake, Lizard, Tortoise
        Amphibian, //Frog, Toad, Newt, Salamander
        Insect, //Butterfly, Beetle, Ant, Bee
        Arachnid, //Spider, Scorpion
    }

    public enum State
    {
        Pending,
        Delivering,
        Delivered,
        Refunded,
        Cancelled,
    }

    public enum PaymentMethod
    {
        Cash,
        Ebanking,
    }

}
