using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashMapNamespace;

namespace TestHashMap
{
	internal class Program
	{
		const int count = 10000000;
		const int creationCount = 10000000;
		static long[] nn = new long[count];
		static long s = 0;
		/// <summary>
		/// Mains the specified arguments.
		/// </summary>
		/// <param name="args">The arguments.</param>
		static unsafe void Main(string[] args)
		{
			long StopBytes = 0;
			long StartBytes = 0;

			int elemCount = 1;

			// Declare your dictionary
			Dictionary<long, Object> myDictionary;

			// Get total memory before create your dictionary
			StartBytes = System.GC.GetTotalMemory(true);
			// Initialize your dictionary
			myDictionary = new Dictionary<long, Object>(elemCount);
			// Get total memory after create your dictionary
			StopBytes = System.GC.GetTotalMemory(true);
			// This ensure a reference to object keeps object in memory
			GC.KeepAlive(myDictionary);


			HashMap64<Object> myIdHashMap;
			// Get total memory before create your dictionary
			StartBytes = System.GC.GetTotalMemory(true);
			// Initialize your dictionary
			myIdHashMap = new HashMap64<Object>(elemCount);
			// Get total memory after create your dictionary
			StopBytes = System.GC.GetTotalMemory(true);
			// This ensure a reference to object keeps object in memory
			GC.KeepAlive(myIdHashMap);

			string test = "1234567890";
			string copy = test;

			fixed (char* pointer = test)
			{
				// Add one to each of the characters.
				for (int i = 0; pointer[i] != '\0'; ++i)
				{
					pointer[i]++;
				}
			}

			test += "1234567890";


			


			for (int i = 0; i < 10; i++)
			{
				Console.WriteLine("Element count: " + elemCount);

				// Get total memory before create your dictionary
				StartBytes = System.GC.GetTotalMemory(true);
				// Initialize your dictionary
				myDictionary = new Dictionary<long, Object>(elemCount);
				for (int j = 1; j <= elemCount; j++)
				{
					myDictionary.Add(j, j);
				}
				// Get total memory after create your dictionary
				StopBytes = System.GC.GetTotalMemory(true);
				// This ensure a reference to object keeps object in memory
				GC.KeepAlive(myDictionary);
				// Calculate the difference , and that all :-)
				Console.WriteLine("\tDictionary Size is \t" + ((long)(StopBytes - StartBytes)).ToString());

				// Get total memory before create your dictionary
				StartBytes = System.GC.GetTotalMemory(true);
				// Initialize your dictionary
				myIdHashMap = new HashMap64<Object>(elemCount);
				for (int j = 1; j <= elemCount; j++)
				{
					myIdHashMap.Add(j, j);
				}
				// Get total memory after create your dictionary
				StopBytes = System.GC.GetTotalMemory(true);
				// This ensure a reference to object keeps object in memory
				GC.KeepAlive(myIdHashMap);
				// Calculate the difference , and that all :-)
				Console.WriteLine("\tIdHaskMap Size is \t" + ((long)(StopBytes - StartBytes)).ToString());


				elemCount = elemCount << 1;
			}

			for (int i = 0; i < count; i++)
			{
				nn[i] = i + 1;
			}


			// shuffle
			Random rand = new Random();
			for (int i = 0; i < count; i++)
			{
				int n1 = rand.Next(count);
				int n2 = rand.Next(count);
				long t = nn[n1];
				nn[n1] = nn[n2];
				nn[n2] = t;
			}

			for (int k = 0; k < 5; k++)
			{
				Console.WriteLine("ITERATION: {0}", k);

				Dictionary<long, long> d = new Dictionary<long, long>();
				HashMap64<long> l = new HashMap64<long>();

				Stopwatch sss;

				sss = Stopwatch.StartNew();
				TestInsertDict(d);
				sss.Stop();
				Console.WriteLine("Insert Dictionary: {0}", sss.ElapsedMilliseconds);

				sss = Stopwatch.StartNew();
				TestInsetIHM(l);
				sss.Stop();
				Console.WriteLine("Insert HashMap64: {0}", sss.ElapsedMilliseconds);

				


				sss = Stopwatch.StartNew();
				TestCreateInsertDict(k + 1);
				sss.Stop();
				Console.WriteLine("Create Insert Dictionary: {0}", sss.ElapsedMilliseconds);

				sss = Stopwatch.StartNew();
				TestCreateInsetIHM(k + 1);
				sss.Stop();
				Console.WriteLine("Create Insert HashMap64: {0}", sss.ElapsedMilliseconds);

				s = 0;
				sss = Stopwatch.StartNew();
				TestReadDict(d);
				sss.Stop();
				Console.WriteLine("Read Dictionary: {0}", sss.ElapsedMilliseconds);

				s = 0;
				sss = Stopwatch.StartNew();
				TestReadIHM(l);
				sss.Stop();
				Console.WriteLine("Read HashMap64: {0}", sss.ElapsedMilliseconds);

				

				sss = Stopwatch.StartNew();
				TestIterDict(d);
				sss.Stop();
				Console.WriteLine("Iteration Dictionary: {0}", sss.ElapsedMilliseconds);

				sss = Stopwatch.StartNew();
				TestIterIHM(l);
				sss.Stop();
				Console.WriteLine("Iteration HashMap64: {0}", sss.ElapsedMilliseconds);

				

				sss = Stopwatch.StartNew();
				TestIterKeysDict(d);
				sss.Stop();
				Console.WriteLine("Iteration keys Dictionary: {0}", sss.ElapsedMilliseconds);

				sss = Stopwatch.StartNew();
				TestIterKeysIHM(l);
				sss.Stop();
				Console.WriteLine("Iteration keys HashMap64: {0}", sss.ElapsedMilliseconds);

				

				sss = Stopwatch.StartNew();
				TestIterValDict(d);
				sss.Stop();
				Console.WriteLine("Iteration values Dictionary: {0}", sss.ElapsedMilliseconds);

				sss = Stopwatch.StartNew();
				TestIterValIHM(l);
				sss.Stop();
				Console.WriteLine("Iteration values HashMap64: {0}", sss.ElapsedMilliseconds);

				
			}
		}


		private static void TestIterValIHM(HashMap64<long> l)
		{
			long num;
			foreach (var item in l.Values)
			{
				num = item;
			}
		}

		private static void TestIterValDict(Dictionary<long, long> d)
		{
			long num;
			foreach (var item in d.Values)
			{
				num = item;
			}
		}

		

		private static void TestIterKeysIHM(HashMap64<long> l)
		{
			long num;
			foreach (var item in l.Keys)
			{
				num = item;
			}
		}

		private static void TestIterKeysDict(Dictionary<long, long> d)
		{
			long num;
			foreach (var item in d.Keys)
			{
				num = item;
			}
		}

		

		private static void TestIterIHM(HashMap64<long> l)
		{
			long num;
			foreach (var item in l)
			{
				num = item.Value;
			}
		}

		private static void TestIterDict(Dictionary<long, long> d)
		{
			long num;
			foreach (var item in d)
			{
				num = item.Value;
			}
		}

		

		private static void TestReadIHM(HashMap64<long> l)
		{
			for (int i = 0; i < count; i++)
			{
				s += l[nn[i]];
			}
		}

		private static void TestReadDict(Dictionary<long, long> d)
		{
			for (int i = 0; i < count; i++)
			{
				s += d[nn[i]];
			}
		}

		

		private static void TestInsetIHM(HashMap64<long> l)
		{
			for (int i = 0; i < count; i++)
			{
				l.Add(i + 1, i);
			}
		}

		private static void TestInsertDict(Dictionary<long, long> d)
		{
			for (int i = 0; i < count; i++)
			{
				d.Add(i + 1, i);
			}
		}

		

		private static void TestCreateInsetIHM(int elemCount)
		{
			for (int j = 0; j < creationCount; j++)
			{
				HashMap64<long> l = new HashMap64<long>(elemCount);
				for (int i = 0; i < elemCount; i++)
				{
					l.Add(i + 1, i);
				}
			}
		}

		private static void TestCreateInsertDict(int elemCount)
		{
			for (int j = 0; j < creationCount; j++)
			{
				Dictionary<long, long> d = new Dictionary<long, long>(elemCount);
				for (int i = 0; i < elemCount; i++)
				{
					d.Add(i + 1, i);
				}
			}
		}
	}
}
