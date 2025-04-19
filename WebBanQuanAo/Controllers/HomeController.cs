using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanQuanAo.Models;

namespace WebBanQuanAo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index(bool? isNew, bool? isTrend)
        {
            var categories = await _context.Categories.ToListAsync();

            if (categories == null)
            {
                return View("Error");
            }

            var products = _context.Products.Include(p => p.Category).AsQueryable();

            // Filter by IsNew
            if (isNew.HasValue)
            {
                products = products.Where(p => p.IsNew == isNew.Value);
            }

            // Filter by IsTrend
            if (isTrend.HasValue)
            {
                products = products.Where(p => p.IsTrend == isTrend.Value);
            }

            var productList = await products.ToListAsync();

            if (productList == null)
            {
                return View("Error");
            }

            ViewBag.Categories = categories;
            return View(productList);
        }


        // Hiển thị sản phẩm theo danh mục
        public async Task<IActionResult> ProductsByCategory(int? categoryId)
        {
            if (categoryId == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category)
                .ToListAsync();

            // Kiểm tra nếu sản phẩm là null
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // Chi tiết sản phẩm
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var cartItem = cart.FirstOrDefault(c => c.Product.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
                if (product != null)
                {
                    cart.Add(new CartItem { Product = product, Quantity = quantity });
                }
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return RedirectToAction("Index");
        }
    }
}
