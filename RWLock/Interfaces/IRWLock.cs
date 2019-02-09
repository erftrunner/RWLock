namespace RWLock
{
    public interface IRWLock
    {
        void ReadLock();
        void WriteLock();

        void ReadUnlock();
        void WriteUnlock();
    }
}