A Lock is a way for us to synchronize between Threads. 
A lock is a shared object that can be **Acquired** by a Thread, and also **Released**. 
Once Acquired, other threads can be made to halt execution until the lock is Released. 
A lock is usually placed around a critical section, where you want to allow a single Thread at a time.