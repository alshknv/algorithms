using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System;

namespace _1_phone_book
{
    public class PhoneHT
    {
        private LinkedList<KeyValuePair<int, string>>[] table;
        private int hashA;
        private int hashB;
        private int prime;
        private int records;
        private readonly Random random = new Random();

        private int GetPrime(int start)
        {
            var num = start + 1;
            while (true)
            {
                var factors = Math.Sqrt(num);
                var factorFound = false;
                for (var factor = 2; factor <= factors; factor++)
                {
                    if (num % factor == 0)
                    {
                        factorFound = true;
                        break;
                    }
                }
                if (!factorFound) return num;
                num++;
            }
        }

        private void RehashTable()
        {
            var newTable = new LinkedList<KeyValuePair<int, string>>[table.Length * 2];
            var newPrime = GetPrime(newTable.Length);
            var newHashA = random.Next(newPrime - 1) + 1;
            var newHashB = random.Next(newPrime);
            for (int i = 0; i < table.Length; i++)
            {
                if (table[i] == null) continue;
                var node = table[i].First;
                while (node != null)
                {
                    var nodeKey = node.Value.Key;
                    var nodeValue = node.Value.Value;
                    var newHash = Hash(nodeKey, newPrime, newHashA, newHashB, newTable.Length);
                    if (newTable[newHash] == null)
                    {
                        newTable[newHash] = new LinkedList<KeyValuePair<int, string>>();
                        newTable[newHash].AddFirst(new KeyValuePair<int, string>(nodeKey, nodeValue));
                    }
                    else
                    {
                        newTable[newHash].AddLast(new KeyValuePair<int, string>(nodeKey, nodeValue));
                    }
                    node = node.Next;
                }
            }
            table = newTable;
            prime = newPrime;
            hashA = newHashA;
            hashB = newHashB;
        }

        private int Hash(int value, int p, int a, int b, int m)
        {
            return (int)(((long)value * a + b) % p % m);
        }

        private int Hash(int value)
        {
            return Hash(value, prime, hashA, hashB, table.Length);
        }

        private LinkedListNode<KeyValuePair<int, string>> FindByKey(LinkedList<KeyValuePair<int, string>> list, int key)
        {
            var item = list.First;
            while (item != null)
            {
                if (item.Value.Key == key) return item;
                item = item.Next;
            }
            return null;
        }

        public PhoneHT(int initialSize)
        {
            table = new LinkedList<KeyValuePair<int, string>>[initialSize];
            prime = GetPrime(initialSize);
            records = 0;
            hashA = random.Next(prime - 1) + 1;
            hashB = random.Next(prime);
        }

        public void Add(int key, string value)
        {
            var tableItem = new KeyValuePair<int, string>(key, value);
            var keyHash = Hash(key);
            if (table[keyHash] == null)
            {
                table[keyHash] = new LinkedList<KeyValuePair<int, string>>();
                table[keyHash].AddFirst(tableItem);
                records++;
                if ((double)records / table.Length >= 0.7)
                {
                    RehashTable();
                }
            }
            else
            {
                var node = FindByKey(table[keyHash], tableItem.Key);
                if (node != null)
                {
                    node.Value = tableItem;
                }
                else
                {
                    table[keyHash].AddLast(tableItem);
                }
            }
        }

        public string Find(int key)
        {
            var keyHash = Hash(key);
            if (table[keyHash] == null)
            {
                return "not found";
            }
            else
            {
                var node = FindByKey(table[keyHash], key);
                if (node != null)
                {
                    return node.Value.Value;
                }
                else
                {
                    return "not found";
                }
            }
        }

        public void Delete(int key)
        {
            var keyHash = Hash(key);
            if (table[keyHash] != null)
            {
                var node = FindByKey(table[keyHash], key);
                if (node != null)
                {
                    table[keyHash].Remove(node);
                    if (table[keyHash].Count == 0)
                    {
                        table[keyHash] = null;
                    }
                }
            }
        }
    }

    public static class PhoneBook
    {
        public static string[] Solve(string[] queries)
        {
            var output = new List<string>();
            var ht = new PhoneHT(200000);
            foreach (var query in queries)
            {
                var queryParts = query.Split(' ');
                switch (queryParts[0])
                {
                    case "add":
                        ht.Add(int.Parse(queryParts[1]), queryParts[2]);
                        break;
                    case "del":
                        ht.Delete(int.Parse(queryParts[1]));
                        break;
                    case "find":
                        output.Add(ht.Find(int.Parse(queryParts[1])));
                        break;
                }
            }
            return output.ToArray();
        }

        static void Main(string[] args)
        {
            var numOfQueries = int.Parse(Console.ReadLine());
            var queries = new string[numOfQueries];
            for (int i = 0; i < numOfQueries; i++)
            {
                queries[i] = Console.ReadLine();
            }
            foreach (var line in Solve(queries))
            {
                Console.WriteLine(line);
            }
        }
    }
}