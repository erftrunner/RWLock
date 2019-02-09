using System;

namespace RWLock
{
    public static class RWLockFactory
    {
        /// <summary>
        /// Erzeugen eines Lock Objektes
        /// </summary>
        /// <param name="path">Pfad in dem die read/rite lock Dateien angelgt werden</param>
        /// <param name="maxRetries">wie öft wird versucht ein read/write lock zu erhalten</param>
        /// <param name="pollSleepTime">Wartezeit zwischen Wiederholungen</param>
        /// <returns>Objekt das die Schnittstelle IRWLock erfüllt</returns>
        public static IRWLock Create(string path, int maxRetries, TimeSpan pollSleepTime)
        {
            return new RWLock(path, maxRetries, pollSleepTime);
        }

        /// <summary>
        ///  Erzeugen eines Lock Objektes mit 25 maxRetries und 200ms sleepPollTime
        /// </summary>
        /// <param name="path">Pfad in dem die read/rite lock Dateien angelgt werden</param>
        /// <returns>Objekt das die Schnittstelle IRWLock erfüllt</returns>
        public static IRWLock Create(string path)
        {
            return new RWLock(path, 3, new TimeSpan(0, 0, 0, 0, 100 /*ms*/));
        }

        public static IUseReadLock CreateUsingReadLock(string path)
        {
            return new UseReadLock(path);
        }
        
        public static IUseWriteLock CreateUsingWriteLock(string path)
        {
            return new UseWriteLock(path);
        }
    }
}