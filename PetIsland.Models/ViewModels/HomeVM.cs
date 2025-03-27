using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetIsland.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<ProductModel> Products { get; set; }
        public IEnumerable<PetModel> Pets { get; set; }
    }
}
