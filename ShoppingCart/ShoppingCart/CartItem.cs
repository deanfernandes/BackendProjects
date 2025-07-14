namespace ShoppingCart
{
    public class CartItem
    {
        public string Name { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }

        public string Color { get; private set; }

        private CartItem() { }

        public class Builder {
            private readonly CartItem _item = new();

            public Builder SetName(string name)
            {
                this._item.Name = name;
                return this;
            }

            public Builder SetQuantity(int quantity)
            {
                _item.Quantity = quantity;
                return this;
            }

            public Builder SetPrice(decimal price)
            {
                _item.Price = price;
                return this;
            }

            public Builder SetColor(string color)
            {
                _item.Color = color;
                return this;
            }

            public CartItem Build()
            {
                return this._item;
            }
        }

        public override string ToString()
        {
            return $"{Color} {Name,-20} ${Price,-6} x{Quantity}";
        }
    }
}
