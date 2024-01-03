namespace SnakeGame
{
    internal class Node<T>
    {
        private T data;
        private Node<T> next;

        public T Data { get { return data; } set { data = value; } }
        public Node<T> Next { get { return next; } set { next = value; } }

        public Node(T data)
        {
          this.data = data;
          next = null;
        }
    }
}
