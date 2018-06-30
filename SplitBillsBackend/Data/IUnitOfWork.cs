using System;
using SplitBillsBackend.Data.Interfaces;

namespace SplitBillsBackend.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IBillsRepository BillsRepository { get; }
        ICategoriesRepository CategoriesRepository { get; }
        IFriendsRepository FriendsRepository { get; }
        ISubcategoriesRepository SubcategoriesRepository { get; }
        IUsersRepository UsersRepository { get; }
        IHistoriesRepository HistoriesRepository { get; }
        int Complete();
    }
}
