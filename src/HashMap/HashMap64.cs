using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SMath = System.Math;

namespace HashMapNamespace
{
	public sealed class HashMap64<TValue> : IEnumerable<KeyValuePair<long, TValue>>, IReadOnlyDictionary<long, TValue>
	{
		private const float loadFactor = 0.75f;
		private const float collisionRatio = 0.15f;

		private int count;
		private int capacity;
		private int limitCapacity;

		private HashMap64Node[] buckets;

		private int collisionEndIndex;
		private int emptySlot;

		public HashMap64() :
			this(8)
		{
		}

		public HashMap64(int capacity)
		{
			this.capacity = HashHelpers<long>.CalculateCapacity(capacity, loadFactor, out limitCapacity);
			this.buckets = new HashMap64Node[this.capacity];
			this.collisionEndIndex = this.capacity;

			emptySlot = -1;
		}

		public HashMap64(HashMap64<TValue> map) :
			this(map.count)
		{
			foreach (long key in map.Keys)
			{
				this.Add(key, map[key]);
			}
		}

		public int Count
		{
			get { return this.count; }
		}

		public int Capacity
		{
			get { return capacity; }
		}

		public void ProvideCapacity(int tempIdCount)
		{
			if (limitCapacity > tempIdCount)
				return;

			Resize(tempIdCount);
		}

		private void Resize()
		{
			Resize(count * 2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Resize(int size)
		{
			HashMap64<TValue> newMap = new HashMap64<TValue>(size);

			for (int i = 0; i < buckets.Length; i++)
			{
				HashMap64Node node = buckets[i];
				if (node.NonEmpty)
					newMap.Add(node.Key, node.Value);
			}

			capacity = newMap.capacity;
			limitCapacity = newMap.limitCapacity;
			buckets = newMap.buckets;
			collisionEndIndex = newMap.collisionEndIndex;
			emptySlot = newMap.emptySlot;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int GetBucket(long key)
		{
			return (int)(((uint)key ^ (uint)(key >> 32)) % (uint)capacity);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Insert(long key, TValue value)
		{
			ref HashMap64Node bucket = ref buckets[GetBucket(key)];
			HashMap64Node v = bucket;

			if (v.NonEmpty == false)
			{
				bucket = new HashMap64Node(value, key, -1);
				count++;
				return;
			}

			if (v.Key == key)
				throw new ArgumentException("Given key already exists in the map.");

			int curr = v.Next;
			while (curr != -1)
			{
				if (buckets[curr].Key == key)
					throw new ArgumentException("Given key already exists in the map.");

				curr = buckets[curr].Next;
			}

			HandleCollisions(key, value, v);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void InsertOrUpdate(long key, TValue value, out bool inserted)
		{
			ref HashMap64Node bucket = ref buckets[GetBucket(key)];
			HashMap64Node v = bucket;

			if (v.NonEmpty == false)
			{
				bucket = new HashMap64Node(value, key, -1);
				count++;
				inserted = true;
				return;
			}

			if (v.Key == key)
			{
				bucket.Value = value;
				inserted = false;
				return;
			}

			int curr = v.Next;
			while (curr != -1)
			{
				if (buckets[curr].Key == key)
				{
					buckets[curr].Value = value;
					inserted = false;
					return;
				}

				curr = buckets[curr].Next;
			}

			inserted = true;

			HandleCollisions(key, value, v);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void HandleCollisions(long key, TValue value, HashMap64Node v)
		{
			int collPos;
			if (emptySlot != -1)
			{
				collPos = emptySlot;
				emptySlot = buckets[emptySlot].Next;
			}
			else
			{
				if (collisionEndIndex == buckets.Length)
				{
					int collisionSize = SMath.Max(2, (int)SMath.Round(capacity * collisionRatio));
					Array.Resize(ref buckets, buckets.Length + collisionSize);
				}

				collPos = collisionEndIndex++;
			}

			int tempNext = v.Next;
			v.Next = collPos;
			buckets[collPos] = new HashMap64Node(value, key, tempNext);
			count++;

			if (count > limitCapacity)
				Resize();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(long key, TValue value)
		{
			Insert(key, value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool AddOrUpdate(long key, TValue value)
		{
			InsertOrUpdate(key, value, out bool inserted);
			return inserted;
		}

		public bool Remove(long key)
		{
			ref HashMap64Node bucket = ref buckets[GetBucket(key)];
			HashMap64Node v = bucket;
			if (v.Key == key)
			{
				count--;

				if (v.Next == -1)
				{
					bucket.Value = default(TValue);
					bucket.Key = 0;
					return true;
				}

				bucket = buckets[v.Next];
				buckets[v.Next] = new HashMap64Node(default(TValue), 0, emptySlot);
				emptySlot = v.Next;
				return true;
			}

			if (v.NonEmpty == false)
				return false;

			if (v.Next == -1)
				return false;

			HashMap64Node c = buckets[v.Next];
			if (c.Key == key)
			{
				bucket.Next = c.Next;
				buckets[v.Next] = new HashMap64Node(default(TValue), 0, emptySlot);
				emptySlot = v.Next;
				count--;
				return true;
			}

			int prev = v.Next;
			int curr = c.Next;
			while (curr != -1)
			{
				if (buckets[curr].Key == key)
				{
					buckets[prev].Next = buckets[curr].Next;
					buckets[curr] = new HashMap64Node(default(TValue), 0, emptySlot);
					emptySlot = curr;
					count--;
					return true;
				}

				prev = curr;
				curr = buckets[curr].Next;
			}

			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool ContainsKey(long key)
		{
			return TryGetValue(key, out TValue value);
		}

		public TValue this[long key]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (!TryGetValue(key, out TValue value))
					throw new KeyNotFoundException("Given key was not found in the map.");

				return value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				InsertOrUpdate(key, value, out bool inserted);
			}
		}

		public bool TryGetValue(long key, out TValue value)
		{
			int bucket = GetBucket(key);
			HashMap64Node v = buckets[bucket];
			if (v.Key == key)
			{
				value = v.Value;
				return true;
			}

			value = default(TValue);
			if (v.NonEmpty == false)
				return false;

			if (v.Next == -1)
				return false;

			int pos = v.Next;
			while (pos != -1)
			{
				v = buckets[pos];
				if (v.Key == key)
				{
					value = v.Value;
					return true;
				}

				pos = v.Next;
			}

			return false;
		}

		public void Clear()
		{
			if (count == 0)
				return;

			for (int i = 0; i < buckets.Length; i++)
			{
				buckets[i].Key = 0;
				buckets[i].Value = default(TValue);
				buckets[i].NonEmpty = false;
			}

			collisionEndIndex = 0;
			emptySlot = -1;
			count = 0;
		}

		public IEnumerable<long> Keys
		{
			get
			{
				for (int i = 0; i < buckets.Length; i++)
				{
					HashMap64Node node = buckets[i];
					if (node.NonEmpty)
						yield return node.Key;
				}

			}
		}

		public IEnumerable<TValue> Values
		{
			get
			{
				for (int i = 0; i < buckets.Length; i++)
				{
					HashMap64Node node = buckets[i];
					if (node.NonEmpty)
						yield return node.Value;
				}
			
			}
		}

		public IEnumerator<KeyValuePair<long, TValue>> GetEnumerator()
		{
			for (int i = 0; i < buckets.Length; i++)
			{
				HashMap64Node node = buckets[i];
				if (node.NonEmpty)
					yield return new KeyValuePair<long, TValue>(node.Key, node.Value);
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)] // Size 1 just tell the compiler to use the smallest possible size (do not pad the struct)
		private struct HashMap64Node
		{
			private long key;
			private TValue value;
			private int next;
			private bool nonEmpty;

			public HashMap64Node(TValue value, long key, int next)
			{
				this.value = value;
				this.key = key;
				this.next = next;
				nonEmpty = true;
			}

			public long Key
			{
				get { return this.key; }
				set { this.key = value; }
			}

			public TValue Value
			{
				get { return this.value; }
				set { this.value = value; }
			}

			public int Next
			{
				get { return next; }
				set { next = value; }
			}

			public bool NonEmpty
			{
				get { return nonEmpty; }
				set { nonEmpty = value; }
			}

			public override string ToString()
			{
				return string.Format("(key={0}, value={1}, next={2})", key, value, next);
			}
		}

	}
}