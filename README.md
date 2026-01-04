# B+ Tree

An efficient and functional C# implementation of the B+ Tree. All data is pushed to the leaves; all operations are executed in logarithmic time. This structure is balanced, sorted, and built for high-density external storage.

### The Rules
The integrity of the tree is maintained by strict structural constraints:
* **Uniformity:** All leaves reside at the same level.
* **The Root:** Maintains at least two children.
* **Branching Factor ($m$):** * Each node (except root) holds between $m/2$ and $m$ children.
    * Each node contains between $\lceil m/2 \rceil - 1$ and $m - 1$ keys.
    * **Optimization:** Recommended list size is **50–100 elements**.

### Performance
*Building an N-Tree takes linear time. Operational scaling is as follows:*

| Items | Time | Memory | Height |
| :--- | :--- | :--- | :--- |
| 1K | 8 ms | 20 MB | 3 |
| 10K | 25 ms | 30 MB | 3 |
| 100K | 140 ms | 40 MB | 4 |
| 1M | 1500 ms | 110 MB | 5 |

### Build Requirements
* **Environment:** C# Console-Mode
* **Compiler:** Visual Studio 2022+

### The Exercise
Linking the leaf nodes together is **deliberately excluded**. If you require sequential traversal, the implementation is left to you.

---

### References
* **Introduction to Algorithms** (Cormen, Leiserson, Rivest, Stein), MIT Press.
* **Shashikant Kadam**, [B+ Tree implementation (C++)](https://github.com/shashikdm/B-Plus-Tree).
