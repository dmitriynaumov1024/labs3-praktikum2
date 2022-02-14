using System;
using System.Collections.Generic;
using System.IO;

/*
    Input file format:
    n[0] n[1] n[2] ... n[k-1]
    
    Operations:
    1. Read numbers from file into a list
    2. In list, remove all odd numbers
    3. In list, repeat all numbers that start with 3
    4. In list, sort all non-prime numbers
    *. Display the list after each operation
*/
static class Program
{
    static void DisplayList(List<int> list)
    {
        Console.Write("List: ");
        foreach (int item in list) {
            Console.Write("{0} ", item);
        }
        Console.Write("\n");
    }

    static void InitList(List<int> list, string filename)
    {   
        foreach (string entry in File.ReadAllText(filename).Split(' ', '\n')) {
            int number = 0;
            if (int.TryParse(entry, out number)) {
                list.Add(number);
            }
        }
    }

    static void RemoveOdd(List<int> list)
    {
        Console.Write("Removing odd \n");
        list.RemoveAll(number => (number % 2 == 1));
    }

    static void Repeat3(List<int> list)
    {
        Console.Write("Repeating numbers that start with 3 \n");
        List<int> temporary = new List<int>();
        foreach (int item in list) {
            if (item.ToString()[0] == '3') {
                temporary.Add(item);
            }
        }
        list.AddRange(temporary);
    }

    // Items are moved if they match sortCondition predicate.
    // Otherwise their positions are preserved.
    static void SortIf(this List<int> list, Predicate<int> sortCondition)
    {
        Console.Write("Sorting \n");

        List<int> 
            trueCondition = new List<int>(), 
            falseCondition = new List<int>(),
            falseConditionIndex = new List<int>();

        int index = 0;
        foreach (int item in list) {
            if (sortCondition(item)) {
                trueCondition.Add(item);
            }
            else {
                falseCondition.Add(item);
                falseConditionIndex.Add(index);
            }
            index++;
        }

        list.Clear();
        trueCondition.Sort();

        IEnumerator<int> 
            trueConitionIter = trueCondition.GetEnumerator(),
            falseConditionIter = falseCondition.GetEnumerator(),
            falseConditionIndexIter = falseConditionIndex.GetEnumerator();

        while (falseConditionIndexIter.MoveNext()) {
            while (falseConditionIndexIter.Current > list.Count) {
                trueConitionIter.MoveNext();
                list.Add(trueConitionIter.Current);
            }
            falseConditionIter.MoveNext();
            list.Add(falseConditionIter.Current);
        }
        // Append trailing items 
        while (trueConitionIter.MoveNext()) {
            list.Add(trueConitionIter.Current);
        }
    }

    static bool IsPrime (int number) {
        bool result = true;
        if (number == 2) {
            result = true;
        } 
        else if (number <= 1 || number % 2 == 0) {
            result = false;
        }
        else {
            int upperBound = ((int)Math.Sqrt(number)) | 1;
            for (int i=upperBound; i>1; i-=2) {
                if (number % i == 0) {
                    result = false;
                    break;
                }
            }
        }
        // Console.Write("{0} is prime? {1} \n", number, result);
        return result;
    }

    static bool IsNonPrime (int number) {
        return !IsPrime(number);
    }

    static T Use<T>(this T obj, Action<T> action) where T: class 
    {
        action(obj);
        return obj;
    }

    static void Main(string[] args)
    {
        if (args.Length < 1) {
            Console.Write("Expected 1 command line parameter: <file_name>");
            return;
        }

        new List<int>()
            .Use(L => InitList(L, args[0]))
            .Use(DisplayList)
            .Use(RemoveOdd)
            .Use(DisplayList)
            .Use(Repeat3)
            .Use(DisplayList)
            .Use(L => L.SortIf(IsNonPrime))
            .Use(DisplayList);
    }
} 
