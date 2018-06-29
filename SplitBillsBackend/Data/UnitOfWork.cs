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

        public IBillsRepository Bills => _billsRepository ?? (_billsRepository = new BillsRepository(_ctx));
        public ICategoriesRepository Categories => _categoriesRepository ?? (_categoriesRepository = new CategoriesRepository(_ctx));
        public IFriendsRepository Friends => _friendsRepository ?? (_friendsRepository = new FriendsRepository(_ctx));
        public ISubcategoriesRepository Subcategories => _subcategoriesRepository ?? (_subcategoriesRepository = new SubcategoriesRepository(_ctx));
        public IUsersRepository Users => _usersRepository ?? (_usersRepository = new UsersRepository(_ctx));

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
