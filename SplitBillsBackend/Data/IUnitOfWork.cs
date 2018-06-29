using System;
using SplitBillsBackend.Data.Interfaces;

namespace SplitBillsBackend.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IBillsRepository Bills { get; }
        ICategoriesRepository Categories { get; }
        IFriendsRepository Friends { get; }
        ISubcategoriesRepository Subcategories { get; }
        IUsersRepository Users { get; }
        int Complete();
    }
}
