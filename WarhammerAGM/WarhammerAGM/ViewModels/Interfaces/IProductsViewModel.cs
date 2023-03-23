using Model;

namespace ViewModels
{
    /// <summary>Этот интерфейс кроме списка продуктов,
    /// должен ещё содержать списки связанных с ним сущностей,
    /// чтобы выбирать из них. Они задаются в базовых интерфейсах. 
    /// Для производителей и ролей у нас есть интерфейсы.
    /// Для остальных нужно добавить.</summary>
    public interface IProductsViewModel : ISourcesViewModel
    {
        string Name { get; }

        /// <summary>Команда добавления товара.</summary>
        RelayCommand<IProduct> AddProduct { get; }

        /// <summary>Команда удаления товара.</summary>
        RelayCommand<IProduct> RemoveProduct { get; }

        /// <summary>Команда редактирования товара.</summary>
        RelayCommand<IProduct> UpdateProduct { get; }

        bool CanAddAndUpdate { get; }
        bool CanRemove { get; }
    }

}
