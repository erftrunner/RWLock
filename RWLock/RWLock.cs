using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace RWLock
{
    class RWLock : IRWLock
    {
	    private const string readlockfilename = "lock.read";
	    private const string writelockfilename = "lock.write";

	    private readonly string readlockfile;
		private readonly string writelockfile;

		private FileStream fsRead;
		private FileStream fsWrite;

		private readonly TimeSpan pollSleepTime;
		private readonly int maxRetries;

		public RWLock (string path, int maxRetries, TimeSpan pollSleepTime)
		{
			Debug.Assert(!string.IsNullOrEmpty(path));
			Debug.Assert (File.Exists (path));
			readlockfile = path + Path.DirectorySeparatorChar + readlockfilename;
			writelockfile = path + Path.DirectorySeparatorChar + writelockfilename;
			
			Debug.Assert(maxRetries >= 0);
			this.maxRetries = maxRetries;
			
			Debug.Assert(pollSleepTime > new TimeSpan(0,0,0,0));
			this.pollSleepTime = pollSleepTime;
		}

		public void ReadLock()
		{
			//writer ncht aushungern
			int retries = maxRetries;
			while (true) 
			{
				try
				{
					//versuche Writer lock file shared zu öffnen
#pragma warning disable CS0642
					using (fsWrite = new FileStream(writelockfile, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
						;	//nix
#pragma warning restore CS0642
				}
				catch(Exception ex) 
				{
					retries--;
					if (retries <= 0)
						throw new ReadLockTimeoutException();
					
					//da ist wohl ein writer aktiv, warten ...
					Thread.Sleep(pollSleepTime);
					continue;
				}

				break;
			}

			//writer aussperren, reader erlauben
			retries = maxRetries;
			while (true) 
			{
				try
				{
					fsRead = new FileStream(readlockfile, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
				}
				catch
				{
					retries--;
					if (retries <= 0)
						throw new ReadLockTimeoutException();

					continue;
				}

				break;
			}
		}

    	public void WriteLock()
		{
			//reader aussperren, write lock file exklusiv öffnen
			int retries = maxRetries;
			while(true)
			{
				try
				{
					fsWrite = new FileStream(writelockfile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None /* exklusiv */);
				}
				catch 
				{
					retries--;
					if (retries <= 0)
						throw new WriteLockTimeoutException();

					continue;
				}

				break;
			}

			retries = maxRetries;
			while (true)
			{
				try
				{
					fsRead = new FileStream(readlockfile, FileMode.OpenOrCreate, FileAccess.ReadWrite,
						FileShare.None /* exklusiv */);
				}
				catch
				{
					retries--;
					if (retries <= 0)
						throw new WriteLockTimeoutException();

					continue;
				}

				break;
			}
		}

		public void ReadUnlock()
		{
			fsRead?.Close ();
			fsWrite?.Close();
		}

		public void WriteUnlock()
		{
			fsRead?.Close();
			fsWrite?.Close ();
		}
    }
}