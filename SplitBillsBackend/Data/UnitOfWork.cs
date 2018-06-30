using System;
using SplitBillsBackend.Data.Interfaces;
using SplitBillsBackend.Data.Repositories;

namespace SplitBillsBackend.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(SplitBillsDbContext ctx)
        {
            _ctx = ctx;
        }

        private readonly SplitBillsDbContext _ctx;
        private IBillsRepository _billsRepository;
        private ICategoriesRepository _categoriesRepository;
        private IFriendsRepository _friendsRepository;
        private ISubcategoriesRepository _subcategoriesRepository;
        private IUsersRepository _usersRepository;
        private IHistoriesRepository _historiesRepository;

        public IBillsRepository BillsRepository => _billsRepository ?? (_billsRepository = new BillsRepository(_ctx));
        public ICategoriesRepository CategoriesRepository => _categoriesRepository ?? (_categoriesRepository = new CategoriesRepository(_ctx));
        public IFriendsRepository FriendsRepository => _friendsRepository ?? (_friendsRepository = new FriendsRepository(_ctx));
        public ISubcategoriesRepository SubcategoriesRepository => _subcategoriesRepository ?? (_subcategoriesRepository = new SubcategoriesRepository(_ctx));
        public IUsersRepository UsersRepository => _usersRepository ?? (_usersRepository = new UsersRepository(_ctx));
        public IHistoriesRepository HistoriesRepository => _historiesRepository ?? (_historiesRepository = new HistoriesRepository(_ctx));

        public int Complete()
        {
            return _ctx.SaveChanges();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _ctx.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
