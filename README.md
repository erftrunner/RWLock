# RWLock

Simples Reader Writer Lock über Dateien in einem Verzeichnis.
Parallel sind mehrere Reader erlaubt, aber nur ein Writer.

## merhere eEader gleichzeitig
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
