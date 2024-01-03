namespace SnakeGame
{
    internal class SimpleLinkedList<T>
    {
        private Node<T> head;
        private int size = 0;

        public Node<T> Head {  get { return head; }}
        public int Size { get { return size; } }

        public T First { get {return head.Data; } }
        public T Last {
            get {
                Node<T> iterator = head;
                while (iterator.Next != null)
                {
                    iterator = iterator.Next;
                }
                return iterator.Data; 
            } 
        }

        public SimpleLinkedList()
        {
            head = null;
        }

        public SimpleLinkedList(T data)
        {
            head = new Node<T>(data);
            size = 1;
        }

        public void AddLast(T data)
        {
            if (head == null)
            {
                head = new Node<T>(data);
                size = 1;
            }
            else
            {
                Node<T> iterator = head;
                while(iterator.Next != null)
                {
                    iterator=iterator.Next;
                }
                Node<T> newNode = new Node<T>(data);
                iterator.Next = newNode;
                size++;
            }
        }


        public void AddFirst(T data)
        {
            if (head == null)
            {
                head = new Node<T>(data);
                size = 1;
            }
            else
            {
                Node<T> newNode = new Node<T>(data);
                newNode.Next = head;
                head = newNode;
                size++;
            }
        }

        private Node<T> Search(T data)
        {
            Node<T> iterator = head;
            while (iterator != null)
            {
               if(iterator.Data.Equals(data))
                {
                    return iterator;
                }

               iterator = iterator.Next;
            }
            return null;
        }

        public bool Remove(T data)
        {
            Node<T> nodeToRemove = Search(data);
            if(nodeToRemove == null)
                return false;
            else if(nodeToRemove == head)
            {
                head = head.Next;
                size--;
                return true;
            }
            else
            {
                Node<T> iterator = head;
                while (iterator.Next != nodeToRemove)
                {
                    iterator = iterator.Next;
                }
                iterator.Next = nodeToRemove.Next;
                size--;
                return true;
            }
        }

        public bool RemoveLast()
        {
            if (head == null)
                return false;
            else if (head.Next == null)
            {
                head = null;
                size--;
                return true;
            }
            else
            {
                Node<T> iterator = head;
                while (iterator.Next.Next != null)
                {
                    iterator = iterator.Next;
                }
                iterator.Next = null;
                size--;
                return true;
            }
        }

        public bool Update(int index, T data) 
        {
            if (index >= size || index < 0) 
            {
                return false;
            }
            Node<T> iterator = head;
            int i = 0;
            while (iterator != null)
            {
                if (i == index)
                {
                    iterator.Data = data;
                    break;
                }
                iterator = iterator.Next;
                i++;
            }
            return true; 
        }
        
        public bool DetectLoop()
        {
            if (size <= 1) 
                return false;
            Node<T> slow = head;
            Node<T> fast = head;

            slow = slow.Next;
            fast = fast.Next.Next;

            while (fast != null && fast.Next != null)
            {
                if (slow == fast)
                {
                    return true;
                }
                slow = slow.Next;
                fast = fast.Next.Next;
            }
            return false;
        }

        public void Reverse()
        {
            Node<T> previous = null, current = head, next = null;
            while (current != null)
            {
                next = current.Next;
                current.Next = previous;
                previous = current;
                current = next;
            }
            head = previous;
        }

        public int FindLength()
        {
            Node<T> iterator = head;
            int count = 0;
            while (iterator != null)
            {
                count++;
                iterator = iterator.Next;
            }
            return count;
        }

        public T Get(int index)
        {
            Node<T> iterator = head;
            for(int i = 0; i < size; i++) 
            {
                if (i == index)
                    return iterator.Data;
                iterator = iterator.Next;
            }
            return default;
        }

        public override string ToString()
        {
            Node<T> iterator = head;
            string s = "";
            while(iterator != null)
            {
                s += iterator.Data.ToString() + ", ";
                iterator=iterator.Next;
            }
            return s;
        }
    }
}
