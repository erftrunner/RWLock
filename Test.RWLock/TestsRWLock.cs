using System;
using System.IO;
using NUnit.Framework;
using RWLock;

namespace Test.RWLock
{
    [TestFixture]
    public class TestsRWlock
    {
        [Test]
        public void TestReadReadLock_Ok()
        {
            string testpath = Path.GetRandomFileName();
            Directory.CreateDirectory(testpath);
            IRWLock testlock = RWLockFactory.Create(testpath);

            try
            {
                string e = Environment.CurrentDirectory;
                testlock.ReadLock();
                testlock.ReadLock();
            }
            catch (ReadLockTimeoutException)
            {
                Assert.Fail();
            }
            
            Directory.Delete(testpath, true);
            Assert.Pass();
        }

        [Test]
        public void TestWriteReadLock_Exception()
        {
            string testpath = Path.GetRandomFileName();
            Directory.CreateDirectory(testpath);
            IRWLock testlock = RWLockFactory.Create(testpath);

            try
            {
                string e = Environment.CurrentDirectory;
                testlock.WriteLock();
                testlock.ReadLock();
            }
            catch (ReadLockTimeoutException)
            {
                Assert.Pass();
            }
            finally
            {
                testlock.ReadUnlock();
                testlock.WriteUnlock();
            }
            
            Directory.Delete(testpath, true);
            Assert.Fail();
        }

        [Test]
        public void TestReadWriteLock_Exception()
        {
            string testpath = Path.GetRandomFileName();
            Directory.CreateDirectory(testpath);
            IRWLock testlock = RWLockFactory.Create(testpath);

            try
            {
                string e = Environment.CurrentDirectory;
                testlock.ReadLock();
                testlock.WriteLock();
            }
            catch (WriteLockTimeoutException)
            {
                Assert.Pass();
            }
            finally
            {
                Directory.Delete(testpath, true); 
            }

            Assert.Fail();
        }

        [Test]
        public void TestWriteWriteLock_Exception()
        {
            string testpath = Path.GetRandomFileName();
            Directory.CreateDirectory(testpath);
            IRWLock testlock = RWLockFactory.Create(testpath);

            try
            {
                string e = Environment.CurrentDirectory;
                testlock.WriteLock();
                testlock.WriteLock();
            }
            catch (WriteLockTimeoutException)
            {
                Assert.Pass();
            }
            
            Directory.Delete(testpath, true);
            Assert.Fail();
        }
        
        [Test]
        public void TestWriteLockUnlockReadLock_Ok()
        {
            string testpath = Path.GetRandomFileName();
            Directory.CreateDirectory(testpath);
            IRWLock testlock = RWLockFactory.Create(testpath);

            try
            {
                string e = Environment.CurrentDirectory;
                testlock.WriteLock();
                testlock.WriteUnlock();
                testlock.ReadLock();
            }
            catch (ReadLockTimeoutException)
            {
                Assert.Fail();
            }
            
            Directory.Delete(testpath, true);
            Assert.Pass();
        }

        [Test]
        public void TestUserReadLock_Ok()
        {
            string testpath = Path.GetRandomFileName();
            Directory.CreateDirectory(testpath);
            IRWLock wlock = RWLockFactory.Create(testpath);

            using (RWLockFactory.CreateUsingReadLock(testpath))
            {
                //was auch immer der Read tun will
            }

            //Writer sollte (wieder) erlaubt sein
            try
            {
                wlock.WriteLock();
               
            }
            catch (WriteLockTimeoutException)
            {
                Assert.Fail();
            }
            finally
            {
                wlock.WriteUnlock();
                Directory.Delete(testpath, true);
            }
            
            Assert.Pass();
        }
    }
}