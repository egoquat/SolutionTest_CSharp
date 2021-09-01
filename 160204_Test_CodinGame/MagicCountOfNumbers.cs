//https://www.codingame.com/ide/puzzle/magic-count-of-numbers
// 100%


using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
public static class Common
{
	public static bool AddNotOverlap<T>(this List<T> list, T item)
	{
		if (list.Any(i => { return i.Equals(item); }))
			return false;

		list.Add(item);
		return true;
	}

	public static string String<T>(this List<T> list)
	{
		string output = "(" + list.Count + ")";
		for (int i = 0; i < list.Count; ++i)
		{
			output += " " + list[i].ToString();
		}
		return output;
	}
}

class Solution
{
	static long GetMultiply(List<long> subs)
	{
		long sum = 1;
		for (int i = 0; i < subs.Count; ++i)
		{
			sum *= subs[i];
		}
		return sum;
	}

	static void GetSubsetsR(List<long> primes, int index, List<long> subset, ref List<List<long>> subsets)
	{
		if (subset.Count >= 2)
		{
			subsets.Add(subset.ToList());
			//Console.Error.WriteLine("subsets.Add:" + index + "/" + subset.String());
		}

		for (int i = index; i < primes.Count; i++)
		{
			subset.Add(primes[i]);
			GetSubsetsR(primes, i + 1, subset, ref subsets);
			subset.RemoveAt(subset.Count - 1);
		}

		return;
	}

	static List<List<long>> GetSubsets(List<long> primes)
	{
		List<long> subset = new List<long>();
		List<List<long>> subsets = new List<List<long>>();

		int index = 0;
		GetSubsetsR(primes, index, subset, ref subsets);

		return subsets;
	}

	static void GetCollectSubs(List<long> primes, ref List<long> subtracts, ref List<long> additions)
	{
		List<List<long>> subsets = GetSubsets(primes);
		for (int i = 0; i < subsets.Count; ++i)
		{
			List<long> subset = subsets[i];
			long multiplied = GetMultiply(subset);
			bool isSubtract = subset.Count % 2 == 0;
			if (true == isSubtract)
			{
				subtracts.Add(multiplied);
			}
			else
			{
				additions.Add(multiplied);
			}
			//Console.Error.WriteLine("subset" + i + "/multiplied:" + multiplied + "/subset:" + subset.String() + "/" + ((true == isSubtract) ? "Minus" : "Plus"));
		}
	}

	static void Main(string[] args)
	{
		string[] inputs;
		inputs = Console.ReadLine().Split(' ');
		long n = long.Parse(inputs[0]);
		long k = long.Parse(inputs[1]);

		Console.Error.WriteLine("input n:" + inputs[0] + "/k:" + inputs[1]);
		inputs = Console.ReadLine().Split(' ');

		List<long> primes = new List<long>();
		for (int i = 0; i < k; i++)
		{
			long prime = long.Parse(inputs[i]);
			primes.Add(prime);
		}

		primes.Sort((a, b) =>
		{
			if (a > b) return 1;
			if (a < b) return -1;
			return 0;
		});

		string output = "";
		for (int i = 0; i < k; i++)
		{
			output += primes[i] + " ";
		}
		Console.Error.WriteLine("input prime:" + output);

		long countCD = 0;
		foreach (long p in primes)
		{
			countCD += n / p;
		}

		Console.Error.WriteLine("countCD 01:" + countCD);

		List<long> subtracts = new List<long>();
		List<long> additions = new List<long>();
		GetCollectSubs(primes, ref subtracts, ref additions);

		Console.Error.WriteLine("count subtracts:" + subtracts.Count + "/additions:" + additions.Count);
		foreach (long i in subtracts)
		{
			long number = n / i;
			//Console.Error.WriteLine(" Subtract:" + number);
			countCD -= number;
		}

		foreach (long i in additions)
		{
			long number = n / i;
			//Console.Error.WriteLine(" Addition:" + number);
			countCD += number;
		}

		// Write an action using Console.WriteLine()
		// To debug: Console.Error.WriteLine("Debug messages...");

		Console.WriteLine(countCD);
	}

}