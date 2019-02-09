# RWLock

Simples Reader Writer Lock über Dateien in einem Verzeichnis.
Parallel sind mehrere Reader erlaubt, aber nur ein Writer.

## mehrere Eeader gleichzeitig
```
IRWLock testlock = RWLockFactory.Create("woauchimmer');

testlock.ReadLock();
//Reader tut was
testlock.ReadLock();
//nächster Reader tut was

//beide Reader mit Arbeit fertig
testlock.ReaUnlLock();
testlock.ReaUnlLock();
```

## Writer blockiert Reader

```
IRWLock testlock = RWLockFactory.Create("woauchimmer");

try
{
    //Writer blockiert Reader (aber auch andere Writer)
    testlock.WriteLock();
    testlock.ReadLock();
}
catch (ReadLockTimeoutException)
{
    //ups, der Reader hat kein Lock erhalten
}
finally
{
    testlock.ReadLock();
    testlock.WriteLock();
}       
```
