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

        /// <summary>
        /// Sort cart items by name using Bubble Sort algorithm
        /// </summary>
        /// <param name="ascending"></param>
        public void SortByName(bool ascending = true)
        {
            for (int i = 0; i < _items.Count - 1; i++)
            {
                for (int j = 0; j < _items.Count - i - 1; j++)
                {
                    int comparison = string.Compare(_items[j].Name, _items[j + 1].Name, StringComparison.OrdinalIgnoreCase);

                    if ((ascending && comparison > 0) || (!ascending && comparison < 0))
                    {
                        var temp = _items[j];
                        _items[j] = _items[j + 1];
                        _items[j + 1] = temp;
                    }
                }
            }

            Console.WriteLine($"Sorted cart by name ({(ascending ? "A–Z" : "Z–A")}).");
        }

        /// <summary>
        /// Sort cart items by price using Selection Sort algorithm
        /// </summary>
        /// <param name="ascending"></param>
        public void SortByPrice(bool ascending = true)
        {
            int n = _items.Count;

            for (int i = 0; i < n - 1; i++)
            {
                int selectedIndex = i;

                for (int j = i + 1; j < n; j++)
                {
                    bool shouldSelect = ascending
                        ? _items[j].Price < _items[selectedIndex].Price
                        : _items[j].Price > _items[selectedIndex].Price;

                    if (shouldSelect)
                    {
                        selectedIndex = j;
                    }
                }

                // Swap
                if (selectedIndex != i)
                {
                    var temp = _items[i];
                    _items[i] = _items[selectedIndex];
                    _items[selectedIndex] = temp;
                }
            }

            Console.WriteLine($"Sorted cart by price ({(ascending ? "Low → High" : "High → Low")}).");
        }
    }
}
