using Model;
using System.Collections.Generic;

namespace ViewModels
{
    public interface ISourcesViewModel
    {       
        IEnumerable<IUnit> Units { get; }

        IEnumerable<IManufacturer> Manufacturers { get; }

        IEnumerable<ISupplier> Suppliers { get; }        

        IEnumerable<ICategory> Categories { get; }

        IEnumerable<IProduct> Products { get; }
    }

}
