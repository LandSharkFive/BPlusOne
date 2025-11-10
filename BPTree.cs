
namespace BPlusOne
{
    public class BPTree
    {
        public Node Root;

        private const int MaxDegree = 40;

        public BPTree()
        {
            Root = null;
        }

        public int GetHeight()
        {
            if (Root == null)
                return 0;
            return Node.GetHeight(Root);
        }

        public int GetNodeCount()
        {
            if (Root == null)
                return 0;
            return Node.GetNodeCount(Root);
        }

        public List<int> GetData()
        {
            if (Root == null)
            {
                return new List<int>();
            }
            return Node.GetData(Root);
        }


        /// <summary>
        /// Does key x exist in the tree?
        /// </summary>
        /// <param name="x">int</param>
        /// <returns>bool</returns>
        public bool Exist(int x)
        {
            if (Root == null)
            {
                return false;
            }

            Node cursor = Root;
            while (!cursor.IsLeaf)
            {
                bool found = false;
                for (int i = 0; i < cursor.Size; i++)
                {
                    if (x < cursor.Key[i])
                    {
                        found = true;
                        cursor = cursor.Child[i];
                        break;
                    }
                }

                if (!found)
                {
                    // If loop finishes, then use the last child.
                    cursor = cursor.Child[cursor.Size];
                }
            }

            // Search in the leaf node
            return cursor.Key.Take(cursor.Size).Contains(x);
        }

        // --- Insert Implementation (Refactored to use Array.Copy) ---

        /// <summary>
        /// Insert key x into the tree.
        /// </summary>
        /// <param name="x">int</param>
        public void Insert(int x)
        {
            if (Root == null)
            {
                Root = new Node();
                Root.Key[0] = x;
                Root.IsLeaf = true;
                Root.Size = 1;
                return;
            }

            Node cursor = Root;
            Node parent = null;

            // Traverse to the leaf node
            while (!cursor.IsLeaf)
            {
                parent = cursor;
                bool found = false;
                for (int i = 0; i < cursor.Size; i++)
                {
                    if (x < cursor.Key[i])
                    {
                        found = true;
                        cursor = cursor.Child[i];
                        break;
                    }
                }

                if (!found)
                {
                    // If loop finishes, then use the last child.
                    cursor = cursor.Child[cursor.Size];
                }
            }

            if (cursor.Size < MaxDegree)
            {
                // Not full: Simple insert with space shifting
                int i = 0;
                while (x > cursor.Key[i] && i < cursor.Size) i++;

                // C# idiomatic shift: Array.Copy (Replaces manual for loop)
                Array.Copy(cursor.Key, i, cursor.Key, i + 1, cursor.Size - i);

                cursor.Key[i] = x;
                cursor.Size++;
                // Update leaf linkage pointer
                cursor.Child[cursor.Size] = cursor.Child[cursor.Size - 1];
                cursor.Child[cursor.Size - 1] = null;
            }
            else
            {
                // Overflow: Split logic
                Node newLeaf = new Node();
                newLeaf.IsLeaf = true;
                // Virtual node/key array to hold all Key including the new one
                int[] virtualNode = new int[MaxDegree + 1];
                Array.Copy(cursor.Key, virtualNode, MaxDegree);

                int i = 0;
                while (x > virtualNode[i] && i < MaxDegree) i++;

                // Shift for new key
                Array.Copy(virtualNode, i, virtualNode, i + 1, MaxDegree - i);
                virtualNode[i] = x;

                // Split point calculation
                cursor.Size = (MaxDegree + 1) / 2;
                newLeaf.Size = MaxDegree + 1 - cursor.Size;

                // Update leaf linkage pointers
                newLeaf.Child[newLeaf.Size] = cursor.Child[MaxDegree];
                cursor.Child[MaxDegree] = null;
                cursor.Child[cursor.Size] = newLeaf; // Link cursor to newLeaf

                // Redistribute Key.
                Array.Copy(virtualNode, 0, cursor.Key, 0, cursor.Size);
                Array.Copy(virtualNode, cursor.Size, newLeaf.Key, 0, newLeaf.Size);

                // Modify the parent.
                if (cursor == Root)
                {
                    Node newRoot = new Node()
                    {
                        Key = { [0] = newLeaf.Key[0] },
                        Child = { [0] = cursor, [1] = newLeaf },
                        Size = 1
                    };
                    Root = newRoot;
                }
                else
                {
                    InsertInternal(newLeaf.Key[0], parent, newLeaf);
                }
            }
        }


        /// <summary>
        /// Insert key x into internal node.
        /// The child is the new leaf.
        /// </summary>
        /// <param name="x">int</param>
        /// <param name="cursor">Node</param>
        /// <param name="child">Node</param>
        private void InsertInternal(int x, Node cursor, Node child)
        {
            // 1. No Overflow: Simple Insertion
            if (cursor.Size < MaxDegree)
            {
                // Find the correct position for new key
                int i = 0;
                while (i < cursor.Size && x > cursor.Key[i])
                {
                    i++;
                }

                // Shift existing keys to the right (to make space at index i)
                // Array.Copy(sourceArray, sourceIndex, destinationArray, destinationIndex, length)
                Array.Copy(cursor.Key, i, cursor.Key, i + 1, cursor.Size - i);

                // Shift existing children to the right (to make space at index i + 1)
                Array.Copy(cursor.Child, i + 1, cursor.Child, i + 2, cursor.Size - i);

                cursor.Key[i] = x;
                cursor.Size++;
                cursor.Child[i + 1] = child;
            }
            else
            {
                // 2. Overflow: Node Split
                Node newInternal = new Node();
                int[] virtualKey = new int[MaxDegree + 1];
                Node[] virtualChild = new Node[MaxDegree + 2];

                // --- 2a. Copy existing elements to virtual arrays (Replaces two 'k' loops) ---
                Array.Copy(cursor.Key, 0, virtualKey, 0, MaxDegree);
                Array.Copy(cursor.Child, 0, virtualChild, 0, MaxDegree + 1);

                // --- 2b. Find insertion position 'i' ---
                int i = 0;
                while (i < MaxDegree && x > virtualKey[i])
                {
                    i++;
                }

                // --- 2c. Shift elements and insert the new key/child ---

                // Shift keys in virtualKey (to make space at index i)
                Array.Copy(virtualKey, i, virtualKey, i + 1, MaxDegree - i);
                virtualKey[i] = x;

                // Shift children in virtualChild (to make space at index i + 1)
                Array.Copy(virtualChild, i + 1, virtualChild, i + 2, MaxDegree + 1 - (i + 1));
                virtualChild[i + 1] = child;

                newInternal.IsLeaf = false;

                // --- 2d. Determine split point and new sizes ---
                cursor.Size = (MaxDegree + 1) / 2;
                newInternal.Size = MaxDegree - cursor.Size;

                // --- 2e. Distribute elements to the new node (Replaces two 'for' loops) ---
                int sourceKeyIndex = cursor.Size + 1;
                int sourceChildIndex = cursor.Size + 1;

                // Copy keys from virtualKey to newInternal Key.
                Array.Copy(virtualKey, sourceKeyIndex, newInternal.Key, 0, newInternal.Size);

                // Copy children from virtualChild to newInternal.Child
                Array.Copy(virtualChild, sourceChildIndex, newInternal.Child, 0, newInternal.Size + 1);

                int promotedKey = virtualKey[cursor.Size];

                // --- 2f. Handle Root update or Parent recursion ---
                if (cursor == Root)
                {
                    Node newRoot = new Node();
                    newRoot.Key[0] = promotedKey;
                    newRoot.Child[0] = cursor;
                    newRoot.Child[1] = newInternal;
                    newRoot.IsLeaf = false;
                    newRoot.Size = 1;
                    Root = newRoot;
                }
                else
                {
                    // Recursion: Insert promoted key into parent
                    InsertInternal(promotedKey, FindParent(Root, cursor), newInternal);
                }
            }
        }

        /// <summary>
        /// Find the parent of the child.
        /// The child is the node that you are looking for.
        /// The cursor is generally the root node, but it can be any
        /// internal node in the tree.
        /// </summary>
        /// <param name="cursor">Node</param>
        /// <param name="child">Node</param>
        /// <returns>Node</returns>
        private Node FindParent(Node cursor, Node child)
        {
            // Finds parent using depth first traversal and ignores leaf nodes as they cannot be parents.
            // Ignores second last level because we will never find parent of a leaf node during insertion using this function.
            Node parent = null;
            if (cursor.IsLeaf || cursor.Child[0].IsLeaf)
            {
                return null;
            }
            for (int i = 0; i < cursor.Size + 1; i++)
            {
                if (cursor.Child[i] == child)
                {
                    parent = cursor;
                    return parent;
                }
                else
                {
                    parent = FindParent(cursor.Child[i], child);
                    if (parent != null)
                    {
                        return parent;
                    }
                }
            }
            return parent;
        }


        /// <summary>
        /// Display the tree in depth first order.
        /// The root is first.
        /// </summary>
        public void Display()
        {
            if (Root == null)
            {
                return;
            }

            DisplayInner(Root);
        }


        /// <summary>
        /// Display the tree in depth first order.  Recursive.
        /// The root is first.
        /// </summary>
        /// <param name="cursor"></param>
        private void DisplayInner(Node cursor)
        {
            //depth first display
            if (cursor != null)
            {
                for (int i = 0; i < cursor.Size; i++)
                {
                    Console.Write(cursor.Key[i] + " ");
                }
                Console.WriteLine();
                if (cursor.IsLeaf == false)
                {
                    for (int i = 0; i < cursor.Size + 1; i++)
                    {
                        DisplayInner(cursor.Child[i]);
                    }
                }
            }
        }


        /// <summary>
        /// Remove x from the tree.
        /// </summary>
        /// <param name="x">int</param>
        public void Remove(int x)
        {
            // delete logic
            if (Root == null)
            {
                return;
            }

            Node cursor = Root;
            Node parent = null;
            int leftSibling = -1;
            int rightSibling = -1;

            // In the following while loop, cursor will travel to the leaf node possibly consisting the Key.
            while (cursor.IsLeaf == false)
            {
                for (int i = 0; i < cursor.Size; i++)
                {
                    parent = cursor;
                    leftSibling = i - 1; // leftSibling is the index of left sibling in the parent node
                    rightSibling = i + 1; // rightSibling is the index of right sibling in the parent node

                    if (x < cursor.Key[i])
                    {
                        cursor = cursor.Child[i];
                        break;
                    }

                    if (i == cursor.Size - 1)
                    {
                        leftSibling = i;
                        rightSibling = i + 2;
                        cursor = cursor.Child[i + 1];
                        break;
                    }
                }
            }

            // In the following for loop, we search for the Key if it exists.
            bool found = false;
            int pos = -1;
            for (pos = 0; pos < cursor.Size; pos++)
            {
                if (cursor.Key[pos] == x)
                {
                    found = true;
                    break;
                }
            }

            if (!found) // If Key does not exist in that leaf node.
            {
                return;
            }

            // Delete the Key from the leaf.
            for (int i = pos; i < cursor.Size - 1; i++)
            {
                cursor.Key[i] = cursor.Key[i + 1];
            }
            cursor.Size--;

            if (cursor == Root) // If this is Root node.
            {
                // Handle Root Case.
                // In C#, setting references to null is enough for garbage collection.
                for (int i = 0; i < MaxDegree + 1; i++)
                {
                    cursor.Child[i] = null;
                }

                if (cursor.Size == 0) // If all Keys are deleted.
                {
                    // C# GC will handle memory cleanup. 
                    Root = null;
                }
                return;
            }

            // In C#, assigning a reference to another is typically a safe copy, but setting
            // the Size index of 'Child' and then the next one to null is a good practice.
            cursor.Child[cursor.Size] = cursor.Child[cursor.Size + 1];
            cursor.Child[cursor.Size + 1] = null;

            // MaxDegree is assumed to be an integer type, so integer division is used for (MaxDegree+1)/2.
            if (cursor.Size >= (MaxDegree + 1) / 2)
            {
                // No underflow.
                return;
            }

            // Underflow condition.
            HandleUnderflow(cursor, parent, leftSibling, rightSibling);
        }

        /// <summary>
        /// Handle Underflow.
        /// </summary>
        /// <param name="cursor">Node</param>
        /// <param name="parent">Node</param>
        /// <param name="leftSibling">int</param>
        /// <param name="rightSibling">int</param>
        private void HandleUnderflow(Node cursor, Node parent, int leftSibling, int rightSibling)
        {
            // Underflow condition.
            // First we try to transfer a Key from sibling node.

            // Check if left sibling exists.
            if (leftSibling >= 0)
            {
                Node leftNode = parent.Child[leftSibling];
                // check if it is possible to transfer
                if (leftNode.Size >= (MaxDegree + 1) / 2 + 1)
                {
                    // make space for transfer
                    for (int i = cursor.Size; i > 0; i--)
                    {
                        cursor.Key[i] = cursor.Key[i - 1];
                    }

                    // shift pointer to next leaf (update the Size first)
                    cursor.Size++;
                    cursor.Child[cursor.Size] = cursor.Child[cursor.Size - 1];
                    cursor.Child[cursor.Size - 1] = null;

                    // transfer
                    cursor.Key[0] = leftNode.Key[leftNode.Size - 1];

                    // Shift pointer of leftsibling.
                    leftNode.Size--;
                    leftNode.Child[leftNode.Size] = cursor;
                    leftNode.Child[leftNode.Size + 1] = null;

                    // Update parent.
                    parent.Key[leftSibling] = cursor.Key[0];
                    return;
                }
            }

            if (rightSibling <= parent.Size) // check if right sibling exist
            {
                Node rightNode = parent.Child[rightSibling];
                // Check if it is possible to transfer.
                if (rightNode.Size >= (MaxDegree + 1) / 2 + 1)
                {
                    // Shift pointer to next leaf (update the Size first).
                    cursor.Size++;
                    cursor.Child[cursor.Size] = cursor.Child[cursor.Size - 1];
                    cursor.Child[cursor.Size - 1] = null;

                    // Transfer
                    cursor.Key[cursor.Size - 1] = rightNode.Key[0];

                    // Shift pointer of rightsibling.
                    rightNode.Size--;
                    rightNode.Child[rightNode.Size] = rightNode.Child[rightNode.Size + 1];
                    rightNode.Child[rightNode.Size + 1] = null;

                    // Shift content of right sibling.
                    for (int i = 0; i < rightNode.Size; i++)
                    {
                        rightNode.Key[i] = rightNode.Key[i + 1];
                    }

                    // Update parent.
                    parent.Key[rightSibling - 1] = rightNode.Key[0];
                    return;
                }
            }

            // Must merge and delete a node.
            if (leftSibling >= 0) // If left sibling exist.
            {
                Node leftNode = parent.Child[leftSibling];
                // Transfer all Keys to leftsibling and then transfer pointer to next leaf node.
                for (int i = leftNode.Size, j = 0; j < cursor.Size; i++, j++)
                {
                    leftNode.Key[i] = cursor.Key[j];
                }

                leftNode.Child[leftNode.Size] = null;
                leftNode.Size += cursor.Size;
                leftNode.Child[leftNode.Size] = cursor.Child[cursor.Size];

                RemoveInternal(parent.Key[leftSibling], parent, cursor); // delete parent node Key

                // In C#, assigning 'cursor' to null allows the Garbage Collector to clean it up.
                // The arrays 'Key' and 'Child' will be collected along with the 'cursor' object.
                // Optionally, to assist the GC (for clarity/large structures):
                // cursor.Key = null; 
                // cursor.Child = null;
                // cursor = null; 
                // This only sets the local reference to null, which is less impactful.
            }
            else if (rightSibling <= parent.Size) // If right sibling exist.
            {
                Node rightNode = parent.Child[rightSibling];
                // Transfer all Keys to cursor and then transfer pointer to next leaf node.
                for (int i = cursor.Size, j = 0; j < rightNode.Size; i++, j++)
                {
                    cursor.Key[i] = rightNode.Key[j];
                }

                cursor.Child[cursor.Size] = null;
                cursor.Size += rightNode.Size;
                cursor.Child[cursor.Size] = rightNode.Child[rightNode.Size];

                // Delete parent node Key.
                RemoveInternal(parent.Key[rightSibling - 1], parent, rightNode);
            }
        }

        /// <summary>
        /// Remove x from internal node.
        /// </summary>
        /// <param name="x">int</param>
        /// <param name="cursor">Node</param>
        /// <param name="child">Node</param>
        private void RemoveInternal(int x, Node cursor, Node child)
        {
            // --- 1. HANDLE Root COLLAPSE (Special Case) ---
            if (cursor == Root)
            {
                if (cursor.Size == 1)
                {
                    if (cursor.Child[1] == child)
                    {
                        // Root is replaced by the left child (ptr[0]).
                        Root = cursor.Child[0];
                        // GC handles cleanup for cursor and child.
                        return;
                    }
                    else if (cursor.Child[0] == child)
                    {
                        // Root is replaced by the right child (ptr[1]).
                        Root = cursor.Child[1];
                        // GC handles cleanup for cursor and child.
                        return;
                    }
                }
            }

            // --- 2. DELETE KEY X FROM CURSOR ---
            int pos;
            for (pos = 0; pos < cursor.Size; pos++)
            {
                if (cursor.Key[pos] == x)
                {
                    break;
                }
            }

            // Key Shift Left.
            for (int i = pos; i < cursor.Size; i++)
            {
                cursor.Key[i] = cursor.Key[i + 1];
            }

            // --- 3. DELETE CHILD POINTER FROM CURSOR ---
            for (pos = 0; pos < cursor.Size + 1; pos++)
            {
                if (cursor.Child[pos] == child)
                {
                    break;
                }
            }

            // Pointer Shift Left.
            for (int i = pos; i < cursor.Size + 1; i++)
            {
                cursor.Child[i] = cursor.Child[i + 1];
            }

            cursor.Size--;

            // --- 4. UNDERFLOW CHECK ---
            int minKeys = (MaxDegree + 1) / 2 - 1;
            if (cursor.Size >= minKeys)
            {
                return;
            }

            if (cursor == Root)
            {
                return;
            }

            // --- 5. FIND SIBLINGS (Requires FindParent() method) ---
            Node parent = FindParent(Root, cursor);
            if (parent == null) return;

            int leftSibling = -1;
            int rightSibling = -1;

            for (pos = 0; pos < parent.Size + 1; pos++)
            {
                if (parent.Child[pos] == cursor)
                {
                    leftSibling = pos - 1;
                    rightSibling = pos + 1;
                    break;
                }
            }

            // --- 6. TRY TRANSFER FROM SIBLINGS ---

            // A. Transfer from Left Sibling
            if (leftSibling >= 0)
            {
                Node leftNode = parent.Child[leftSibling];
                if (leftNode.Size >= minKeys + 1)
                {
                    // 1. Make space for transfer of key (Shift Right).
                    for (int i = cursor.Size; i > 0; i--)
                    {
                        cursor.Key[i] = cursor.Key[i - 1];
                    }

                    // 2. Transfer key from left sibling through parent.
                    cursor.Key[0] = parent.Key[leftSibling];
                    parent.Key[leftSibling] = leftNode.Key[leftNode.Size - 1];

                    // 3. Make space for transfer of ptr (Shift Right).
                    for (int i = cursor.Size + 1; i > 0; i--)
                    {
                        cursor.Child[i] = cursor.Child[i - 1];
                    }

                    // 4. Transfer last pointer from left node to cursor.
                    cursor.Child[0] = leftNode.Child[leftNode.Size];

                    cursor.Size++;
                    leftNode.Size--;

                    return;
                }
            }

            // B. Transfer from Right Sibling
            if (rightSibling <= parent.Size)
            {
                Node rightNode = parent.Child[rightSibling];
                if (rightNode.Size >= minKeys + 1)
                {
                    // 1. Transfer key from right sibling through parent
                    cursor.Key[cursor.Size] = parent.Key[pos];
                    parent.Key[pos] = rightNode.Key[0];

                    // 2. Shift Right Node keys Left 
                    for (int i = 0; i < rightNode.Size - 1; i++)
                    {
                        rightNode.Key[i] = rightNode.Key[i + 1];
                    }

                    // 3. Transfer first pointer from right node to cursor.
                    cursor.Child[cursor.Size + 1] = rightNode.Child[0];

                    // 4. Shift Right Node pointers Left 
                    for (int i = 0; i < rightNode.Size; ++i)
                    {
                        rightNode.Child[i] = rightNode.Child[i + 1];
                    }

                    cursor.Size++;
                    rightNode.Size--;

                    return;
                }
            }

            // --- 7. MERGE WITH SIBLINGS ---

            // C. Merge with Left Sibling (leftNode + parent key + cursor).
            if (leftSibling >= 0)
            {
                Node leftNode = parent.Child[leftSibling];

                // 1. Transfer parent key down.
                leftNode.Key[leftNode.Size] = parent.Key[leftSibling];

                // 2. Copy cursor keys to leftNode.
                for (int i = leftNode.Size + 1, j = 0; j < cursor.Size; i++, j++)
                {
                    leftNode.Key[i] = cursor.Key[j];
                }

                // 3. Copy cursor pointers to leftNode. 
                for (int i = leftNode.Size + 1, j = 0; j < cursor.Size + 1; i++, j++)
                {
                    leftNode.Child[i] = cursor.Child[j];
                    cursor.Child[j] = null; // Clear cursor's pointer references
                }

                leftNode.Size += cursor.Size + 1; // +1 for the parent key
                cursor.Size = 0;

                RemoveInternal(parent.Key[leftSibling], parent, cursor);
            }
            // D. Merge with Right Sibling (cursor + parent key + rightNode).
            else if (rightSibling <= parent.Size)
            {
                Node rightNode = parent.Child[rightSibling];

                // 1. Transfer parent key down.
                cursor.Key[cursor.Size] = parent.Key[rightSibling - 1];

                // 2. Copy rightNode keys to cursor. 
                for (int i = cursor.Size + 1, j = 0; j < rightNode.Size; i++, j++)
                {
                    cursor.Key[i] = rightNode.Key[j];
                }

                // 3. Copy rightNode pointers to cursor. 
                for (int i = cursor.Size + 1, j = 0; j < rightNode.Size + 1; i++, j++)
                {
                    cursor.Child[i] = rightNode.Child[j];
                    rightNode.Child[j] = null; // Clear rightNode's pointer references.
                }

                cursor.Size += rightNode.Size + 1; // +1 for the parent key
                rightNode.Size = 0;

                RemoveInternal(parent.Key[rightSibling - 1], parent, rightNode);
            }
        }

        public void ReadFile(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    int a = 0;
                    if (int.TryParse(line, out a))
                    {
                        Insert(a);
                    }
                }
            }
        }


        public void WriteToFile(string fileName)
        {
            if (Root == null)
            {
                return;
            }

            using (StreamWriter sw = new StreamWriter(fileName, false))
            {
                Node.WriteToStream(Root, sw);
            }
        }

        public void Clear()
        {
            if (Root == null)
            {
                return;
            }

            Root.Clear(Root);
            Root = null;
        }


    }
}
