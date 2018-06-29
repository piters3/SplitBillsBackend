using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SplitBillsBackend.Data.Interfaces;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Data.Repositories
{
    public class FriendsRepository : Repository<Friend>, IFriendsRepository
    {
        private SplitBillsDbContext _ctx => Ctx as SplitBillsDbContext;

        public FriendsRepository(SplitBillsDbContext context) : base(context)
        {
        }
    
        public IEnumerable<Friend> GetUserFriends(int id)
        {
            return _ctx.Friends
                .Include(f => f.FirstFriend)
                .Include(f => f.SecondFriend)
                .Where(f => f.FirstFriend.Id == id)
                .ToList();
        }     
    }
}
