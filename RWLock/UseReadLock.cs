using System;

namespace RWLock
{
    public class UseReadLock : IUseReadLock
    {
        private readonly IRWLock _rwlock;
        
        public UseReadLock(string path)
        {
            _rwlock = RWLockFactory.Create(path);
            _rwlock.ReadLock();
        }

        public void Dispose()
        {
            _rwlock.ReadUnlock();
        }
    }
}