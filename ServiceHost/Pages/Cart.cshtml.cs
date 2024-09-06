using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using ShopManagement.Configuration.Order;

namespace ServiceHost.Pages;

public class CartModel : PageModel
{
    public List<CartItem> CartItems;
    public const string CookieName = "cart-items";

    public void OnGet()
    {
        var serializer = new JavaScriptSerializer();
        var value = Request.Cookies[CookieName];
        CartItems = serializer
            .Deserialize<List<CartItem>>(value); // convert value to List<CartItem> (list of cart items)

        foreach (var cartItem in CartItems)
        {
            cartItem.TotalItemPrice = cartItem.UnitPrice * cartItem.Count;
        }
    }

    public IActionResult OnGetRemoveFromCart(long id)
    {
        var serializer = new JavaScriptSerializer();
        var value = Request.Cookies[CookieName];
        Response.Cookies.Delete(CookieName);
        var cartItems = serializer.Deserialize<List<CartItem>>(value);
        var itemToRemove = cartItems.FirstOrDefault(x => x.Id == id);
        cartItems.Remove(itemToRemove);
        var options = new CookieOptions {Expires = DateTime.Now.AddDays(2)};
        Response.Cookies.Append(CookieName, serializer.Serialize(cartItems), options);
        return RedirectToPage("/Cart");
    }
}