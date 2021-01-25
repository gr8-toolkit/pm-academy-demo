ILSpy

```csharp
// System.Threading.Tasks.Task
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Internal.Runtime.CompilerServices;

[DebuggerTypeProxy(typeof(SystemThreadingTasks_TaskDebugView))]
[DebuggerDisplay("Id = {Id}, Status = {Status}, Method = {DebuggerDisplayMethodDescription}")]
public class Task : IAsyncResult, IDisposable
{
	internal class ContingentProperties
	{
		internal ExecutionContext m_capturedContext;

		internal volatile ManualResetEventSlim m_completionEvent;

		internal volatile TaskExceptionHolder m_exceptionsHolder;

		internal CancellationToken m_cancellationToken;

		internal StrongBox<CancellationTokenRegistration> m_cancellationRegistration;

		internal volatile int m_internalCancellationRequested;

		internal volatile int m_completionCountdown = 1;

		internal volatile List<Task> m_exceptionalChildren;

		internal Task m_parent;

		internal void SetCompleted()
		{
			m_completionEvent?.Set();
		}

		internal void UnregisterCancellationCallback()
		{
			if (m_cancellationRegistration != null)
			{
				try
				{
					m_cancellationRegistration.Value.Dispose();
				}
				catch (ObjectDisposedException)
				{
				}
				m_cancellationRegistration = null;
			}
		}
	}

	private sealed class SetOnInvokeMres : ManualResetEventSlim, ITaskCompletionAction
	{
		public bool InvokeMayRunArbitraryCode => false;

		internal SetOnInvokeMres()
			: base(initialState: false, 0)
		{
		}

		public void Invoke(Task completingTask)
		{
			Set();
		}
	}

	private sealed class SetOnCountdownMres : ManualResetEventSlim, ITaskCompletionAction
	{
		private int _count;

		public bool InvokeMayRunArbitraryCode => false;

		internal SetOnCountdownMres(int count)
		{
			_count = count;
		}

		public void Invoke(Task completingTask)
		{
			if (Interlocked.Decrement(ref _count) == 0)
			{
				Set();
			}
		}
	}

	private class DelayPromise : Task
	{
		private readonly TimerQueueTimer _timer;

		internal DelayPromise(int millisecondsDelay)
		{
			if (TplEventSource.Log.IsEnabled())
			{
				TplEventSource.Log.TraceOperationBegin(base.Id, "Task.Delay", 0L);
			}
			if (s_asyncDebuggingEnabled)
			{
				AddToActiveTasks(this);
			}
			if (millisecondsDelay != -1)
			{
				_timer = new TimerQueueTimer(delegate(object state)
				{
					((DelayPromise)state).CompleteTimedOut();
				}, this, (uint)millisecondsDelay, uint.MaxValue, flowExecutionContext: false);
				if (base.IsCompleted)
				{
					_timer.Close();
				}
			}
		}

		private void CompleteTimedOut()
		{
			if (TrySetResult())
			{
				Cleanup();
				if (s_asyncDebuggingEnabled)
				{
					RemoveFromActiveTasks(this);
				}
				if (TplEventSource.Log.IsEnabled())
				{
					TplEventSource.Log.TraceOperationEnd(base.Id, AsyncCausalityStatus.Completed);
				}
			}
		}

		protected virtual void Cleanup()
		{
			_timer?.Close();
		}
	}

	private sealed class DelayPromiseWithCancellation : DelayPromise
	{
		private readonly CancellationToken _token;

		private readonly CancellationTokenRegistration _registration;

		internal DelayPromiseWithCancellation(int millisecondsDelay, CancellationToken token)
			: base(millisecondsDelay)
		{
			_token = token;
			_registration = token.UnsafeRegister(delegate(object state)
			{
				((DelayPromiseWithCancellation)state).CompleteCanceled();
			}, this);
		}

		private void CompleteCanceled()
		{
			if (TrySetCanceled(_token))
			{
				Cleanup();
			}
		}

		protected override void Cleanup()
		{
			_registration.Dispose();
			base.Cleanup();
		}
	}

	private sealed class WhenAllPromise : Task, ITaskCompletionAction
	{
		private readonly Task[] m_tasks;

		private int m_count;

		public bool InvokeMayRunArbitraryCode => true;

		internal override bool ShouldNotifyDebuggerOfWaitCompletion
		{
			get
			{
				if (base.ShouldNotifyDebuggerOfWaitCompletion)
				{
					return AnyTaskRequiresNotifyDebuggerOfWaitCompletion(m_tasks);
				}
				return false;
			}
		}

		internal WhenAllPromise(Task[] tasks)
		{
			if (TplEventSource.Log.IsEnabled())
			{
				TplEventSource.Log.TraceOperationBegin(base.Id, "Task.WhenAll", 0L);
			}
			if (s_asyncDebuggingEnabled)
			{
				AddToActiveTasks(this);
			}
			m_tasks = tasks;
			m_count = tasks.Length;
			foreach (Task task in tasks)
			{
				if (task.IsCompleted)
				{
					Invoke(task);
				}
				else
				{
					task.AddCompletionAction(this);
				}
			}
		}

		public void Invoke(Task completedTask)
		{
			if (TplEventSource.Log.IsEnabled())
			{
				TplEventSource.Log.TraceOperationRelation(base.Id, CausalityRelation.Join);
			}
			if (Interlocked.Decrement(ref m_count) != 0)
			{
				return;
			}
			List<ExceptionDispatchInfo> list = null;
			Task task = null;
			for (int i = 0; i < m_tasks.Length; i++)
			{
				Task task2 = m_tasks[i];
				if (task2.IsFaulted)
				{
					if (list == null)
					{
						list = new List<ExceptionDispatchInfo>();
					}
					list.AddRange(task2.GetExceptionDispatchInfos());
				}
				else if (task2.IsCanceled && task == null)
				{
					task = task2;
				}
				if (task2.IsWaitNotificationEnabled)
				{
					SetNotificationForWaitCompletion(enabled: true);
				}
				else
				{
					m_tasks[i] = null;
				}
			}
			if (list != null)
			{
				TrySetException(list);
				return;
			}
			if (task != null)
			{
				TrySetCanceled(task.CancellationToken, task.GetCancellationExceptionDispatchInfo());
				return;
			}
			if (TplEventSource.Log.IsEnabled())
			{
				TplEventSource.Log.TraceOperationEnd(base.Id, AsyncCausalityStatus.Completed);
			}
			if (s_asyncDebuggingEnabled)
			{
				RemoveFromActiveTasks(this);
			}
			TrySetResult();
		}
	}

	private sealed class WhenAllPromise<T> : Task<T[]>, ITaskCompletionAction
	{
		private readonly Task<T>[] m_tasks;

		private int m_count;

		public bool InvokeMayRunArbitraryCode => true;

		internal override bool ShouldNotifyDebuggerOfWaitCompletion
		{
			get
			{
				if (base.ShouldNotifyDebuggerOfWaitCompletion)
				{
					Task[] tasks = m_tasks;
					return AnyTaskRequiresNotifyDebuggerOfWaitCompletion(tasks);
				}
				return false;
			}
		}

		internal WhenAllPromise(Task<T>[] tasks)
		{
			m_tasks = tasks;
			m_count = tasks.Length;
			if (TplEventSource.Log.IsEnabled())
			{
				TplEventSource.Log.TraceOperationBegin(base.Id, "Task.WhenAll", 0L);
			}
			if (s_asyncDebuggingEnabled)
			{
				AddToActiveTasks(this);
			}
			foreach (Task<T> task in tasks)
			{
				if (task.IsCompleted)
				{
					Invoke(task);
				}
				else
				{
					task.AddCompletionAction(this);
				}
			}
		}

		public void Invoke(Task ignored)
		{
			if (TplEventSource.Log.IsEnabled())
			{
				TplEventSource.Log.TraceOperationRelation(base.Id, CausalityRelation.Join);
			}
			if (Interlocked.Decrement(ref m_count) != 0)
			{
				return;
			}
			T[] array = new T[m_tasks.Length];
			List<ExceptionDispatchInfo> list = null;
			Task task = null;
			for (int i = 0; i < m_tasks.Length; i++)
			{
				Task<T> task2 = m_tasks[i];
				if (task2.IsFaulted)
				{
					if (list == null)
					{
						list = new List<ExceptionDispatchInfo>();
					}
					list.AddRange(task2.GetExceptionDispatchInfos());
				}
				else if (task2.IsCanceled)
				{
					if (task == null)
					{
						task = task2;
					}
				}
				else
				{
					array[i] = task2.GetResultCore(waitCompletionNotification: false);
				}
				if (task2.IsWaitNotificationEnabled)
				{
					SetNotificationForWaitCompletion(enabled: true);
				}
				else
				{
					m_tasks[i] = null;
				}
			}
			if (list != null)
			{
				TrySetException(list);
				return;
			}
			if (task != null)
			{
				TrySetCanceled(task.CancellationToken, task.GetCancellationExceptionDispatchInfo());
				return;
			}
			if (TplEventSource.Log.IsEnabled())
			{
				TplEventSource.Log.TraceOperationEnd(base.Id, AsyncCausalityStatus.Completed);
			}
			if (s_asyncDebuggingEnabled)
			{
				RemoveFromActiveTasks(this);
			}
			TrySetResult(array);
		}
	}

	private sealed class TwoTaskWhenAnyPromise<TTask> : Task<TTask>, ITaskCompletionAction where TTask : Task
	{
		private TTask _task1;

		private TTask _task2;

		public bool InvokeMayRunArbitraryCode => true;

		public TwoTaskWhenAnyPromise(TTask task1, TTask task2)
		{
			_task1 = task1;
			_task2 = task2;
			if (TplEventSource.Log.IsEnabled())
			{
				TplEventSource.Log.TraceOperationBegin(base.Id, "Task.WhenAny", 0L);
			}
			if (s_asyncDebuggingEnabled)
			{
				AddToActiveTasks(this);
			}
			task1.AddCompletionAction(this);
			task2.AddCompletionAction(this);
			if (task1.IsCompleted)
			{
				task2.RemoveContinuation(this);
			}
		}

		public void Invoke(Task completingTask)
		{
			Task task;
			if ((task = Interlocked.Exchange(ref _task1, null)) != null)
			{
				Task task2 = _task2;
				_task2 = null;
				if (TplEventSource.Log.IsEnabled())
				{
					TplEventSource.Log.TraceOperationRelation(base.Id, CausalityRelation.Choice);
					TplEventSource.Log.TraceOperationEnd(base.Id, AsyncCausalityStatus.Completed);
				}
				if (s_asyncDebuggingEnabled)
				{
					RemoveFromActiveTasks(this);
				}
				if (!task.IsCompleted)
				{
					task.RemoveContinuation(this);
				}
				else
				{
					task2.RemoveContinuation(this);
				}
				bool flag = TrySetResult((TTask)completingTask);
			}
		}
	}

	[ThreadStatic]
	internal static Task t_currentTask;

	internal static int s_taskIdCounter;

	private volatile int m_taskId;

	internal Delegate m_action;

	internal object m_stateObject;

	internal TaskScheduler m_taskScheduler;

	internal volatile int m_stateFlags;

	private volatile object m_continuationObject;

	private static readonly object s_taskCompletionSentinel = new object();

	internal static bool s_asyncDebuggingEnabled;

	private static Dictionary<int, Task> s_currentActiveTasks;

	internal ContingentProperties m_contingentProperties;

	internal static readonly Task<VoidTaskResult> s_cachedCompleted = new Task<VoidTaskResult>(canceled: false, default(VoidTaskResult), (TaskCreationOptions)16384, default(CancellationToken));

	private static readonly ContextCallback s_ecCallback = delegate(object obj)
	{
		Unsafe.As<Task>(obj).InnerInvoke();
	};

	private Task? ParentForDebugger => m_contingentProperties?.m_parent;

	private int StateFlagsForDebugger => m_stateFlags;

	private string DebuggerDisplayMethodDescription => m_action?.Method.ToString() ?? "{null}";

	internal TaskCreationOptions Options => OptionsMethod(m_stateFlags);

	internal bool IsWaitNotificationEnabledOrNotRanToCompletion
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (m_stateFlags & 0x11000000) != 16777216;
		}
	}

	internal virtual bool ShouldNotifyDebuggerOfWaitCompletion => IsWaitNotificationEnabled;

	internal bool IsWaitNotificationEnabled => (m_stateFlags & 0x10000000) != 0;

	public int Id
	{
		get
		{
			if (m_taskId == 0)
			{
				int value = NewId();
				Interlocked.CompareExchange(ref m_taskId, value, 0);
			}
			return m_taskId;
		}
	}

	public static int? CurrentId => InternalCurrent?.Id;

	internal static Task? InternalCurrent => t_currentTask;

	public AggregateException? Exception
	{
		[NullableContext(2)]
		get
		{
			AggregateException result = null;
			if (IsFaulted)
			{
				result = GetExceptions(includeTaskCanceledExceptions: false);
			}
			return result;
		}
	}

	public TaskStatus Status
	{
		get
		{
			int stateFlags = m_stateFlags;
			if (((uint)stateFlags & 0x200000u) != 0)
			{
				return TaskStatus.Faulted;
			}
			if (((uint)stateFlags & 0x400000u) != 0)
			{
				return TaskStatus.Canceled;
			}
			if (((uint)stateFlags & 0x1000000u) != 0)
			{
				return TaskStatus.RanToCompletion;
			}
			if (((uint)stateFlags & 0x800000u) != 0)
			{
				return TaskStatus.WaitingForChildrenToComplete;
			}
			if (((uint)stateFlags & 0x20000u) != 0)
			{
				return TaskStatus.Running;
			}
			if (((uint)stateFlags & 0x10000u) != 0)
			{
				return TaskStatus.WaitingToRun;
			}
			if (((uint)stateFlags & 0x2000000u) != 0)
			{
				return TaskStatus.WaitingForActivation;
			}
			return TaskStatus.Created;
		}
	}

	public bool IsCanceled => (m_stateFlags & 0x600000) == 4194304;

	internal bool IsCancellationRequested
	{
		get
		{
			ContingentProperties contingentProperties = Volatile.Read(ref m_contingentProperties);
			if (contingentProperties != null)
			{
				if (contingentProperties.m_internalCancellationRequested != 1)
				{
					return contingentProperties.m_cancellationToken.IsCancellationRequested;
				}
				return true;
			}
			return false;
		}
	}

	internal CancellationToken CancellationToken => Volatile.Read(ref m_contingentProperties)?.m_cancellationToken ?? default(CancellationToken);

	internal bool IsCancellationAcknowledged => (m_stateFlags & 0x100000) != 0;

	public bool IsCompleted
	{
		get
		{
			int stateFlags = m_stateFlags;
			return IsCompletedMethod(stateFlags);
		}
	}

	public bool IsCompletedSuccessfully => (m_stateFlags & 0x1600000) == 16777216;

	public TaskCreationOptions CreationOptions => Options & (TaskCreationOptions)(-65281);

	WaitHandle IAsyncResult.AsyncWaitHandle
	{
		get
		{
			if (((uint)m_stateFlags & 0x40000u) != 0)
			{
				ThrowHelper.ThrowObjectDisposedException(ExceptionResource.Task_ThrowIfDisposed);
			}
			return CompletedEvent.WaitHandle;
		}
	}

	public object? AsyncState
	{
		[NullableContext(2)]
		get
		{
			return m_stateObject;
		}
	}

	bool IAsyncResult.CompletedSynchronously => false;

	internal TaskScheduler? ExecutingTaskScheduler => m_taskScheduler;

	public static TaskFactory Factory
	{
		get;
	} = new TaskFactory();


	public static Task CompletedTask => s_cachedCompleted;

	internal ManualResetEventSlim CompletedEvent
	{
		get
		{
			ContingentProperties contingentProperties = EnsureContingentPropertiesInitialized();
			if (contingentProperties.m_completionEvent == null)
			{
				bool isCompleted = IsCompleted;
				ManualResetEventSlim manualResetEventSlim = new ManualResetEventSlim(isCompleted);
				if (Interlocked.CompareExchange(ref contingentProperties.m_completionEvent, manualResetEventSlim, null) != null)
				{
					manualResetEventSlim.Dispose();
				}
				else if (!isCompleted && IsCompleted)
				{
					manualResetEventSlim.Set();
				}
			}
			return contingentProperties.m_completionEvent;
		}
	}

	internal bool ExceptionRecorded
	{
		get
		{
			ContingentProperties contingentProperties = Volatile.Read(ref m_contingentProperties);
			if (contingentProperties != null && contingentProperties.m_exceptionsHolder != null)
			{
				return contingentProperties.m_exceptionsHolder.ContainsFaultList;
			}
			return false;
		}
	}

	public bool IsFaulted => (m_stateFlags & 0x200000) != 0;

	internal ExecutionContext? CapturedContext
	{
		get
		{
			if ((m_stateFlags & 0x20000000) == 536870912)
			{
				return null;
			}
			return m_contingentProperties?.m_capturedContext ?? ExecutionContext.Default;
		}
		set
		{
			if (value == null)
			{
				m_stateFlags |= 536870912;
			}
			else if (value != ExecutionContext.Default)
			{
				EnsureContingentPropertiesInitializedUnsafe().m_capturedContext = value;
			}
		}
	}

	internal bool IsExceptionObservedByParent => (m_stateFlags & 0x80000) != 0;

	internal bool IsDelegateInvoked => (m_stateFlags & 0x20000) != 0;

	internal static bool AddToActiveTasks(Task task)
	{
		LazyInitializer.EnsureInitialized(ref s_currentActiveTasks, () => new Dictionary<int, Task>());
		int id = task.Id;
		lock (s_currentActiveTasks)
		{
			s_currentActiveTasks[id] = task;
		}
		return true;
	}

	internal static void RemoveFromActiveTasks(Task task)
	{
		if (s_currentActiveTasks != null)
		{
			int id = task.Id;
			lock (s_currentActiveTasks)
			{
				s_currentActiveTasks.Remove(id);
			}
		}
	}

	internal Task(bool canceled, TaskCreationOptions creationOptions, CancellationToken ct)
	{
		if (canceled)
		{
			m_stateFlags = (int)((TaskCreationOptions)5242880 | creationOptions);
			m_contingentProperties = new ContingentProperties
			{
				m_cancellationToken = ct,
				m_internalCancellationRequested = 1
			};
		}
		else
		{
			m_stateFlags = (int)((TaskCreationOptions)16777216 | creationOptions);
		}
	}

	internal Task()
	{
		m_stateFlags = 33555456;
	}

	internal Task(object state, TaskCreationOptions creationOptions, bool promiseStyle)
	{
		if (((uint)creationOptions & 0xFFFFFFBBu) != 0)
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.creationOptions);
		}
		if ((creationOptions & TaskCreationOptions.AttachedToParent) != 0)
		{
			Task internalCurrent = InternalCurrent;
			if (internalCurrent != null)
			{
				EnsureContingentPropertiesInitializedUnsafe().m_parent = internalCurrent;
			}
		}
		TaskConstructorCore(null, state, default(CancellationToken), creationOptions, InternalTaskOptions.PromiseTask, null);
	}

	public Task(Action action)
		: this(action, null, null, default(CancellationToken), TaskCreationOptions.None, InternalTaskOptions.None, null)
	{
	}

	public Task(Action action, CancellationToken cancellationToken)
		: this(action, null, null, cancellationToken, TaskCreationOptions.None, InternalTaskOptions.None, null)
	{
	}

	public Task(Action action, TaskCreationOptions creationOptions)
		: this(action, null, InternalCurrentIfAttached(creationOptions), default(CancellationToken), creationOptions, InternalTaskOptions.None, null)
	{
	}

	public Task(Action action, CancellationToken cancellationToken, TaskCreationOptions creationOptions)
		: this(action, null, InternalCurrentIfAttached(creationOptions), cancellationToken, creationOptions, InternalTaskOptions.None, null)
	{
	}

	[NullableContext(2)]
	public Task(Action<object?> action, object? state)
		: this(action, state, null, default(CancellationToken), TaskCreationOptions.None, InternalTaskOptions.None, null)
	{
	}

	[NullableContext(2)]
	public Task(Action<object?> action, object? state, CancellationToken cancellationToken)
		: this(action, state, null, cancellationToken, TaskCreationOptions.None, InternalTaskOptions.None, null)
	{
	}

	[NullableContext(2)]
	public Task(Action<object?> action, object? state, TaskCreationOptions creationOptions)
		: this(action, state, InternalCurrentIfAttached(creationOptions), default(CancellationToken), creationOptions, InternalTaskOptions.None, null)
	{
	}

	[NullableContext(2)]
	public Task(Action<object?> action, object? state, CancellationToken cancellationToken, TaskCreationOptions creationOptions)
		: this(action, state, InternalCurrentIfAttached(creationOptions), cancellationToken, creationOptions, InternalTaskOptions.None, null)
	{
	}

	internal Task(Delegate action, object state, Task parent, CancellationToken cancellationToken, TaskCreationOptions creationOptions, InternalTaskOptions internalOptions, TaskScheduler scheduler)
	{
		if ((object)action == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.action);
		}
		if (parent != null && (creationOptions & TaskCreationOptions.AttachedToParent) != 0)
		{
			EnsureContingentPropertiesInitializedUnsafe().m_parent = parent;
		}
		TaskConstructorCore(action, state, cancellationToken, creationOptions, internalOptions, scheduler);
		CapturedContext = ExecutionContext.Capture();
	}

	internal void TaskConstructorCore(Delegate action, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions, InternalTaskOptions internalOptions, TaskScheduler scheduler)
	{
		m_action = action;
		m_stateObject = state;
		m_taskScheduler = scheduler;
		if (((uint)creationOptions & 0xFFFFFFA0u) != 0)
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.creationOptions);
		}
		int num = (int)creationOptions | (int)internalOptions;
		m_stateFlags = (((object)m_action == null || (internalOptions & InternalTaskOptions.ContinuationTask) != 0) ? (num | 0x2000000) : num);
		ContingentProperties contingentProperties = m_contingentProperties;
		if (contingentProperties != null)
		{
			Task parent = contingentProperties.m_parent;
			if (parent != null && (creationOptions & TaskCreationOptions.AttachedToParent) != 0 && (parent.CreationOptions & TaskCreationOptions.DenyChildAttach) == 0)
			{
				parent.AddNewChild();
			}
		}
		if (cancellationToken.CanBeCanceled)
		{
			AssignCancellationToken(cancellationToken, null, null);
		}
	}

	private void AssignCancellationToken(CancellationToken cancellationToken, Task antecedent, TaskContinuation continuation)
	{
		ContingentProperties contingentProperties = EnsureContingentPropertiesInitializedUnsafe();
		contingentProperties.m_cancellationToken = cancellationToken;
		try
		{
			if ((Options & (TaskCreationOptions)13312) != 0)
			{
				return;
			}
			if (cancellationToken.IsCancellationRequested)
			{
				InternalCancel();
				return;
			}
			CancellationTokenRegistration value = ((antecedent != null) ? cancellationToken.UnsafeRegister(delegate(object t)
			{
				Tuple<Task, Task, TaskContinuation> tuple = (Tuple<Task, Task, TaskContinuation>)t;
				Task item = tuple.Item1;
				Task item2 = tuple.Item2;
				item2.RemoveContinuation(tuple.Item3);
				item.InternalCancel();
			}, new Tuple<Task, Task, TaskContinuation>(this, antecedent, continuation)) : cancellationToken.UnsafeRegister(delegate(object t)
			{
				((Task)t).InternalCancel();
			}, this));
			contingentProperties.m_cancellationRegistration = new StrongBox<CancellationTokenRegistration>(value);
		}
		catch
		{
			Task task = m_contingentProperties?.m_parent;
			if (task != null && (Options & TaskCreationOptions.AttachedToParent) != 0 && (task.Options & TaskCreationOptions.DenyChildAttach) == 0)
			{
				task.DisregardChild();
			}
			throw;
		}
	}

	internal static TaskCreationOptions OptionsMethod(int flags)
	{
		return (TaskCreationOptions)(flags & 0xFFFF);
	}

	internal bool AtomicStateUpdate(int newBits, int illegalBits)
	{
		int stateFlags = m_stateFlags;
		if ((stateFlags & illegalBits) == 0)
		{
			if (Interlocked.CompareExchange(ref m_stateFlags, stateFlags | newBits, stateFlags) != stateFlags)
			{
				return AtomicStateUpdateSlow(newBits, illegalBits);
			}
			return true;
		}
		return false;
	}

	private bool AtomicStateUpdateSlow(int newBits, int illegalBits)
	{
		int num = m_stateFlags;
		while (true)
		{
			if ((num & illegalBits) != 0)
			{
				return false;
			}
			int num2 = Interlocked.CompareExchange(ref m_stateFlags, num | newBits, num);
			if (num2 == num)
			{
				break;
			}
			num = num2;
		}
		return true;
	}

	internal bool AtomicStateUpdate(int newBits, int illegalBits, ref int oldFlags)
	{
		int num = (oldFlags = m_stateFlags);
		while (true)
		{
			if ((num & illegalBits) != 0)
			{
				return false;
			}
			oldFlags = Interlocked.CompareExchange(ref m_stateFlags, num | newBits, num);
			if (oldFlags == num)
			{
				break;
			}
			num = oldFlags;
		}
		return true;
	}

	internal void SetNotificationForWaitCompletion(bool enabled)
	{
		if (enabled)
		{
			bool flag = AtomicStateUpdate(268435456, 90177536);
		}
		else
		{
			Interlocked.And(ref m_stateFlags, -268435457);
		}
	}

	internal bool NotifyDebuggerOfWaitCompletionIfNecessary()
	{
		if (IsWaitNotificationEnabled && ShouldNotifyDebuggerOfWaitCompletion)
		{
			NotifyDebuggerOfWaitCompletion();
			return true;
		}
		return false;
	}

	internal static bool AnyTaskRequiresNotifyDebuggerOfWaitCompletion(Task[] tasks)
	{
		foreach (Task task in tasks)
		{
			if (task != null && task.IsWaitNotificationEnabled && task.ShouldNotifyDebuggerOfWaitCompletion)
			{
				return true;
			}
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
	private void NotifyDebuggerOfWaitCompletion()
	{
		SetNotificationForWaitCompletion(enabled: false);
	}

	internal bool MarkStarted()
	{
		return AtomicStateUpdate(65536, 4259840);
	}

	internal void FireTaskScheduledIfNeeded(TaskScheduler ts)
	{
		if ((m_stateFlags & 0x40000000) == 0)
		{
			m_stateFlags |= 1073741824;
			Task internalCurrent = InternalCurrent;
			Task task = m_contingentProperties?.m_parent;
			TplEventSource.Log.TaskScheduled(ts.Id, internalCurrent?.Id ?? 0, Id, task?.Id ?? 0, (int)Options);
		}
	}

	internal void AddNewChild()
	{
		ContingentProperties contingentProperties = EnsureContingentPropertiesInitialized();
		if (contingentProperties.m_completionCountdown == 1)
		{
			contingentProperties.m_completionCountdown++;
		}
		else
		{
			Interlocked.Increment(ref contingentProperties.m_completionCountdown);
		}
	}

	internal void DisregardChild()
	{
		ContingentProperties contingentProperties = EnsureContingentPropertiesInitialized();
		Interlocked.Decrement(ref contingentProperties.m_completionCountdown);
	}

	public void Start()
	{
		Start(TaskScheduler.Current);
	}

	public void Start(TaskScheduler scheduler)
	{
		int stateFlags = m_stateFlags;
		if (IsCompletedMethod(stateFlags))
		{
			ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_Start_TaskCompleted);
		}
		if (scheduler == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
		}
		TaskCreationOptions taskCreationOptions = OptionsMethod(stateFlags);
		if ((taskCreationOptions & (TaskCreationOptions)1024) != 0)
		{
			ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_Start_Promise);
		}
		if ((taskCreationOptions & (TaskCreationOptions)512) != 0)
		{
			ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_Start_ContinuationTask);
		}
		if (Interlocked.CompareExchange(ref m_taskScheduler, scheduler, null) != null)
		{
			ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_Start_AlreadyStarted);
		}
		ScheduleAndStart(needsProtection: true);
	}

	public void RunSynchronously()
	{
		InternalRunSynchronously(TaskScheduler.Current, waitForCompletion: true);
	}

	public void RunSynchronously(TaskScheduler scheduler)
	{
		if (scheduler == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
		}
		InternalRunSynchronously(scheduler, waitForCompletion: true);
	}

	internal void InternalRunSynchronously(TaskScheduler scheduler, bool waitForCompletion)
	{
		int stateFlags = m_stateFlags;
		TaskCreationOptions taskCreationOptions = OptionsMethod(stateFlags);
		if ((taskCreationOptions & (TaskCreationOptions)512) != 0)
		{
			ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_RunSynchronously_Continuation);
		}
		if ((taskCreationOptions & (TaskCreationOptions)1024) != 0)
		{
			ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_RunSynchronously_Promise);
		}
		if (IsCompletedMethod(stateFlags))
		{
			ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_RunSynchronously_TaskCompleted);
		}
		if (Interlocked.CompareExchange(ref m_taskScheduler, scheduler, null) != null)
		{
			ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_RunSynchronously_AlreadyStarted);
		}
		if (MarkStarted())
		{
			bool flag = false;
			try
			{
				if (!scheduler.TryRunInline(this, taskWasPreviouslyQueued: false))
				{
					scheduler.InternalQueueTask(this);
					flag = true;
				}
				if (waitForCompletion && !IsCompleted)
				{
					SpinThenBlockingWait(-1, default(CancellationToken));
				}
			}
			catch (Exception innerException)
			{
				if (!flag)
				{
					TaskSchedulerException ex = new TaskSchedulerException(innerException);
					AddException(ex);
					Finish(userDelegateExecute: false);
					m_contingentProperties.m_exceptionsHolder.MarkAsHandled(calledFromFinalizer: false);
					throw ex;
				}
				throw;
			}
		}
		else
		{
			ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_RunSynchronously_TaskCompleted);
		}
	}

	internal static Task InternalStartNew(Task creatingTask, Delegate action, object state, CancellationToken cancellationToken, TaskScheduler scheduler, TaskCreationOptions options, InternalTaskOptions internalOptions)
	{
		if (scheduler == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
		}
		Task task = new Task(action, state, creatingTask, cancellationToken, options, internalOptions | InternalTaskOptions.QueuedByRuntime, scheduler);
		task.ScheduleAndStart(needsProtection: false);
		return task;
	}

	internal static int NewId()
	{
		int num;
		do
		{
			num = Interlocked.Increment(ref s_taskIdCounter);
		}
		while (num == 0);
		if (TplEventSource.Log.IsEnabled())
		{
			TplEventSource.Log.NewID(num);
		}
		return num;
	}

	internal static Task InternalCurrentIfAttached(TaskCreationOptions creationOptions)
	{
		if ((creationOptions & TaskCreationOptions.AttachedToParent) == 0)
		{
			return null;
		}
		return InternalCurrent;
	}

	internal ContingentProperties EnsureContingentPropertiesInitialized()
	{
		return LazyInitializer.EnsureInitialized(ref m_contingentProperties, () => new ContingentProperties());
	}

	internal ContingentProperties EnsureContingentPropertiesInitializedUnsafe()
	{
		return m_contingentProperties ?? (m_contingentProperties = new ContingentProperties());
	}

	private static bool IsCompletedMethod(int flags)
	{
		return (flags & 0x1600000) != 0;
	}

	internal void SpinUntilCompleted()
	{
		SpinWait spinWait = default(SpinWait);
		while (!IsCompleted)
		{
			spinWait.SpinOnce();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			if ((Options & (TaskCreationOptions)16384) != 0)
			{
				return;
			}
			if (!IsCompleted)
			{
				ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_Dispose_NotCompleted);
			}
			ContingentProperties contingentProperties = Volatile.Read(ref m_contingentProperties);
			if (contingentProperties != null)
			{
				ManualResetEventSlim completionEvent = contingentProperties.m_completionEvent;
				if (completionEvent != null)
				{
					contingentProperties.m_completionEvent = null;
					if (!completionEvent.IsSet)
					{
						completionEvent.Set();
					}
					completionEvent.Dispose();
				}
			}
		}
		m_stateFlags |= 262144;
	}

	internal void ScheduleAndStart(bool needsProtection)
	{
		if (needsProtection)
		{
			if (!MarkStarted())
			{
				return;
			}
		}
		else
		{
			m_stateFlags |= 65536;
		}
		if (s_asyncDebuggingEnabled)
		{
			AddToActiveTasks(this);
		}
		if (TplEventSource.Log.IsEnabled() && (Options & (TaskCreationOptions)512) == 0)
		{
			TplEventSource.Log.TraceOperationBegin(Id, "Task: " + m_action.Method.Name, 0L);
		}
		try
		{
			m_taskScheduler.InternalQueueTask(this);
		}
		catch (Exception innerException)
		{
			TaskSchedulerException ex = new TaskSchedulerException(innerException);
			AddException(ex);
			Finish(userDelegateExecute: false);
			if ((Options & (TaskCreationOptions)512) == 0)
			{
				m_contingentProperties.m_exceptionsHolder.MarkAsHandled(calledFromFinalizer: false);
			}
			throw ex;
		}
	}

	internal void AddException(object exceptionObject)
	{
		AddException(exceptionObject, representsCancellation: false);
	}

	internal void AddException(object exceptionObject, bool representsCancellation)
	{
		ContingentProperties contingentProperties = EnsureContingentPropertiesInitialized();
		if (contingentProperties.m_exceptionsHolder == null)
		{
			TaskExceptionHolder taskExceptionHolder = new TaskExceptionHolder(this);
			if (Interlocked.CompareExchange(ref contingentProperties.m_exceptionsHolder, taskExceptionHolder, null) != null)
			{
				taskExceptionHolder.MarkAsHandled(calledFromFinalizer: false);
			}
		}
		lock (contingentProperties)
		{
			contingentProperties.m_exceptionsHolder.Add(exceptionObject, representsCancellation);
		}
	}

	private AggregateException GetExceptions(bool includeTaskCanceledExceptions)
	{
		Exception ex = null;
		if (includeTaskCanceledExceptions && IsCanceled)
		{
			ex = new TaskCanceledException(this);
			ex.SetCurrentStackTrace();
		}
		if (ExceptionRecorded)
		{
			return m_contingentProperties.m_exceptionsHolder.CreateExceptionObject(calledFromFinalizer: false, ex);
		}
		if (ex != null)
		{
			return new AggregateException(ex);
		}
		return null;
	}

	internal ReadOnlyCollection<ExceptionDispatchInfo> GetExceptionDispatchInfos()
	{
		return m_contingentProperties.m_exceptionsHolder.GetExceptionDispatchInfos();
	}

	internal ExceptionDispatchInfo GetCancellationExceptionDispatchInfo()
	{
		ContingentProperties contingentProperties = Volatile.Read(ref m_contingentProperties);
		if (contingentProperties == null)
		{
			return null;
		}
		return contingentProperties.m_exceptionsHolder?.GetCancellationExceptionDispatchInfo();
	}

	internal void ThrowIfExceptional(bool includeTaskCanceledExceptions)
	{
		Exception exceptions = GetExceptions(includeTaskCanceledExceptions);
		if (exceptions != null)
		{
			UpdateExceptionObservedStatus();
			throw exceptions;
		}
	}

	internal static void ThrowAsync(Exception exception, SynchronizationContext targetContext)
	{
		ExceptionDispatchInfo state2 = ExceptionDispatchInfo.Capture(exception);
		if (targetContext != null)
		{
			try
			{
				targetContext.Post(delegate(object state)
				{
					((ExceptionDispatchInfo)state).Throw();
				}, state2);
				return;
			}
			catch (Exception ex)
			{
				state2 = ExceptionDispatchInfo.Capture(new AggregateException(exception, ex));
			}
		}
		ThreadPool.QueueUserWorkItem(delegate(object state)
		{
			((ExceptionDispatchInfo)state).Throw();
		}, state2);
	}

	internal void UpdateExceptionObservedStatus()
	{
		Task task = m_contingentProperties?.m_parent;
		if (task != null && (Options & TaskCreationOptions.AttachedToParent) != 0 && (task.CreationOptions & TaskCreationOptions.DenyChildAttach) == 0 && InternalCurrent == task)
		{
			m_stateFlags |= 524288;
		}
	}

	internal void Finish(bool userDelegateExecute)
	{
		if (m_contingentProperties == null)
		{
			FinishStageTwo();
		}
		else
		{
			FinishSlow(userDelegateExecute);
		}
	}

	private void FinishSlow(bool userDelegateExecute)
	{
		if (!userDelegateExecute)
		{
			FinishStageTwo();
			return;
		}
		ContingentProperties contingentProperties = m_contingentProperties;
		if (contingentProperties.m_completionCountdown == 1 || Interlocked.Decrement(ref contingentProperties.m_completionCountdown) == 0)
		{
			FinishStageTwo();
		}
		else
		{
			AtomicStateUpdate(8388608, 23068672);
		}
		List<Task> exceptionalChildren = contingentProperties.m_exceptionalChildren;
		if (exceptionalChildren == null)
		{
			return;
		}
		lock (exceptionalChildren)
		{
			exceptionalChildren.RemoveAll((Task t) => t.IsExceptionObservedByParent);
		}
	}

	private void FinishStageTwo()
	{
		ContingentProperties contingentProperties = Volatile.Read(ref m_contingentProperties);
		if (contingentProperties != null)
		{
			AddExceptionsFromChildren(contingentProperties);
		}
		int num;
		if (ExceptionRecorded)
		{
			num = 2097152;
			if (TplEventSource.Log.IsEnabled())
			{
				TplEventSource.Log.TraceOperationEnd(Id, AsyncCausalityStatus.Error);
			}
			if (s_asyncDebuggingEnabled)
			{
				RemoveFromActiveTasks(this);
			}
		}
		else if (IsCancellationRequested && IsCancellationAcknowledged)
		{
			num = 4194304;
			if (TplEventSource.Log.IsEnabled())
			{
				TplEventSource.Log.TraceOperationEnd(Id, AsyncCausalityStatus.Canceled);
			}
			if (s_asyncDebuggingEnabled)
			{
				RemoveFromActiveTasks(this);
			}
		}
		else
		{
			num = 16777216;
			if (TplEventSource.Log.IsEnabled())
			{
				TplEventSource.Log.TraceOperationEnd(Id, AsyncCausalityStatus.Completed);
			}
			if (s_asyncDebuggingEnabled)
			{
				RemoveFromActiveTasks(this);
			}
		}
		Interlocked.Exchange(ref m_stateFlags, m_stateFlags | num);
		contingentProperties = Volatile.Read(ref m_contingentProperties);
		if (contingentProperties != null)
		{
			contingentProperties.SetCompleted();
			contingentProperties.UnregisterCancellationCallback();
		}
		FinishStageThree();
	}

	internal void FinishStageThree()
	{
		m_action = null;
		ContingentProperties contingentProperties = m_contingentProperties;
		if (contingentProperties != null)
		{
			contingentProperties.m_capturedContext = null;
			NotifyParentIfPotentiallyAttachedTask();
		}
		FinishContinuations();
	}

	internal void NotifyParentIfPotentiallyAttachedTask()
	{
		Task task = m_contingentProperties?.m_parent;
		if (task != null && (task.CreationOptions & TaskCreationOptions.DenyChildAttach) == 0 && ((uint)m_stateFlags & 0xFFFFu & 4u) != 0)
		{
			task.ProcessChildCompletion(this);
		}
	}

	internal void ProcessChildCompletion(Task childTask)
	{
		ContingentProperties contingentProperties = Volatile.Read(ref m_contingentProperties);
		if (childTask.IsFaulted && !childTask.IsExceptionObservedByParent)
		{
			if (contingentProperties.m_exceptionalChildren == null)
			{
				Interlocked.CompareExchange(ref contingentProperties.m_exceptionalChildren, new List<Task>(), null);
			}
			List<Task> exceptionalChildren = contingentProperties.m_exceptionalChildren;
			if (exceptionalChildren != null)
			{
				lock (exceptionalChildren)
				{
					exceptionalChildren.Add(childTask);
				}
			}
		}
		if (Interlocked.Decrement(ref contingentProperties.m_completionCountdown) == 0)
		{
			FinishStageTwo();
		}
	}

	internal void AddExceptionsFromChildren(ContingentProperties props)
	{
		List<Task> exceptionalChildren = props.m_exceptionalChildren;
		if (exceptionalChildren == null)
		{
			return;
		}
		lock (exceptionalChildren)
		{
			foreach (Task item in exceptionalChildren)
			{
				if (item.IsFaulted && !item.IsExceptionObservedByParent)
				{
					TaskExceptionHolder exceptionsHolder = Volatile.Read(ref item.m_contingentProperties).m_exceptionsHolder;
					AddException(exceptionsHolder.CreateExceptionObject(calledFromFinalizer: false, null));
				}
			}
		}
		props.m_exceptionalChildren = null;
	}

	internal bool ExecuteEntry()
	{
		int oldFlags = 0;
		if (!AtomicStateUpdate(131072, 23199744, ref oldFlags) && (oldFlags & 0x400000) == 0)
		{
			return false;
		}
		if (!IsCancellationRequested & !IsCanceled)
		{
			ExecuteWithThreadLocal(ref t_currentTask);
		}
		else
		{
			ExecuteEntryCancellationRequestedOrCanceled();
		}
		return true;
	}

	internal virtual void ExecuteFromThreadPool(Thread threadPoolThread)
	{
		ExecuteEntryUnsafe(threadPoolThread);
	}

	internal void ExecuteEntryUnsafe(Thread threadPoolThread)
	{
		m_stateFlags |= 131072;
		if (!IsCancellationRequested & !IsCanceled)
		{
			ExecuteWithThreadLocal(ref t_currentTask, threadPoolThread);
		}
		else
		{
			ExecuteEntryCancellationRequestedOrCanceled();
		}
	}

	internal void ExecuteEntryCancellationRequestedOrCanceled()
	{
		if (!IsCanceled)
		{
			int num = Interlocked.Exchange(ref m_stateFlags, m_stateFlags | 0x400000);
			if ((num & 0x400000) == 0)
			{
				CancellationCleanupLogic();
			}
		}
	}

	private void ExecuteWithThreadLocal(ref Task currentTaskSlot, Thread threadPoolThread = null)
	{
		Task task = currentTaskSlot;
		TplEventSource log = TplEventSource.Log;
		Guid oldActivityThatWillContinue = default(Guid);
		bool flag = log.IsEnabled();
		if (flag)
		{
			if (log.TasksSetActivityIds)
			{
				EventSource.SetCurrentThreadActivityId(TplEventSource.CreateGuidForTaskID(Id), out oldActivityThatWillContinue);
			}
			if (task != null)
			{
				log.TaskStarted(task.m_taskScheduler.Id, task.Id, Id);
			}
			else
			{
				log.TaskStarted(TaskScheduler.Current.Id, 0, Id);
			}
		}
		bool flag2 = TplEventSource.Log.IsEnabled();
		if (flag2)
		{
			TplEventSource.Log.TraceSynchronousWorkBegin(Id, CausalitySynchronousWork.Execution);
		}
		try
		{
			currentTaskSlot = this;
			try
			{
				ExecutionContext capturedContext = CapturedContext;
				if (capturedContext == null)
				{
					InnerInvoke();
				}
				else if (threadPoolThread == null)
				{
					ExecutionContext.RunInternal(capturedContext, s_ecCallback, this);
				}
				else
				{
					ExecutionContext.RunFromThreadPoolDispatchLoop(threadPoolThread, capturedContext, s_ecCallback, this);
				}
			}
			catch (Exception unhandledException)
			{
				HandleException(unhandledException);
			}
			if (flag2)
			{
				TplEventSource.Log.TraceSynchronousWorkEnd(CausalitySynchronousWork.Execution);
			}
			Finish(userDelegateExecute: true);
		}
		finally
		{
			currentTaskSlot = task;
			if (flag)
			{
				if (task != null)
				{
					log.TaskCompleted(task.m_taskScheduler.Id, task.Id, Id, IsFaulted);
				}
				else
				{
					log.TaskCompleted(TaskScheduler.Current.Id, 0, Id, IsFaulted);
				}
				if (log.TasksSetActivityIds)
				{
					EventSource.SetCurrentThreadActivityId(oldActivityThatWillContinue);
				}
			}
		}
	}

	internal virtual void InnerInvoke()
	{
		Action action = m_action as Action;
		if (action != null)
		{
			action();
		}
		else
		{
			(m_action as Action<object>)?.Invoke(m_stateObject);
		}
	}

	private void HandleException(Exception unhandledException)
	{
		OperationCanceledException ex = unhandledException as OperationCanceledException;
		if (ex != null && IsCancellationRequested && m_contingentProperties.m_cancellationToken == ex.CancellationToken)
		{
			SetCancellationAcknowledged();
			AddException(ex, representsCancellation: true);
		}
		else
		{
			AddException(unhandledException);
		}
	}

	public TaskAwaiter GetAwaiter()
	{
		return new TaskAwaiter(this);
	}

	public ConfiguredTaskAwaitable ConfigureAwait(bool continueOnCapturedContext)
	{
		return new ConfiguredTaskAwaitable(this, continueOnCapturedContext);
	}

	internal void SetContinuationForAwait(Action continuationAction, bool continueOnCapturedContext, bool flowExecutionContext)
	{
		TaskContinuation taskContinuation = null;
		if (continueOnCapturedContext)
		{
			SynchronizationContext current = SynchronizationContext.Current;
			if (current != null && current.GetType() != typeof(SynchronizationContext))
			{
				taskContinuation = new SynchronizationContextAwaitTaskContinuation(current, continuationAction, flowExecutionContext);
			}
			else
			{
				TaskScheduler internalCurrent = TaskScheduler.InternalCurrent;
				if (internalCurrent != null && internalCurrent != TaskScheduler.Default)
				{
					taskContinuation = new TaskSchedulerAwaitTaskContinuation(internalCurrent, continuationAction, flowExecutionContext);
				}
			}
		}
		if (taskContinuation == null && flowExecutionContext)
		{
			taskContinuation = new AwaitTaskContinuation(continuationAction, flowExecutionContext: true);
		}
		if (taskContinuation != null)
		{
			if (!AddTaskContinuation(taskContinuation, addBeforeOthers: false))
			{
				taskContinuation.Run(this, canInlineContinuationTask: false);
			}
		}
		else if (!AddTaskContinuation(continuationAction, addBeforeOthers: false))
		{
			AwaitTaskContinuation.UnsafeScheduleAction(continuationAction, this);
		}
	}

	internal void UnsafeSetContinuationForAwait(IAsyncStateMachineBox stateMachineBox, bool continueOnCapturedContext)
	{
		if (continueOnCapturedContext)
		{
			SynchronizationContext current = SynchronizationContext.Current;
			if (current != null && current.GetType() != typeof(SynchronizationContext))
			{
				SynchronizationContextAwaitTaskContinuation synchronizationContextAwaitTaskContinuation = new SynchronizationContextAwaitTaskContinuation(current, stateMachineBox.MoveNextAction, flowExecutionContext: false);
				if (!AddTaskContinuation(synchronizationContextAwaitTaskContinuation, addBeforeOthers: false))
				{
					synchronizationContextAwaitTaskContinuation.Run(this, canInlineContinuationTask: false);
				}
				return;
			}
			TaskScheduler internalCurrent = TaskScheduler.InternalCurrent;
			if (internalCurrent != null && internalCurrent != TaskScheduler.Default)
			{
				TaskSchedulerAwaitTaskContinuation taskSchedulerAwaitTaskContinuation = new TaskSchedulerAwaitTaskContinuation(internalCurrent, stateMachineBox.MoveNextAction, flowExecutionContext: false);
				if (!AddTaskContinuation(taskSchedulerAwaitTaskContinuation, addBeforeOthers: false))
				{
					taskSchedulerAwaitTaskContinuation.Run(this, canInlineContinuationTask: false);
				}
				return;
			}
		}
		if (!AddTaskContinuation(stateMachineBox, addBeforeOthers: false))
		{
			ThreadPool.UnsafeQueueUserWorkItemInternal(stateMachineBox, preferLocal: true);
		}
	}

	public static YieldAwaitable Yield()
	{
		return default(YieldAwaitable);
	}

	public void Wait()
	{
		Wait(-1, default(CancellationToken));
	}

	public bool Wait(TimeSpan timeout)
	{
		long num = (long)timeout.TotalMilliseconds;
		if (num < -1 || num > int.MaxValue)
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.timeout);
		}
		return Wait((int)num, default(CancellationToken));
	}

	public void Wait(CancellationToken cancellationToken)
	{
		Wait(-1, cancellationToken);
	}

	public bool Wait(int millisecondsTimeout)
	{
		return Wait(millisecondsTimeout, default(CancellationToken));
	}

	public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
	{
		if (millisecondsTimeout < -1)
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.millisecondsTimeout);
		}
		if (!IsWaitNotificationEnabledOrNotRanToCompletion)
		{
			return true;
		}
		if (!InternalWait(millisecondsTimeout, cancellationToken))
		{
			return false;
		}
		if (IsWaitNotificationEnabledOrNotRanToCompletion)
		{
			NotifyDebuggerOfWaitCompletionIfNecessary();
			if (IsCanceled)
			{
				cancellationToken.ThrowIfCancellationRequested();
			}
			ThrowIfExceptional(includeTaskCanceledExceptions: true);
		}
		return true;
	}

	private bool WrappedTryRunInline()
	{
		if (m_taskScheduler == null)
		{
			return false;
		}
		try
		{
			return m_taskScheduler.TryRunInline(this, taskWasPreviouslyQueued: true);
		}
		catch (Exception innerException)
		{
			throw new TaskSchedulerException(innerException);
		}
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	internal bool InternalWait(int millisecondsTimeout, CancellationToken cancellationToken)
	{
		return InternalWaitCore(millisecondsTimeout, cancellationToken);
	}

	private bool InternalWaitCore(int millisecondsTimeout, CancellationToken cancellationToken)
	{
		if (IsCompleted)
		{
			return true;
		}
		TplEventSource log = TplEventSource.Log;
		bool flag = log.IsEnabled();
		if (flag)
		{
			Task internalCurrent = InternalCurrent;
			log.TaskWaitBegin(internalCurrent?.m_taskScheduler.Id ?? TaskScheduler.Default.Id, internalCurrent?.Id ?? 0, Id, TplEventSource.TaskWaitBehavior.Synchronous, 0);
		}
		Debugger.NotifyOfCrossThreadDependency();
		bool result = (millisecondsTimeout == -1 && !cancellationToken.CanBeCanceled && WrappedTryRunInline() && IsCompleted) || SpinThenBlockingWait(millisecondsTimeout, cancellationToken);
		if (flag)
		{
			Task internalCurrent2 = InternalCurrent;
			if (internalCurrent2 != null)
			{
				log.TaskWaitEnd(internalCurrent2.m_taskScheduler.Id, internalCurrent2.Id, Id);
			}
			else
			{
				log.TaskWaitEnd(TaskScheduler.Default.Id, 0, Id);
			}
			log.TaskWaitContinuationComplete(Id);
		}
		return result;
	}

	private bool SpinThenBlockingWait(int millisecondsTimeout, CancellationToken cancellationToken)
	{
		bool flag = millisecondsTimeout == -1;
		uint num = (uint)((!flag) ? Environment.TickCount : 0);
		bool flag2 = SpinWait(millisecondsTimeout);
		if (!flag2)
		{
			SetOnInvokeMres setOnInvokeMres = new SetOnInvokeMres();
			try
			{
				AddCompletionAction(setOnInvokeMres, addBeforeOthers: true);
				if (flag)
				{
					return setOnInvokeMres.Wait(-1, cancellationToken);
				}
				uint num2 = (uint)Environment.TickCount - num;
				if (num2 < millisecondsTimeout)
				{
					return setOnInvokeMres.Wait((int)(millisecondsTimeout - num2), cancellationToken);
				}
				return flag2;
			}
			finally
			{
				if (!IsCompleted)
				{
					RemoveContinuation(setOnInvokeMres);
				}
			}
		}
		return flag2;
	}

	private bool SpinWait(int millisecondsTimeout)
	{
		if (IsCompleted)
		{
			return true;
		}
		if (millisecondsTimeout == 0)
		{
			return false;
		}
		int spinCountforSpinBeforeWait = System.Threading.SpinWait.SpinCountforSpinBeforeWait;
		SpinWait spinWait = default(SpinWait);
		while (spinWait.Count < spinCountforSpinBeforeWait)
		{
			spinWait.SpinOnce(-1);
			if (IsCompleted)
			{
				return true;
			}
		}
		return false;
	}

	internal void InternalCancel()
	{
		TaskSchedulerException ex = null;
		bool flag = false;
		if (((uint)m_stateFlags & 0x10000u) != 0)
		{
			TaskScheduler taskScheduler = m_taskScheduler;
			try
			{
				flag = taskScheduler?.TryDequeue(this) ?? false;
			}
			catch (Exception innerException)
			{
				ex = new TaskSchedulerException(innerException);
			}
		}
		RecordInternalCancellationRequest();
		bool flag2 = false;
		if (flag)
		{
			flag2 = AtomicStateUpdate(4194304, 4325376);
		}
		else if ((m_stateFlags & 0x10000) == 0)
		{
			flag2 = AtomicStateUpdate(4194304, 23265280);
		}
		if (flag2)
		{
			CancellationCleanupLogic();
		}
		if (ex != null)
		{
			throw ex;
		}
	}

	internal void InternalCancelContinueWithInitialState()
	{
		m_stateFlags |= 4194304;
		CancellationCleanupLogic();
	}

	internal void RecordInternalCancellationRequest()
	{
		EnsureContingentPropertiesInitialized().m_internalCancellationRequested = 1;
	}

	internal void RecordInternalCancellationRequest(CancellationToken tokenToRecord, object cancellationException)
	{
		RecordInternalCancellationRequest();
		if (tokenToRecord != default(CancellationToken))
		{
			m_contingentProperties.m_cancellationToken = tokenToRecord;
		}
		if (cancellationException != null)
		{
			AddException(cancellationException, representsCancellation: true);
		}
	}

	internal void CancellationCleanupLogic()
	{
		Interlocked.Exchange(ref m_stateFlags, m_stateFlags | 0x400000);
		ContingentProperties contingentProperties = Volatile.Read(ref m_contingentProperties);
		if (contingentProperties != null)
		{
			contingentProperties.SetCompleted();
			contingentProperties.UnregisterCancellationCallback();
		}
		if (TplEventSource.Log.IsEnabled())
		{
			TplEventSource.Log.TraceOperationEnd(Id, AsyncCausalityStatus.Canceled);
		}
		if (s_asyncDebuggingEnabled)
		{
			RemoveFromActiveTasks(this);
		}
		FinishStageThree();
	}

	private void SetCancellationAcknowledged()
	{
		m_stateFlags |= 1048576;
	}

	internal bool TrySetResult()
	{
		if (AtomicStateUpdate(83886080, 90177536))
		{
			ContingentProperties contingentProperties = m_contingentProperties;
			if (contingentProperties != null)
			{
				NotifyParentIfPotentiallyAttachedTask();
				contingentProperties.SetCompleted();
			}
			FinishContinuations();
			return true;
		}
		return false;
	}

	internal bool TrySetException(object exceptionObject)
	{
		bool result = false;
		EnsureContingentPropertiesInitialized();
		if (AtomicStateUpdate(67108864, 90177536))
		{
			AddException(exceptionObject);
			Finish(userDelegateExecute: false);
			result = true;
		}
		return result;
	}

	internal bool TrySetCanceled(CancellationToken tokenToRecord)
	{
		return TrySetCanceled(tokenToRecord, null);
	}

	internal bool TrySetCanceled(CancellationToken tokenToRecord, object cancellationException)
	{
		bool result = false;
		if (AtomicStateUpdate(67108864, 90177536))
		{
			RecordInternalCancellationRequest(tokenToRecord, cancellationException);
			CancellationCleanupLogic();
			result = true;
		}
		return result;
	}

	internal void FinishContinuations()
	{
		object obj = Interlocked.Exchange(ref m_continuationObject, s_taskCompletionSentinel);
		if (obj != null)
		{
			RunContinuations(obj);
		}
	}

	private void RunContinuations(object continuationObject)
	{
		TplEventSource tplEventSource = TplEventSource.Log;
		if (!tplEventSource.IsEnabled())
		{
			tplEventSource = null;
		}
		if (TplEventSource.Log.IsEnabled())
		{
			TplEventSource.Log.TraceSynchronousWorkBegin(Id, CausalitySynchronousWork.CompletionNotification);
		}
		bool flag = (m_stateFlags & 0x40) == 0 && RuntimeHelpers.TryEnsureSufficientExecutionStack();
		IAsyncStateMachineBox asyncStateMachineBox = continuationObject as IAsyncStateMachineBox;
		if (asyncStateMachineBox == null)
		{
			Action action = continuationObject as Action;
			if (action == null)
			{
				TaskContinuation taskContinuation = continuationObject as TaskContinuation;
				if (taskContinuation == null)
				{
					ITaskCompletionAction taskCompletionAction = continuationObject as ITaskCompletionAction;
					if (taskCompletionAction != null)
					{
						RunOrQueueCompletionAction(taskCompletionAction, flag);
						LogFinishCompletionNotification();
						return;
					}
					List<object> list = (List<object>)continuationObject;
					lock (list)
					{
					}
					int count = list.Count;
					if (flag)
					{
						bool flag2 = false;
						for (int i = 0; i < count; i++)
						{
							object obj = list[i];
							if (obj == null)
							{
								continue;
							}
							ContinueWithTaskContinuation continueWithTaskContinuation = obj as ContinueWithTaskContinuation;
							if (continueWithTaskContinuation != null)
							{
								if ((continueWithTaskContinuation.m_options & TaskContinuationOptions.ExecuteSynchronously) == 0)
								{
									list[i] = null;
									tplEventSource?.RunningContinuationList(Id, i, continueWithTaskContinuation);
									continueWithTaskContinuation.Run(this, canInlineContinuationTask: false);
								}
							}
							else
							{
								if (obj is ITaskCompletionAction)
								{
									continue;
								}
								if (flag2)
								{
									list[i] = null;
									tplEventSource?.RunningContinuationList(Id, i, obj);
									IAsyncStateMachineBox asyncStateMachineBox2 = obj as IAsyncStateMachineBox;
									if (asyncStateMachineBox2 == null)
									{
										Action action2 = obj as Action;
										if (action2 != null)
										{
											AwaitTaskContinuation.RunOrScheduleAction(action2, allowInlining: false);
										}
										else
										{
											((TaskContinuation)obj).Run(this, canInlineContinuationTask: false);
										}
									}
									else
									{
										AwaitTaskContinuation.RunOrScheduleAction(asyncStateMachineBox2, allowInlining: false);
									}
								}
								flag2 = true;
							}
						}
					}
					for (int j = 0; j < count; j++)
					{
						object obj2 = list[j];
						if (obj2 == null)
						{
							continue;
						}
						list[j] = null;
						tplEventSource?.RunningContinuationList(Id, j, obj2);
						IAsyncStateMachineBox asyncStateMachineBox3 = obj2 as IAsyncStateMachineBox;
						if (asyncStateMachineBox3 == null)
						{
							Action action3 = obj2 as Action;
							if (action3 == null)
							{
								TaskContinuation taskContinuation2 = obj2 as TaskContinuation;
								if (taskContinuation2 != null)
								{
									taskContinuation2.Run(this, flag);
								}
								else
								{
									RunOrQueueCompletionAction((ITaskCompletionAction)obj2, flag);
								}
							}
							else
							{
								AwaitTaskContinuation.RunOrScheduleAction(action3, flag);
							}
						}
						else
						{
							AwaitTaskContinuation.RunOrScheduleAction(asyncStateMachineBox3, flag);
						}
					}
					LogFinishCompletionNotification();
				}
				else
				{
					taskContinuation.Run(this, flag);
					LogFinishCompletionNotification();
				}
			}
			else
			{
				AwaitTaskContinuation.RunOrScheduleAction(action, flag);
				LogFinishCompletionNotification();
			}
		}
		else
		{
			AwaitTaskContinuation.RunOrScheduleAction(asyncStateMachineBox, flag);
			LogFinishCompletionNotification();
		}
	}

	private void RunOrQueueCompletionAction(ITaskCompletionAction completionAction, bool allowInlining)
	{
		if (allowInlining || !completionAction.InvokeMayRunArbitraryCode)
		{
			completionAction.Invoke(this);
		}
		else
		{
			ThreadPool.UnsafeQueueUserWorkItemInternal(new CompletionActionInvoker(completionAction, this), preferLocal: true);
		}
	}

	private static void LogFinishCompletionNotification()
	{
		if (TplEventSource.Log.IsEnabled())
		{
			TplEventSource.Log.TraceSynchronousWorkEnd(CausalitySynchronousWork.CompletionNotification);
		}
	}

	public Task ContinueWith(Action<Task> continuationAction)
	{
		return ContinueWith(continuationAction, TaskScheduler.Current, default(CancellationToken), TaskContinuationOptions.None);
	}

	public Task ContinueWith(Action<Task> continuationAction, CancellationToken cancellationToken)
	{
		return ContinueWith(continuationAction, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None);
	}

	public Task ContinueWith(Action<Task> continuationAction, TaskScheduler scheduler)
	{
		return ContinueWith(continuationAction, scheduler, default(CancellationToken), TaskContinuationOptions.None);
	}

	public Task ContinueWith(Action<Task> continuationAction, TaskContinuationOptions continuationOptions)
	{
		return ContinueWith(continuationAction, TaskScheduler.Current, default(CancellationToken), continuationOptions);
	}

	public Task ContinueWith(Action<Task> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		return ContinueWith(continuationAction, scheduler, cancellationToken, continuationOptions);
	}

	private Task ContinueWith(Action<Task> continuationAction, TaskScheduler scheduler, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions)
	{
		if (continuationAction == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationAction);
		}
		if (scheduler == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
		}
		CreationOptionsFromContinuationOptions(continuationOptions, out var creationOptions, out var internalOptions);
		Task task = new ContinuationTaskFromTask(this, continuationAction, null, creationOptions, internalOptions);
		ContinueWithCore(task, scheduler, cancellationToken, continuationOptions);
		return task;
	}

	public Task ContinueWith(Action<Task, object?> continuationAction, object? state)
	{
		return ContinueWith(continuationAction, state, TaskScheduler.Current, default(CancellationToken), TaskContinuationOptions.None);
	}

	public Task ContinueWith(Action<Task, object?> continuationAction, object? state, CancellationToken cancellationToken)
	{
		return ContinueWith(continuationAction, state, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None);
	}

	public Task ContinueWith(Action<Task, object?> continuationAction, object? state, TaskScheduler scheduler)
	{
		return ContinueWith(continuationAction, state, scheduler, default(CancellationToken), TaskContinuationOptions.None);
	}

	public Task ContinueWith(Action<Task, object?> continuationAction, object? state, TaskContinuationOptions continuationOptions)
	{
		return ContinueWith(continuationAction, state, TaskScheduler.Current, default(CancellationToken), continuationOptions);
	}

	public Task ContinueWith(Action<Task, object?> continuationAction, object? state, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		return ContinueWith(continuationAction, state, scheduler, cancellationToken, continuationOptions);
	}

	private Task ContinueWith(Action<Task, object> continuationAction, object state, TaskScheduler scheduler, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions)
	{
		if (continuationAction == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationAction);
		}
		if (scheduler == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
		}
		CreationOptionsFromContinuationOptions(continuationOptions, out var creationOptions, out var internalOptions);
		Task task = new ContinuationTaskFromTask(this, continuationAction, state, creationOptions, internalOptions);
		ContinueWithCore(task, scheduler, cancellationToken, continuationOptions);
		return task;
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction)
	{
		return ContinueWith(continuationFunction, TaskScheduler.Current, default(CancellationToken), TaskContinuationOptions.None);
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, CancellationToken cancellationToken)
	{
		return ContinueWith(continuationFunction, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None);
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, TaskScheduler scheduler)
	{
		return ContinueWith(continuationFunction, scheduler, default(CancellationToken), TaskContinuationOptions.None);
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, TaskContinuationOptions continuationOptions)
	{
		return ContinueWith(continuationFunction, TaskScheduler.Current, default(CancellationToken), continuationOptions);
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		return ContinueWith(continuationFunction, scheduler, cancellationToken, continuationOptions);
	}

	private Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, TaskScheduler scheduler, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions)
	{
		if (continuationFunction == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
		}
		if (scheduler == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
		}
		CreationOptionsFromContinuationOptions(continuationOptions, out var creationOptions, out var internalOptions);
		Task<TResult> task = new ContinuationResultTaskFromTask<TResult>(this, continuationFunction, null, creationOptions, internalOptions);
		ContinueWithCore(task, scheduler, cancellationToken, continuationOptions);
		return task;
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, object?, TResult> continuationFunction, object? state)
	{
		return ContinueWith(continuationFunction, state, TaskScheduler.Current, default(CancellationToken), TaskContinuationOptions.None);
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, object?, TResult> continuationFunction, object? state, CancellationToken cancellationToken)
	{
		return ContinueWith(continuationFunction, state, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None);
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, object?, TResult> continuationFunction, object? state, TaskScheduler scheduler)
	{
		return ContinueWith(continuationFunction, state, scheduler, default(CancellationToken), TaskContinuationOptions.None);
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, object?, TResult> continuationFunction, object? state, TaskContinuationOptions continuationOptions)
	{
		return ContinueWith(continuationFunction, state, TaskScheduler.Current, default(CancellationToken), continuationOptions);
	}

	public Task<TResult> ContinueWith<TResult>(Func<Task, object?, TResult> continuationFunction, object? state, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
	{
		return ContinueWith(continuationFunction, state, scheduler, cancellationToken, continuationOptions);
	}

	private Task<TResult> ContinueWith<TResult>(Func<Task, object, TResult> continuationFunction, object state, TaskScheduler scheduler, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions)
	{
		if (continuationFunction == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
		}
		if (scheduler == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
		}
		CreationOptionsFromContinuationOptions(continuationOptions, out var creationOptions, out var internalOptions);
		Task<TResult> task = new ContinuationResultTaskFromTask<TResult>(this, continuationFunction, state, creationOptions, internalOptions);
		ContinueWithCore(task, scheduler, cancellationToken, continuationOptions);
		return task;
	}

	internal static void CreationOptionsFromContinuationOptions(TaskContinuationOptions continuationOptions, out TaskCreationOptions creationOptions, out InternalTaskOptions internalOptions)
	{
		if ((continuationOptions & (TaskContinuationOptions.LongRunning | TaskContinuationOptions.ExecuteSynchronously)) == (TaskContinuationOptions.LongRunning | TaskContinuationOptions.ExecuteSynchronously))
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.continuationOptions, ExceptionResource.Task_ContinueWith_ESandLR);
		}
		if (((uint)continuationOptions & 0xFFF0FF80u) != 0)
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.continuationOptions);
		}
		if ((continuationOptions & (TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.NotOnRanToCompletion)) == (TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.NotOnRanToCompletion))
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.continuationOptions, ExceptionResource.Task_ContinueWith_NotOnAnything);
		}
		creationOptions = (TaskCreationOptions)(continuationOptions & (TaskContinuationOptions.PreferFairness | TaskContinuationOptions.LongRunning | TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.HideScheduler | TaskContinuationOptions.RunContinuationsAsynchronously));
		internalOptions = (((continuationOptions & TaskContinuationOptions.LazyCancellation) != 0) ? (InternalTaskOptions.ContinuationTask | InternalTaskOptions.LazyCancellation) : InternalTaskOptions.ContinuationTask);
	}

	internal void ContinueWithCore(Task continuationTask, TaskScheduler scheduler, CancellationToken cancellationToken, TaskContinuationOptions options)
	{
		TaskContinuation taskContinuation = new ContinueWithTaskContinuation(continuationTask, options, scheduler);
		if (cancellationToken.CanBeCanceled)
		{
			if (IsCompleted || cancellationToken.IsCancellationRequested)
			{
				continuationTask.AssignCancellationToken(cancellationToken, null, null);
			}
			else
			{
				continuationTask.AssignCancellationToken(cancellationToken, this, taskContinuation);
			}
		}
		if (continuationTask.IsCompleted)
		{
			return;
		}
		if ((Options & (TaskCreationOptions)1024) != 0 && !(this is ITaskCompletionAction))
		{
			TplEventSource log = TplEventSource.Log;
			if (log.IsEnabled())
			{
				log.AwaitTaskContinuationScheduled(TaskScheduler.Current.Id, CurrentId.GetValueOrDefault(), continuationTask.Id);
			}
		}
		if (!AddTaskContinuation(taskContinuation, addBeforeOthers: false))
		{
			taskContinuation.Run(this, canInlineContinuationTask: true);
		}
	}

	internal void AddCompletionAction(ITaskCompletionAction action, bool addBeforeOthers = false)
	{
		if (!AddTaskContinuation(action, addBeforeOthers))
		{
			action.Invoke(this);
		}
	}

	private bool AddTaskContinuationComplex(object tc, bool addBeforeOthers)
	{
		object continuationObject = m_continuationObject;
		if (continuationObject != s_taskCompletionSentinel && !(continuationObject is List<object>))
		{
			Interlocked.CompareExchange(ref m_continuationObject, new List<object>
			{
				continuationObject
			}, continuationObject);
		}
		List<object> list = m_continuationObject as List<object>;
		if (list != null)
		{
			lock (list)
			{
				if (m_continuationObject != s_taskCompletionSentinel)
				{
					if (list.Count == list.Capacity)
					{
						list.RemoveAll((object l) => l == null);
					}
					if (addBeforeOthers)
					{
						list.Insert(0, tc);
					}
					else
					{
						list.Add(tc);
					}
					return true;
				}
			}
		}
		return false;
	}

	private bool AddTaskContinuation(object tc, bool addBeforeOthers)
	{
		if (IsCompleted)
		{
			return false;
		}
		if (m_continuationObject != null || Interlocked.CompareExchange(ref m_continuationObject, tc, null) != null)
		{
			return AddTaskContinuationComplex(tc, addBeforeOthers);
		}
		return true;
	}

	internal void RemoveContinuation(object continuationObject)
	{
		object continuationObject2 = m_continuationObject;
		if (continuationObject2 == s_taskCompletionSentinel)
		{
			return;
		}
		List<object> list = continuationObject2 as List<object>;
		if (list == null)
		{
			if (Interlocked.CompareExchange(ref m_continuationObject, new List<object>(), continuationObject) == continuationObject)
			{
				return;
			}
			list = m_continuationObject as List<object>;
		}
		if (list == null)
		{
			return;
		}
		lock (list)
		{
			if (m_continuationObject != s_taskCompletionSentinel)
			{
				int num = list.IndexOf(continuationObject);
				if (num != -1)
				{
					list[num] = null;
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	public static void WaitAll(params Task[] tasks)
	{
		WaitAllCore(tasks, -1, default(CancellationToken));
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	public static bool WaitAll(Task[] tasks, TimeSpan timeout)
	{
		long num = (long)timeout.TotalMilliseconds;
		if (num < -1 || num > int.MaxValue)
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.timeout);
		}
		return WaitAllCore(tasks, (int)num, default(CancellationToken));
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	public static bool WaitAll(Task[] tasks, int millisecondsTimeout)
	{
		return WaitAllCore(tasks, millisecondsTimeout, default(CancellationToken));
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	public static void WaitAll(Task[] tasks, CancellationToken cancellationToken)
	{
		WaitAllCore(tasks, -1, cancellationToken);
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	public static bool WaitAll(Task[] tasks, int millisecondsTimeout, CancellationToken cancellationToken)
	{
		return WaitAllCore(tasks, millisecondsTimeout, cancellationToken);
	}

	private static bool WaitAllCore(Task[] tasks, int millisecondsTimeout, CancellationToken cancellationToken)
	{
		if (tasks == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
		}
		if (millisecondsTimeout < -1)
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.millisecondsTimeout);
		}
		cancellationToken.ThrowIfCancellationRequested();
		List<Exception> exceptions = null;
		List<Task> list = null;
		List<Task> list2 = null;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = true;
		for (int num = tasks.Length - 1; num >= 0; num--)
		{
			Task task = tasks[num];
			if (task == null)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Task_WaitMulti_NullTask, ExceptionArgument.tasks);
			}
			bool flag4 = task.IsCompleted;
			if (!flag4)
			{
				if (millisecondsTimeout != -1 || cancellationToken.CanBeCanceled)
				{
					AddToList(task, ref list, tasks.Length);
				}
				else
				{
					flag4 = task.WrappedTryRunInline() && task.IsCompleted;
					if (!flag4)
					{
						AddToList(task, ref list, tasks.Length);
					}
				}
			}
			if (flag4)
			{
				if (task.IsFaulted)
				{
					flag = true;
				}
				else if (task.IsCanceled)
				{
					flag2 = true;
				}
				if (task.IsWaitNotificationEnabled)
				{
					AddToList(task, ref list2, 1);
				}
			}
		}
		if (list != null)
		{
			flag3 = WaitAllBlockingCore(list, millisecondsTimeout, cancellationToken);
			if (flag3)
			{
				foreach (Task item in list)
				{
					if (item.IsFaulted)
					{
						flag = true;
					}
					else if (item.IsCanceled)
					{
						flag2 = true;
					}
					if (item.IsWaitNotificationEnabled)
					{
						AddToList(item, ref list2, 1);
					}
				}
			}
			GC.KeepAlive(tasks);
		}
		if (flag3 && list2 != null)
		{
			foreach (Task item2 in list2)
			{
				if (item2.NotifyDebuggerOfWaitCompletionIfNecessary())
				{
					break;
				}
			}
		}
		if (flag3 && (flag || flag2))
		{
			if (!flag)
			{
				cancellationToken.ThrowIfCancellationRequested();
			}
			foreach (Task t in tasks)
			{
				AddExceptionsForCompletedTask(ref exceptions, t);
			}
			ThrowHelper.ThrowAggregateException(exceptions);
		}
		return flag3;
	}

	private static void AddToList<T>(T item, ref List<T> list, int initSize)
	{
		if (list == null)
		{
			list = new List<T>(initSize);
		}
		list.Add(item);
	}

	private static bool WaitAllBlockingCore(List<Task> tasks, int millisecondsTimeout, CancellationToken cancellationToken)
	{
		bool flag = false;
		SetOnCountdownMres setOnCountdownMres = new SetOnCountdownMres(tasks.Count);
		try
		{
			foreach (Task task in tasks)
			{
				task.AddCompletionAction(setOnCountdownMres, addBeforeOthers: true);
			}
			flag = setOnCountdownMres.Wait(millisecondsTimeout, cancellationToken);
		}
		finally
		{
			if (!flag)
			{
				foreach (Task task2 in tasks)
				{
					if (!task2.IsCompleted)
					{
						task2.RemoveContinuation(setOnCountdownMres);
					}
				}
			}
		}
		return flag;
	}

	internal static void AddExceptionsForCompletedTask(ref List<Exception> exceptions, Task t)
	{
		AggregateException exceptions2 = t.GetExceptions(includeTaskCanceledExceptions: true);
		if (exceptions2 != null)
		{
			t.UpdateExceptionObservedStatus();
			if (exceptions == null)
			{
				exceptions = new List<Exception>(exceptions2.InnerExceptions.Count);
			}
			exceptions.AddRange(exceptions2.InnerExceptions);
		}
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	public static int WaitAny(params Task[] tasks)
	{
		return WaitAnyCore(tasks, -1, default(CancellationToken));
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	public static int WaitAny(Task[] tasks, TimeSpan timeout)
	{
		long num = (long)timeout.TotalMilliseconds;
		if (num < -1 || num > int.MaxValue)
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.timeout);
		}
		return WaitAnyCore(tasks, (int)num, default(CancellationToken));
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	public static int WaitAny(Task[] tasks, CancellationToken cancellationToken)
	{
		return WaitAnyCore(tasks, -1, cancellationToken);
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	public static int WaitAny(Task[] tasks, int millisecondsTimeout)
	{
		return WaitAnyCore(tasks, millisecondsTimeout, default(CancellationToken));
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	public static int WaitAny(Task[] tasks, int millisecondsTimeout, CancellationToken cancellationToken)
	{
		return WaitAnyCore(tasks, millisecondsTimeout, cancellationToken);
	}

	private static int WaitAnyCore(Task[] tasks, int millisecondsTimeout, CancellationToken cancellationToken)
	{
		if (tasks == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
		}
		if (millisecondsTimeout < -1)
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.millisecondsTimeout);
		}
		cancellationToken.ThrowIfCancellationRequested();
		int num = -1;
		for (int i = 0; i < tasks.Length; i++)
		{
			Task task = tasks[i];
			if (task == null)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Task_WaitMulti_NullTask, ExceptionArgument.tasks);
			}
			if (num == -1 && task.IsCompleted)
			{
				num = i;
			}
		}
		if (num == -1 && tasks.Length != 0)
		{
			Task<Task> task2 = TaskFactory.CommonCWAnyLogic(tasks, isSyncBlocking: true);
			if (task2.Wait(millisecondsTimeout, cancellationToken))
			{
				num = Array.IndexOf(tasks, task2.Result);
			}
			else
			{
				TaskFactory.CommonCWAnyLogicCleanup(task2);
			}
		}
		GC.KeepAlive(tasks);
		return num;
	}

	public static Task<TResult> FromResult<TResult>(TResult result)
	{
		return new Task<TResult>(result);
	}

	public static Task FromException(Exception exception)
	{
		if (exception == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.exception);
		}
		Task task = new Task();
		bool flag = task.TrySetException(exception);
		return task;
	}

	public static Task<TResult> FromException<TResult>(Exception exception)
	{
		if (exception == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.exception);
		}
		Task<TResult> task = new Task<TResult>();
		bool flag = task.TrySetException(exception);
		return task;
	}

	public static Task FromCanceled(CancellationToken cancellationToken)
	{
		if (!cancellationToken.IsCancellationRequested)
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.cancellationToken);
		}
		return new Task(canceled: true, TaskCreationOptions.None, cancellationToken);
	}

	public static Task<TResult> FromCanceled<TResult>(CancellationToken cancellationToken)
	{
		if (!cancellationToken.IsCancellationRequested)
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.cancellationToken);
		}
		return new Task<TResult>(canceled: true, default(TResult), TaskCreationOptions.None, cancellationToken);
	}

	internal static Task FromCanceled(OperationCanceledException exception)
	{
		Task task = new Task();
		bool flag = task.TrySetCanceled(exception.CancellationToken, exception);
		return task;
	}

	internal static Task<TResult> FromCanceled<TResult>(OperationCanceledException exception)
	{
		Task<TResult> task = new Task<TResult>();
		bool flag = task.TrySetCanceled(exception.CancellationToken, exception);
		return task;
	}

	public static Task Run(Action action)
	{
		return InternalStartNew(null, action, null, default(CancellationToken), TaskScheduler.Default, TaskCreationOptions.DenyChildAttach, InternalTaskOptions.None);
	}

	public static Task Run(Action action, CancellationToken cancellationToken)
	{
		return InternalStartNew(null, action, null, cancellationToken, TaskScheduler.Default, TaskCreationOptions.DenyChildAttach, InternalTaskOptions.None);
	}

	public static Task<TResult> Run<TResult>(Func<TResult> function)
	{
		return Task<TResult>.StartNew(null, function, default(CancellationToken), TaskCreationOptions.DenyChildAttach, InternalTaskOptions.None, TaskScheduler.Default);
	}

	public static Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellationToken)
	{
		return Task<TResult>.StartNew(null, function, cancellationToken, TaskCreationOptions.DenyChildAttach, InternalTaskOptions.None, TaskScheduler.Default);
	}

	public static Task Run(Func<Task?> function)
	{
		return Run(function, default(CancellationToken));
	}

	public static Task Run(Func<Task?> function, CancellationToken cancellationToken)
	{
		if (function == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.function);
		}
		if (cancellationToken.IsCancellationRequested)
		{
			return FromCanceled(cancellationToken);
		}
		Task<Task> outerTask = Task<Task>.Factory.StartNew(function, cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		return new UnwrapPromise<VoidTaskResult>(outerTask, lookForOce: true);
	}

	public static Task<TResult> Run<TResult>(Func<Task<TResult>?> function)
	{
		return Run(function, default(CancellationToken));
	}

	public static Task<TResult> Run<TResult>(Func<Task<TResult>?> function, CancellationToken cancellationToken)
	{
		if (function == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.function);
		}
		if (cancellationToken.IsCancellationRequested)
		{
			return FromCanceled<TResult>(cancellationToken);
		}
		Task<Task<TResult>> outerTask = Task<Task<TResult>>.Factory.StartNew(function, cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		return new UnwrapPromise<TResult>(outerTask, lookForOce: true);
	}

	public static Task Delay(TimeSpan delay)
	{
		return Delay(delay, default(CancellationToken));
	}

	public static Task Delay(TimeSpan delay, CancellationToken cancellationToken)
	{
		long num = (long)delay.TotalMilliseconds;
		if (num < -1 || num > int.MaxValue)
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.delay, ExceptionResource.Task_Delay_InvalidDelay);
		}
		return Delay((int)num, cancellationToken);
	}

	public static Task Delay(int millisecondsDelay)
	{
		return Delay(millisecondsDelay, default(CancellationToken));
	}

	public static Task Delay(int millisecondsDelay, CancellationToken cancellationToken)
	{
		if (millisecondsDelay < -1)
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.millisecondsDelay, ExceptionResource.Task_Delay_InvalidMillisecondsDelay);
		}
		if (!cancellationToken.IsCancellationRequested)
		{
			if (millisecondsDelay != 0)
			{
				if (!cancellationToken.CanBeCanceled)
				{
					return new DelayPromise(millisecondsDelay);
				}
				return new DelayPromiseWithCancellation(millisecondsDelay, cancellationToken);
			}
			return CompletedTask;
		}
		return FromCanceled(cancellationToken);
	}

	public static Task WhenAll(IEnumerable<Task> tasks)
	{
		Task[] array = tasks as Task[];
		if (array != null)
		{
			return WhenAll(array);
		}
		ICollection<Task> collection = tasks as ICollection<Task>;
		if (collection != null)
		{
			int num = 0;
			array = new Task[collection.Count];
			foreach (Task task in tasks)
			{
				if (task == null)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_NullTask, ExceptionArgument.tasks);
				}
				array[num++] = task;
			}
			return InternalWhenAll(array);
		}
		if (tasks == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
		}
		List<Task> list = new List<Task>();
		foreach (Task task2 in tasks)
		{
			if (task2 == null)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_NullTask, ExceptionArgument.tasks);
			}
			list.Add(task2);
		}
		return InternalWhenAll(list.ToArray());
	}

	public static Task WhenAll(params Task[] tasks)
	{
		if (tasks == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
		}
		int num = tasks.Length;
		if (num == 0)
		{
			return InternalWhenAll(tasks);
		}
		Task[] array = new Task[num];
		for (int i = 0; i < num; i++)
		{
			Task task = tasks[i];
			if (task == null)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_NullTask, ExceptionArgument.tasks);
			}
			array[i] = task;
		}
		return InternalWhenAll(array);
	}

	private static Task InternalWhenAll(Task[] tasks)
	{
		if (tasks.Length != 0)
		{
			return new WhenAllPromise(tasks);
		}
		return CompletedTask;
	}

	public static Task<TResult[]> WhenAll<TResult>(IEnumerable<Task<TResult>> tasks)
	{
		Task<TResult>[] array = tasks as Task<TResult>[];
		if (array != null)
		{
			return WhenAll(array);
		}
		ICollection<Task<TResult>> collection = tasks as ICollection<Task<TResult>>;
		if (collection != null)
		{
			int num = 0;
			array = new Task<TResult>[collection.Count];
			foreach (Task<TResult> task in tasks)
			{
				if (task == null)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_NullTask, ExceptionArgument.tasks);
				}
				array[num++] = task;
			}
			return InternalWhenAll(array);
		}
		if (tasks == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
		}
		List<Task<TResult>> list = new List<Task<TResult>>();
		foreach (Task<TResult> task2 in tasks)
		{
			if (task2 == null)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_NullTask, ExceptionArgument.tasks);
			}
			list.Add(task2);
		}
		return InternalWhenAll(list.ToArray());
	}

	public static Task<TResult[]> WhenAll<TResult>(params Task<TResult>[] tasks)
	{
		if (tasks == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
		}
		int num = tasks.Length;
		if (num == 0)
		{
			return InternalWhenAll(tasks);
		}
		Task<TResult>[] array = new Task<TResult>[num];
		for (int i = 0; i < num; i++)
		{
			Task<TResult> task = tasks[i];
			if (task == null)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_NullTask, ExceptionArgument.tasks);
			}
			array[i] = task;
		}
		return InternalWhenAll(array);
	}

	private static Task<TResult[]> InternalWhenAll<TResult>(Task<TResult>[] tasks)
	{
		if (tasks.Length != 0)
		{
			return new WhenAllPromise<TResult>(tasks);
		}
		return new Task<TResult[]>(canceled: false, Array.Empty<TResult>(), TaskCreationOptions.None, default(CancellationToken));
	}

	public static Task<Task> WhenAny(params Task[] tasks)
	{
		if (tasks == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
		}
		if (tasks.Length == 2)
		{
			return WhenAny(tasks[0], tasks[1]);
		}
		if (tasks.Length == 0)
		{
			ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_EmptyTaskList, ExceptionArgument.tasks);
		}
		int num = tasks.Length;
		Task[] array = new Task[num];
		for (int i = 0; i < num; i++)
		{
			Task task = tasks[i];
			if (task == null)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_NullTask, ExceptionArgument.tasks);
			}
			array[i] = task;
		}
		return TaskFactory.CommonCWAnyLogic(array);
	}

	public static Task<Task> WhenAny(Task task1, Task task2)
	{
		if (task1 != null && task2 != null)
		{
			if (!task1.IsCompleted)
			{
				if (!task2.IsCompleted)
				{
					return new TwoTaskWhenAnyPromise<Task>(task1, task2);
				}
				return FromResult(task2);
			}
			return FromResult(task1);
		}
		throw new ArgumentNullException((task1 == null) ? "task1" : "task2");
	}

	public static Task<Task> WhenAny(IEnumerable<Task> tasks)
	{
		if (tasks == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
		}
		List<Task> list = new List<Task>();
		foreach (Task task in tasks)
		{
			if (task == null)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_NullTask, ExceptionArgument.tasks);
			}
			list.Add(task);
		}
		if (list.Count == 0)
		{
			ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_EmptyTaskList, ExceptionArgument.tasks);
		}
		return TaskFactory.CommonCWAnyLogic(list);
	}

	public static Task<Task<TResult>> WhenAny<TResult>(params Task<TResult>[] tasks)
	{
		if (tasks != null && tasks.Length == 2)
		{
			return WhenAny(tasks[0], tasks[1]);
		}
		Task<Task> task = WhenAny((Task[])tasks);
		return task.ContinueWith(Task<TResult>.TaskWhenAnyCast.Value, default(CancellationToken), TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
	}

	public static Task<Task<TResult>> WhenAny<TResult>(Task<TResult> task1, Task<TResult> task2)
	{
		if (task1 != null && task2 != null)
		{
			if (!task1.IsCompleted)
			{
				if (!task2.IsCompleted)
				{
					return new TwoTaskWhenAnyPromise<Task<TResult>>(task1, task2);
				}
				return FromResult(task2);
			}
			return FromResult(task1);
		}
		throw new ArgumentNullException((task1 == null) ? "task1" : "task2");
	}

	public static Task<Task<TResult>> WhenAny<TResult>(IEnumerable<Task<TResult>> tasks)
	{
		Task<Task> task = WhenAny((IEnumerable<Task>)tasks);
		return task.ContinueWith(Task<TResult>.TaskWhenAnyCast.Value, default(CancellationToken), TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
	}

	internal static Task<TResult> CreateUnwrapPromise<TResult>(Task outerTask, bool lookForOce)
	{
		return new UnwrapPromise<TResult>(outerTask, lookForOce);
	}

	internal virtual Delegate[] GetDelegateContinuationsForDebugger()
	{
		if (m_continuationObject != this)
		{
			return GetDelegatesFromContinuationObject(m_continuationObject);
		}
		return null;
	}

	private static Delegate[] GetDelegatesFromContinuationObject(object continuationObject)
	{
		if (continuationObject != null)
		{
			Action action = continuationObject as Action;
			if (action != null)
			{
				return new Delegate[1]
				{
					AsyncMethodBuilderCore.TryGetStateMachineForDebugger(action)
				};
			}
			TaskContinuation taskContinuation = continuationObject as TaskContinuation;
			if (taskContinuation != null)
			{
				return taskContinuation.GetDelegateContinuationsForDebugger();
			}
			Task task = continuationObject as Task;
			if (task != null)
			{
				Delegate[] delegateContinuationsForDebugger = task.GetDelegateContinuationsForDebugger();
				if (delegateContinuationsForDebugger != null)
				{
					return delegateContinuationsForDebugger;
				}
			}
			ITaskCompletionAction taskCompletionAction = continuationObject as ITaskCompletionAction;
			if (taskCompletionAction != null)
			{
				return new Delegate[1]
				{
					new Action<Task>(taskCompletionAction.Invoke)
				};
			}
			List<object> list = continuationObject as List<object>;
			if (list != null)
			{
				List<Delegate> list2 = new List<Delegate>();
				foreach (object item in list)
				{
					Delegate[] delegatesFromContinuationObject = GetDelegatesFromContinuationObject(item);
					if (delegatesFromContinuationObject == null)
					{
						continue;
					}
					Delegate[] array = delegatesFromContinuationObject;
					foreach (Delegate @delegate in array)
					{
						if ((object)@delegate != null)
						{
							list2.Add(@delegate);
						}
					}
				}
				return list2.ToArray();
			}
		}
		return null;
	}

	private static Task GetActiveTaskFromId(int taskId)
	{
		Task value = null;
		s_currentActiveTasks?.TryGetValue(taskId, out value);
		return value;
	}
}

```