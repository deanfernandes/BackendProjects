namespace ShoppingCart;

class Program {
    static void Main()
    {
        var shoppingCart = new ShoppingCart();

        var cartItemBuilder = new CartItem.Builder();
        cartItemBuilder.SetName("t shirt");
        cartItemBuilder.SetColor("white");
        cartItemBuilder.SetPrice(5.99m);
        cartItemBuilder.SetQuantity(2);
        var cartItem = cartItemBuilder.Build();
        shoppingCart.AddItem(cartItem);

        var cartItem2 = new CartItem.Builder()
            .SetName("jacket")
            .SetColor("black")
            .SetPrice(70m)
            .SetQuantity(1)
            .Build();
        shoppingCart.AddItem(cartItem2);

        var cartItem3 = new CartItem.Builder()
            .SetName("cotton socks")
            .SetColor("black")
            .SetPrice(1.99m)
            .SetQuantity(5)
            .Build();
        shoppingCart.AddItem(cartItem3);

        shoppingCart.RemoveItem(cartItem);
        shoppingCart.UndoLastRemoveItem();

        //shoppingCart.PrintCart();

        //shoppingCart.SortByName();
        shoppingCart.SortByPrice(false);
        shoppingCart.PrintCart();
    }
}