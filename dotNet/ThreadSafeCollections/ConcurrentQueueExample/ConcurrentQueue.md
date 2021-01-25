ILSpy

```csharp
// System.Collections.Concurrent.ConcurrentQueue<T>
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(IProducerConsumerCollectionDebugView<>))]
public class ConcurrentQueue<T> : IProducerConsumerCollection<T>, IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
{
	private readonly object _crossSegmentLock;

	private volatile ConcurrentQueueSegment<T> _tail;

	private volatile ConcurrentQueueSegment<T> _head;

	bool ICollection.IsSynchronized => false;

	object ICollection.SyncRoot
	{
		get
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.ConcurrentCollection_SyncRoot_NotSupported);
			return null;
		}
	}

	public bool IsEmpty
	{
		get
		{
			T result;
			return !TryPeek(out result, resultUsed: false);
		}
	}

	public int Count
	{
		get
		{
			SpinWait spinWait = default(SpinWait);
			ConcurrentQueueSegment<T> head;
			ConcurrentQueueSegment<T> tail;
			int num;
			int num2;
			int num3;
			int num4;
			while (true)
			{
				head = _head;
				tail = _tail;
				num = Volatile.Read(ref head._headAndTail.Head);
				num2 = Volatile.Read(ref head._headAndTail.Tail);
				if (head == tail)
				{
					if (head == _head && tail == _tail && num == Volatile.Read(ref head._headAndTail.Head) && num2 == Volatile.Read(ref head._headAndTail.Tail))
					{
						return GetCount(head, num, num2);
					}
				}
				else if (head._nextSegment == tail)
				{
					num3 = Volatile.Read(ref tail._headAndTail.Head);
					num4 = Volatile.Read(ref tail._headAndTail.Tail);
					if (head == _head && tail == _tail && num == Volatile.Read(ref head._headAndTail.Head) && num2 == Volatile.Read(ref head._headAndTail.Tail) && num3 == Volatile.Read(ref tail._headAndTail.Head) && num4 == Volatile.Read(ref tail._headAndTail.Tail))
					{
						break;
					}
				}
				else
				{
					lock (_crossSegmentLock)
					{
						if (head == _head && tail == _tail)
						{
							int num5 = Volatile.Read(ref tail._headAndTail.Head);
							int num6 = Volatile.Read(ref tail._headAndTail.Tail);
							if (num == Volatile.Read(ref head._headAndTail.Head) && num2 == Volatile.Read(ref head._headAndTail.Tail) && num5 == Volatile.Read(ref tail._headAndTail.Head) && num6 == Volatile.Read(ref tail._headAndTail.Tail))
							{
								int num7 = GetCount(head, num, num2) + GetCount(tail, num5, num6);
								for (ConcurrentQueueSegment<T> nextSegment = head._nextSegment; nextSegment != tail; nextSegment = nextSegment._nextSegment)
								{
									num7 += nextSegment._headAndTail.Tail - nextSegment.FreezeOffset;
								}
								return num7;
							}
						}
					}
				}
				spinWait.SpinOnce();
			}
			return GetCount(head, num, num2) + GetCount(tail, num3, num4);
		}
	}

	public ConcurrentQueue()
	{
		_crossSegmentLock = new object();
		_tail = (_head = new ConcurrentQueueSegment<T>(32));
	}

	public ConcurrentQueue(IEnumerable<T> collection)
	{
		if (collection == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
		}
		_crossSegmentLock = new object();
		int num = 32;
		ICollection<T> collection2 = collection as ICollection<T>;
		if (collection2 != null)
		{
			int count = collection2.Count;
			if (count > num)
			{
				num = Math.Min(ConcurrentQueueSegment<T>.RoundUpToPowerOf2(count), 1048576);
			}
		}
		_tail = (_head = new ConcurrentQueueSegment<T>(num));
		foreach (T item in collection)
		{
			Enqueue(item);
		}
	}

	void ICollection.CopyTo(Array array, int index)
	{
		T[] array2 = array as T[];
		if (array2 != null)
		{
			CopyTo(array2, index);
			return;
		}
		if (array == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
		}
		ToArray().CopyTo(array, index);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<T>)this).GetEnumerator();
	}

	bool IProducerConsumerCollection<T>.TryAdd(T item)
	{
		Enqueue(item);
		return true;
	}

	bool IProducerConsumerCollection<T>.TryTake([MaybeNullWhen(false)] out T item)
	{
		return TryDequeue(out item);
	}

	public T[] ToArray()
	{
		SnapForObservation(out var head, out var headHead, out var tail, out var tailTail);
		long count = GetCount(head, headHead, tail, tailTail);
		T[] array = new T[count];
		using IEnumerator<T> enumerator = Enumerate(head, headHead, tail, tailTail);
		int num = 0;
		while (enumerator.MoveNext())
		{
			array[num++] = enumerator.Current;
		}
		return array;
	}

	private static int GetCount(ConcurrentQueueSegment<T> s, int head, int tail)
	{
		if (head != tail && head != tail - s.FreezeOffset)
		{
			head &= s._slotsMask;
			tail &= s._slotsMask;
			if (head >= tail)
			{
				return s._slots.Length - head + tail;
			}
			return tail - head;
		}
		return 0;
	}

	private static long GetCount(ConcurrentQueueSegment<T> head, int headHead, ConcurrentQueueSegment<T> tail, int tailTail)
	{
		long num = 0L;
		int num2 = ((head == tail) ? tailTail : Volatile.Read(ref head._headAndTail.Tail)) - head.FreezeOffset;
		if (headHead < num2)
		{
			headHead &= head._slotsMask;
			num2 &= head._slotsMask;
			num += ((headHead < num2) ? (num2 - headHead) : (head._slots.Length - headHead + num2));
		}
		if (head != tail)
		{
			for (ConcurrentQueueSegment<T> nextSegment = head._nextSegment; nextSegment != tail; nextSegment = nextSegment._nextSegment)
			{
				num += nextSegment._headAndTail.Tail - nextSegment.FreezeOffset;
			}
			num += tailTail - tail.FreezeOffset;
		}
		return num;
	}

	public void CopyTo(T[] array, int index)
	{
		if (array == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
		}
		if (index < 0)
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
		}
		SnapForObservation(out var head, out var headHead, out var tail, out var tailTail);
		long count = GetCount(head, headHead, tail, tailTail);
		if (index > array.Length - count)
		{
			ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
		}
		int num = index;
		using IEnumerator<T> enumerator = Enumerate(head, headHead, tail, tailTail);
		while (enumerator.MoveNext())
		{
			array[num++] = enumerator.Current;
		}
	}

	public IEnumerator<T> GetEnumerator()
	{
		SnapForObservation(out var head, out var headHead, out var tail, out var tailTail);
		return Enumerate(head, headHead, tail, tailTail);
	}

	private void SnapForObservation(out ConcurrentQueueSegment<T> head, out int headHead, out ConcurrentQueueSegment<T> tail, out int tailTail)
	{
		lock (_crossSegmentLock)
		{
			head = _head;
			tail = _tail;
			ConcurrentQueueSegment<T> concurrentQueueSegment = head;
			while (true)
			{
				concurrentQueueSegment._preservedForObservation = true;
				if (concurrentQueueSegment == tail)
				{
					break;
				}
				concurrentQueueSegment = concurrentQueueSegment._nextSegment;
			}
			tail.EnsureFrozenForEnqueues();
			headHead = Volatile.Read(ref head._headAndTail.Head);
			tailTail = Volatile.Read(ref tail._headAndTail.Tail);
		}
	}

	private static T GetItemWhenAvailable(ConcurrentQueueSegment<T> segment, int i)
	{
		int num = (i + 1) & segment._slotsMask;
		if ((segment._slots[i].SequenceNumber & segment._slotsMask) != num)
		{
			SpinWait spinWait = default(SpinWait);
			while ((Volatile.Read(ref segment._slots[i].SequenceNumber) & segment._slotsMask) != num)
			{
				spinWait.SpinOnce();
			}
		}
		return segment._slots[i].Item;
	}

	private IEnumerator<T> Enumerate(ConcurrentQueueSegment<T> head, int headHead, ConcurrentQueueSegment<T> tail, int tailTail)
	{
		int headTail = ((head == tail) ? tailTail : Volatile.Read(ref head._headAndTail.Tail)) - head.FreezeOffset;
		if (headHead < headTail)
		{
			headHead &= head._slotsMask;
			headTail &= head._slotsMask;
			if (headHead < headTail)
			{
				for (int n = headHead; n < headTail; n++)
				{
					yield return GetItemWhenAvailable(head, n);
				}
			}
			else
			{
				for (int n = headHead; n < head._slots.Length; n++)
				{
					yield return GetItemWhenAvailable(head, n);
				}
				for (int n = 0; n < headTail; n++)
				{
					yield return GetItemWhenAvailable(head, n);
				}
			}
		}
		if (head == tail)
		{
			yield break;
		}
		for (ConcurrentQueueSegment<T> s = head._nextSegment; s != tail; s = s._nextSegment)
		{
			int n = s._headAndTail.Tail - s.FreezeOffset;
			for (int i = 0; i < n; i++)
			{
				yield return GetItemWhenAvailable(s, i);
			}
		}
		tailTail -= tail.FreezeOffset;
		for (int n = 0; n < tailTail; n++)
		{
			yield return GetItemWhenAvailable(tail, n);
		}
	}

	public void Enqueue(T item)
	{
		if (!_tail.TryEnqueue(item))
		{
			EnqueueSlow(item);
		}
	}

	private void EnqueueSlow(T item)
	{
		while (true)
		{
			ConcurrentQueueSegment<T> tail = _tail;
			if (tail.TryEnqueue(item))
			{
				break;
			}
			lock (_crossSegmentLock)
			{
				if (tail == _tail)
				{
					tail.EnsureFrozenForEnqueues();
					int boundedLength = (tail._preservedForObservation ? 32 : Math.Min(tail.Capacity * 2, 1048576));
					_tail = (tail._nextSegment = new ConcurrentQueueSegment<T>(boundedLength));
				}
			}
		}
	}

	public bool TryDequeue([MaybeNullWhen(false)] out T result)
	{
		if (!_head.TryDequeue(out result))
		{
			return TryDequeueSlow(out result);
		}
		return true;
	}

	private bool TryDequeueSlow([MaybeNullWhen(false)] out T item)
	{
		while (true)
		{
			ConcurrentQueueSegment<T> head = _head;
			if (head.TryDequeue(out item))
			{
				return true;
			}
			if (head._nextSegment == null)
			{
				item = default(T);
				return false;
			}
			if (head.TryDequeue(out item))
			{
				break;
			}
			lock (_crossSegmentLock)
			{
				if (head == _head)
				{
					_head = head._nextSegment;
				}
			}
		}
		return true;
	}

	public bool TryPeek([MaybeNullWhen(false)] out T result)
	{
		return TryPeek(out result, resultUsed: true);
	}

	private bool TryPeek([MaybeNullWhen(false)] out T result, bool resultUsed)
	{
		ConcurrentQueueSegment<T> concurrentQueueSegment = _head;
		while (true)
		{
			ConcurrentQueueSegment<T> concurrentQueueSegment2 = Volatile.Read(ref concurrentQueueSegment._nextSegment);
			if (concurrentQueueSegment.TryPeek(out result, resultUsed))
			{
				return true;
			}
			if (concurrentQueueSegment2 != null)
			{
				concurrentQueueSegment = concurrentQueueSegment2;
			}
			else if (Volatile.Read(ref concurrentQueueSegment._nextSegment) == null)
			{
				break;
			}
		}
		result = default(T);
		return false;
	}

	public void Clear()
	{
		lock (_crossSegmentLock)
		{
			_tail.EnsureFrozenForEnqueues();
			_tail = (_head = new ConcurrentQueueSegment<T>(32));
		}
	}
}

```


```csharp
// System.Collections.Concurrent.ConcurrentQueueSegment<T>
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;

[DebuggerDisplay("Capacity = {Capacity}")]
internal sealed class ConcurrentQueueSegment<T>
{
	[StructLayout(LayoutKind.Auto)]
	[DebuggerDisplay("Item = {Item}, SequenceNumber = {SequenceNumber}")]
	internal struct Slot
	{
		public T Item;

		public int SequenceNumber;
	}

	internal readonly Slot[] _slots;

	internal readonly int _slotsMask;

	internal PaddedHeadAndTail _headAndTail;

	internal bool _preservedForObservation;

	internal bool _frozenForEnqueues;

	internal ConcurrentQueueSegment<T> _nextSegment;

	internal int Capacity => _slots.Length;

	internal int FreezeOffset => _slots.Length * 2;

	internal ConcurrentQueueSegment(int boundedLength)
	{
		_slots = new Slot[boundedLength];
		_slotsMask = boundedLength - 1;
		for (int i = 0; i < _slots.Length; i++)
		{
			_slots[i].SequenceNumber = i;
		}
	}

	internal static int RoundUpToPowerOf2(int i)
	{
		i--;
		i |= i >> 1;
		i |= i >> 2;
		i |= i >> 4;
		i |= i >> 8;
		i |= i >> 16;
		return i + 1;
	}

	internal void EnsureFrozenForEnqueues()
	{
		if (!_frozenForEnqueues)
		{
			_frozenForEnqueues = true;
			Interlocked.Add(ref _headAndTail.Tail, FreezeOffset);
		}
	}

	public bool TryDequeue([MaybeNullWhen(false)] out T item)
	{
		Slot[] slots = _slots;
		SpinWait spinWait = default(SpinWait);
		while (true)
		{
			int num = Volatile.Read(ref _headAndTail.Head);
			int num2 = num & _slotsMask;
			int num3 = Volatile.Read(ref slots[num2].SequenceNumber);
			int num4 = num3 - (num + 1);
			if (num4 == 0)
			{
				if (Interlocked.CompareExchange(ref _headAndTail.Head, num + 1, num) == num)
				{
					item = slots[num2].Item;
					if (!Volatile.Read(ref _preservedForObservation))
					{
						slots[num2].Item = default(T);
						Volatile.Write(ref slots[num2].SequenceNumber, num + slots.Length);
					}
					return true;
				}
			}
			else if (num4 < 0)
			{
				bool frozenForEnqueues = _frozenForEnqueues;
				int num5 = Volatile.Read(ref _headAndTail.Tail);
				if (num5 - num <= 0 || (frozenForEnqueues && num5 - FreezeOffset - num <= 0))
				{
					break;
				}
			}
			spinWait.SpinOnce(-1);
		}
		item = default(T);
		return false;
	}

	public bool TryPeek([MaybeNullWhen(false)] out T result, bool resultUsed)
	{
		if (resultUsed)
		{
			_preservedForObservation = true;
			Interlocked.MemoryBarrier();
		}
		Slot[] slots = _slots;
		SpinWait spinWait = default(SpinWait);
		while (true)
		{
			int num = Volatile.Read(ref _headAndTail.Head);
			int num2 = num & _slotsMask;
			int num3 = Volatile.Read(ref slots[num2].SequenceNumber);
			int num4 = num3 - (num + 1);
			if (num4 == 0)
			{
				result = (resultUsed ? slots[num2].Item : default(T));
				return true;
			}
			if (num4 < 0)
			{
				bool frozenForEnqueues = _frozenForEnqueues;
				int num5 = Volatile.Read(ref _headAndTail.Tail);
				if (num5 - num <= 0 || (frozenForEnqueues && num5 - FreezeOffset - num <= 0))
				{
					break;
				}
			}
			spinWait.SpinOnce(-1);
		}
		result = default(T);
		return false;
	}

	public bool TryEnqueue(T item)
	{
		Slot[] slots = _slots;
		SpinWait spinWait = default(SpinWait);
		while (true)
		{
			int num = Volatile.Read(ref _headAndTail.Tail);
			int num2 = num & _slotsMask;
			int num3 = Volatile.Read(ref slots[num2].SequenceNumber);
			int num4 = num3 - num;
			if (num4 == 0)
			{
				if (Interlocked.CompareExchange(ref _headAndTail.Tail, num + 1, num) == num)
				{
					slots[num2].Item = item;
					Volatile.Write(ref slots[num2].SequenceNumber, num + 1);
					return true;
				}
			}
			else if (num4 < 0)
			{
				break;
			}
			spinWait.SpinOnce(-1);
		}
		return false;
	}
}

```