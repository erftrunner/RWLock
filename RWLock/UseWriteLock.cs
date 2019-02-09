using System;

namespace RWLock
{
    public class UseWriteLock : IUseWriteLock
    {
        private readonly IRWLock _rwlock;

        public UseWriteLock(string path)
        {
            _rwlock = RWLockFactory.Create(path);
            _rwlock.WriteLock();
        }

        public void Dispose()
        {
            _rwlock.WriteUnlock();
        }
    }
}