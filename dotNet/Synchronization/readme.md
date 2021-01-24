## C# synchronization


Synchronization problems
- race condition
- deadlock
- busy wait
- thread starvation

Synchronization categories
- Thread blocking methods
  - Sleep
  - Join
  - Task.Wait
- Source locking
  - Exclusive
    - lock 
    - Monitor
    - Mutex 
    - SpinLock
  - Nonexclusive
    - Semaphore
    - SemaphoreSlim
    - ReaderWriterLock 
    - ReaderWriterLockSlim 
- Signaling
  - EventWaitHandle 
    - AutoResetEvent 
    - ManualResetEvent
    - ManualResetEventSlim
  - Monitor’s Wait/Pulse
  - CountdownEvent
  - Barrier 
- Lock-free
  - Interlocked 
  - volatile 