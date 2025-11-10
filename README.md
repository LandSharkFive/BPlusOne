# B+ Tree

A B+ Tree is a tree where each node contains a list of child nodes.  Each leaf contains an list of integers. All data is saved in the leaves. All operations are done in logarithmic time. Building a N-Tree takes linear time. The best size for the list is between 50 and 100 elements. A B+ Tree can used with external storage. A B+ Tree can be used as a dictionary. A B+ Tree is a sorted list. A B+ Tree is balanced.

## Rules

1. All leaves are at the same level.
2. The root has at least two children.
3. Each node except root can have a maximum of m children and at least m/2 children.
4. Each node can contain a maximum of m - 1 keys and a minimum of Ceiling(m/2) - 1 keys.

## Install and Build

The is a C# Console-Mode Project.  Use Visual Studio 2022 and above to compile.  

## Performance

Performance is good. 

| Items | Time | Memory | Height |
| --- | --- | --- | --- |
| 1K | 8 ms | 23 mb | 3 |
| 10K | 25 ms | 30 mb | 3 |
| 100K | 146 ms | 40 mb | 4 |
| 1M | 1500 ms | 108 mb | 5 |

## Exercise

Linking the leaf nodes together is deliberately not included.  This is left as an exercise.  

## References

1. Shashikant Kadam (2020), B+ Tree implementation using C++, GitHub, https://github.com/shashikdm/B-Plus-Tree
2. Introduction to Algorithms, Third Edition, Thomas M. Cormen, Charles E. Leiserson, Ronald Rivest, Clifford Stein, MIT Press, 2009.



