using System.Collections.Generic;

namespace ShoppingCart
{
    public class ShoppingCart
    {
        private List<CartItem> _items = new();
        private Stack<CartItem> _undoRemoveItems = new();

        public void AddItem(CartItem item)
        {
            _items.Add(item);
        }

        public void RemoveItem(CartItem item)
        {
            _items.Remove(item);
            _undoRemoveItems.Push(item);
        }

        public void ClearCart()
        {
            _items.Clear();
        }

        public void PrintCart()
        {
            Console.WriteLine("----------Shopping Cart----------");
            foreach (var item in _items)
            {
                Console.WriteLine($"- {item}");
            }
            Console.WriteLine($"Total: ${this.CalculateTotal()}");
        }

        private decimal CalculateTotal()
        {
            decimal total = 0m;
            foreach (var item in _items)
            {
                total += item.Price * item.Quantity;
            }
            return total;
        }

        public void UndoLastRemoveItem()
        {
            this.AddItem(_undoRemoveItems.Pop());
        }
    }
}
