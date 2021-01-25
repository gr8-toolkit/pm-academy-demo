ILSpy

```csharp
// System.Threading.Mutex
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

public sealed class Mutex : WaitHandle
{
	[NullableContext(2)]
	public Mutex(bool initiallyOwned, string? name, out bool createdNew)
	{
		CreateMutexCore(initiallyOwned, name, out createdNew);
	}

	[NullableContext(2)]
	public Mutex(bool initiallyOwned, string? name)
	{
		CreateMutexCore(initiallyOwned, name, out var _);
	}

	public Mutex(bool initiallyOwned)
	{
		CreateMutexCore(initiallyOwned, null, out var _);
	}

	public Mutex()
	{
		CreateMutexCore(initiallyOwned: false, null, out var _);
	}

	private Mutex(SafeWaitHandle handle)
	{
		base.SafeWaitHandle = handle;
	}

	public static Mutex OpenExisting(string name)
	{
		Mutex result;
		return OpenExistingWorker(name, out result) switch
		{
			OpenExistingResult.NameNotFound => throw new WaitHandleCannotBeOpenedException(), 
			OpenExistingResult.NameInvalid => throw new WaitHandleCannotBeOpenedException(SR.Format(SR.Threading_WaitHandleCannotBeOpenedException_InvalidHandle, name)), 
			OpenExistingResult.PathNotFound => throw new DirectoryNotFoundException(SR.Format(SR.IO_PathNotFound_Path, name)), 
			_ => result, 
		};
	}

	public static bool TryOpenExisting(string name, [NotNullWhen(true)] out Mutex? result)
	{
		return OpenExistingWorker(name, out result) == OpenExistingResult.Success;
	}

	private void CreateMutexCore(bool initiallyOwned, string name, out bool createdNew)
	{
		uint flags = (initiallyOwned ? 1u : 0u);
		SafeWaitHandle safeWaitHandle = Interop.Kernel32.CreateMutexEx(IntPtr.Zero, name, flags, 34603009u);
		int lastWin32Error = Marshal.GetLastWin32Error();
		if (safeWaitHandle.IsInvalid)
		{
			safeWaitHandle.SetHandleAsInvalid();
			if (lastWin32Error == 6)
			{
				throw new WaitHandleCannotBeOpenedException(SR.Format(SR.Threading_WaitHandleCannotBeOpenedException_InvalidHandle, name));
			}
			throw Win32Marshal.GetExceptionForWin32Error(lastWin32Error, name);
		}
		createdNew = lastWin32Error != 183;
		base.SafeWaitHandle = safeWaitHandle;
	}

	private static OpenExistingResult OpenExistingWorker(string name, out Mutex result)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (name.Length == 0)
		{
			throw new ArgumentException(SR.Argument_EmptyName, "name");
		}
		result = null;
		SafeWaitHandle safeWaitHandle = Interop.Kernel32.OpenMutex(34603009u, inheritHandle: false, name);
		if (safeWaitHandle.IsInvalid)
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (2 == lastWin32Error || 123 == lastWin32Error)
			{
				return OpenExistingResult.NameNotFound;
			}
			if (3 == lastWin32Error)
			{
				return OpenExistingResult.PathNotFound;
			}
			if (6 == lastWin32Error)
			{
				return OpenExistingResult.NameInvalid;
			}
			throw Win32Marshal.GetExceptionForWin32Error(lastWin32Error, name);
		}
		result = new Mutex(safeWaitHandle);
		return OpenExistingResult.Success;
	}

	public void ReleaseMutex()
	{
		if (!Interop.Kernel32.ReleaseMutex(base.SafeWaitHandle))
		{
			throw new ApplicationException(SR.Arg_SynchronizationLockException);
		}
	}
}

```