// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Data Management
const appState = {
    products: [
        {
            id: 1,
            name: "Bomba Hałasowa Pro",
            price: 49.99,
            category: "Hałasowe",
            rating: 5,
            description: "Profesjonalna bomba do głośnego świętowania"
        },
        {
            id: 2,
            name: "Bengalski Złoty",
            price: 29.99,
            category: "Świetlne",
            rating: 5,
            description: "Bengalski ze złotym blaskiem na 60 sekund"
        },
        {
            id: 3,
            name: "Zestaw Mieszany Deluxe",
            price: 149.99,
            category: "Zestawy",
            rating: 5,
            description: "Komplety zestaw hałasowy + świetlny"
        },
        {
            id: 4,
            name: "Petarda Sygnałowa",
            price: 19.99,
            category: "Hałasowe",
            rating: 4,
            description: "Mała petarda do sygnalizacji"
        }
    ],
    cart: [],
    currentProduct: null,
    filteredProducts: [],
    searchTerm: ''
};

// Initialize app
function initApp() {
    renderProducts();
    updateCartCount();
    setupEventListeners();
    appState.filteredProducts = [...appState.products];
}

// Event Listeners
function setupEventListeners() {
    // Header sticky behavior
    let lastScroll = 0;
    window.addEventListener('scroll', () => {
        const header = document.getElementById('mainHeader');
        const currentScroll = window.pageYOffset;

        if (currentScroll > lastScroll && currentScroll > 100) {
            header.style.transform = 'translateY(-100%)';
        } else {
            header.style.transform = 'translateY(0)';
        }
        lastScroll = currentScroll;
    });

    // Mobile menu toggle
    document.getElementById('mobileMenuBtn').addEventListener('click', () => {
        const nav = document.getElementById('mainNav');
        nav.classList.toggle('show');
    });

    // User dropdown
    document.getElementById('userBtn').addEventListener('click', () => {
        const dropdown = document.getElementById('userDropdown');
        dropdown.classList.toggle('show');
    });

    // Close dropdowns when clicking outside
    document.addEventListener('click', (e) => {
        if (!e.target.closest('.user-menu')) {
            document.getElementById('userDropdown').classList.remove('show');
        }
    });

    // Cart button
    document.getElementById('cartBtn').addEventListener('click', showCart);

    // Close cart modal
    document.getElementById('closeCart').addEventListener('click', () => {
        document.getElementById('cartModal').classList.remove('show');
    });

    // Continue shopping
    document.getElementById('continueShopping').addEventListener('click', () => {
        document.getElementById('cartModal').classList.remove('show');
    });

    // Proceed to checkout
    document.getElementById('proceedCheckout').addEventListener('click', () => {
        if (appState.cart.length === 0) {
            alert('Koszyk jest pusty!');
            return;
        }
        document.getElementById('cartModal').classList.remove('show');
        showCheckout();
    });

    // Close product modal
    document.getElementById('closeProduct').addEventListener('click', () => {
        document.getElementById('productModal').classList.remove('show');
    });

    // Search functionality
    document.getElementById('searchBtn').addEventListener('click', performSearch);
    document.getElementById('searchInput').addEventListener('keypress', (e) => {
        if (e.key === 'Enter') {
            performSearch();
        }
    });

    // Newsletter form
    document.getElementById('newsletterForm').addEventListener('submit', (e) => {
        e.preventDefault();
        alert('Dziękujemy za zapisanie się do newslettera!');
        e.target.reset();
    });

    // Close modals when clicking outside
    document.getElementById('cartModal').addEventListener('click', (e) => {
        if (e.target.id === 'cartModal') {
            e.target.classList.remove('show');
        }
    });

    document.getElementById('productModal').addEventListener('click', (e) => {
        if (e.target.id === 'productModal') {
            e.target.classList.remove('show');
        }
    });

    // Navigation links
    document.querySelectorAll('.nav-link').forEach(link => {
        link.addEventListener('click', (e) => {
            e.preventDefault();
            document.querySelectorAll('.nav-link').forEach(l => l.classList.remove('active'));
            link.classList.add('active');
            const target = link.getAttribute('href').substring(1);
            scrollToSection(target);
        });
    });
}

// Render products
function renderProducts(products = appState.products) {
    const grid = document.getElementById('productsGrid');
    grid.innerHTML = products.map(product => `
    <div class="product-card" onclick="showProductDetail(${product.id})">
      <div class="product-image">📦</div>
      <span class="product-badge">${product.category}</span>
      <h3 class="product-name">${product.name}</h3>
      <div class="product-rating">${'⭐'.repeat(product.rating)}</div>
      <p class="product-description">${product.description}</p>
      <p class="product-price">${product.price.toFixed(2)} PLN</p>
      <div class="product-actions">
        <button class="btn btn-primary" onclick="event.stopPropagation(); addToCart(${product.id})">Dodaj do koszyka</button>
        <button class="btn btn-outline" onclick="event.stopPropagation(); showProductDetail(${product.id})">Szczegóły</button>
      </div>
    </div>
  `).join('');
}

// Add to cart
function addToCart(productId) {
    const product = appState.products.find(p => p.id === productId);
    const existingItem = appState.cart.find(item => item.id === productId);

    if (existingItem) {
        existingItem.quantity++;
    } else {
        appState.cart.push({
            ...product,
            quantity: 1
        });
    }

    updateCartCount();
    showNotification('Produkt dodany do koszyka!');
}

// Update cart count
function updateCartCount() {
    const count = appState.cart.reduce((sum, item) => sum + item.quantity, 0);
    document.getElementById('cartCount').textContent = count;
}

// Show cart
function showCart() {
    const modal = document.getElementById('cartModal');
    const cartItems = document.getElementById('cartItems');

    if (appState.cart.length === 0) {
        cartItems.innerHTML = '<div class="empty-cart"><p>Koszyk jest pusty</p><p>🛒</p></div>';
    } else {
        cartItems.innerHTML = appState.cart.map(item => `
      <div class="cart-item">
        <div class="cart-item-image">📦</div>
        <div class="cart-item-info">
          <div class="cart-item-name">${item.name}</div>
          <div class="cart-item-price">${item.price.toFixed(2)} PLN</div>
        </div>
        <div class="cart-item-controls">
          <button class="qty-btn" onclick="updateQuantity(${item.id}, -1)">-</button>
          <span class="cart-item-qty">${item.quantity}</span>
          <button class="qty-btn" onclick="updateQuantity(${item.id}, 1)">+</button>
          <button class="remove-btn" onclick="removeFromCart(${item.id})">🗑️</button>
        </div>
      </div>
    `).join('');
    }

    updateCartSummary();
    modal.classList.add('show');
}

// Update quantity
function updateQuantity(productId, change) {
    const item = appState.cart.find(item => item.id === productId);
    if (item) {
        item.quantity += change;
        if (item.quantity <= 0) {
            removeFromCart(productId);
        } else {
            showCart();
            updateCartCount();
        }
    }
}

// Remove from cart
function removeFromCart(productId) {
    appState.cart = appState.cart.filter(item => item.id !== productId);
    showCart();
    updateCartCount();
}

// Update cart summary
function updateCartSummary() {
    const subtotal = appState.cart.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    const shipping = subtotal > 0 ? 15.00 : 0;
    const tax = subtotal * 0.23; // 23% VAT
    const total = subtotal + shipping + tax;

    document.getElementById('subtotal').textContent = `${subtotal.toFixed(2)} PLN`;
    document.getElementById('shipping').textContent = `${shipping.toFixed(2)} PLN`;
    document.getElementById('tax').textContent = `${tax.toFixed(2)} PLN`;
    document.getElementById('total').textContent = `${total.toFixed(2)} PLN`;
}

// Show product detail
function showProductDetail(productId) {
    const product = appState.products.find(p => p.id === productId);
    appState.currentProduct = product;

    document.getElementById('modalProductName').textContent = product.name;
    document.getElementById('modalRating').textContent = '⭐'.repeat(product.rating);
    document.getElementById('modalPrice').textContent = `${product.price.toFixed(2)} PLN`;
    document.getElementById('modalDescription').textContent = product.description;
    document.getElementById('modalQuantity').value = 1;

    document.getElementById('productModal').classList.add('show');
}

// Modal quantity controls
function increaseModalQty() {
    const input = document.getElementById('modalQuantity');
    input.value = parseInt(input.value) + 1;
}

function decreaseModalQty() {
    const input = document.getElementById('modalQuantity');
    if (parseInt(input.value) > 1) {
        input.value = parseInt(input.value) - 1;
    }
}

// Add to cart from modal
function addToCartFromModal() {
    if (!appState.currentProduct) return;

    const quantity = parseInt(document.getElementById('modalQuantity').value);
    const existingItem = appState.cart.find(item => item.id === appState.currentProduct.id);

    if (existingItem) {
        existingItem.quantity += quantity;
    } else {
        appState.cart.push({
            ...appState.currentProduct,
            quantity: quantity
        });
    }

    updateCartCount();
    document.getElementById('productModal').classList.remove('show');
    showNotification(`${quantity}x ${appState.currentProduct.name} dodano do koszyka!`);
}

// Filter by category
function filterByCategory(category) {
    const filtered = appState.products.filter(p => p.category === category);
    renderProducts(filtered);
    scrollToSection('produkty');
}

// Search functionality
function performSearch() {
    const searchTerm = document.getElementById('searchInput').value.toLowerCase();
    if (!searchTerm) return;

    appState.searchTerm = searchTerm;
    appState.filteredProducts = appState.products.filter(p =>
        p.name.toLowerCase().includes(searchTerm) ||
        p.description.toLowerCase().includes(searchTerm) ||
        p.category.toLowerCase().includes(searchTerm)
    );

    showSearchPage();
}

// Show search page
function showSearchPage() {
    document.getElementById('home').style.display = 'none';
    document.querySelector('.products-section').style.display = 'none';
    document.querySelector('.categories-section').style.display = 'none';
    document.querySelector('.testimonials-section').style.display = 'none';
    document.getElementById('checkoutPage').style.display = 'none';

    const searchPage = document.getElementById('searchPage');
    searchPage.style.display = 'block';

    document.getElementById('searchTerm').textContent = appState.searchTerm;
    document.getElementById('resultsCount').textContent = `Znaleziono ${appState.filteredProducts.length} produktów`;

    const resultsGrid = document.getElementById('searchResults');
    resultsGrid.innerHTML = appState.filteredProducts.map(product => `
    <div class="product-card" onclick="showProductDetail(${product.id})">
      <div class="product-image">📦</div>
      <span class="product-badge">${product.category}</span>
      <h3 class="product-name">${product.name}</h3>
      <div class="product-rating">${'⭐'.repeat(product.rating)}</div>
      <p class="product-description">${product.description}</p>
      <p class="product-price">${product.price.toFixed(2)} PLN</p>
      <div class="product-actions">
        <button class="btn btn-primary" onclick="event.stopPropagation(); addToCart(${product.id})">Dodaj do koszyka</button>
        <button class="btn btn-outline" onclick="event.stopPropagation(); showProductDetail(${product.id})">Szczegóły</button>
      </div>
    </div>
  `).join('');

    window.scrollTo(0, 0);
}

// Apply filters
function applyFilters() {
    let filtered = [...appState.products];

    // Category filters
    const categoryCheckboxes = document.querySelectorAll('.filter-section input[type="checkbox"][value]');
    const selectedCategories = Array.from(categoryCheckboxes)
        .filter(cb => cb.checked && ['Hałasowe', 'Świetlne', 'Zestawy'].includes(cb.value))
        .map(cb => cb.value);

    if (selectedCategories.length > 0) {
        filtered = filtered.filter(p => selectedCategories.includes(p.category));
    }

    // Price filters
    const priceRadio = document.querySelector('.filter-section input[name="price"]:checked');
    if (priceRadio && priceRadio.value !== 'all') {
        const priceRange = priceRadio.value;
        if (priceRange === '0-50') {
            filtered = filtered.filter(p => p.price <= 50);
        } else if (priceRange === '50-100') {
            filtered = filtered.filter(p => p.price > 50 && p.price <= 100);
        } else if (priceRange === '100+') {
            filtered = filtered.filter(p => p.price > 100);
        }
    }

    // Rating filters
    const ratingCheckboxes = document.querySelectorAll('.filter-section input[type="checkbox"][value="5"], .filter-section input[type="checkbox"][value="4"]');
    const selectedRatings = Array.from(ratingCheckboxes)
        .filter(cb => cb.checked)
        .map(cb => parseInt(cb.value));

    if (selectedRatings.length > 0) {
        const minRating = Math.min(...selectedRatings);
        filtered = filtered.filter(p => p.rating >= minRating);
    }

    appState.filteredProducts = filtered;
    document.getElementById('resultsCount').textContent = `Znaleziono ${filtered.length} produktów`;

    const resultsGrid = document.getElementById('searchResults');
    resultsGrid.innerHTML = filtered.map(product => `
    <div class="product-card" onclick="showProductDetail(${product.id})">
      <div class="product-image">📦</div>
      <span class="product-badge">${product.category}</span>
      <h3 class="product-name">${product.name}</h3>
      <div class="product-rating">${'⭐'.repeat(product.rating)}</div>
      <p class="product-description">${product.description}</p>
      <p class="product-price">${product.price.toFixed(2)} PLN</p>
      <div class="product-actions">
        <button class="btn btn-primary" onclick="event.stopPropagation(); addToCart(${product.id})">Dodaj do koszyka</button>
        <button class="btn btn-outline" onclick="event.stopPropagation(); showProductDetail(${product.id})">Szczegóły</button>
      </div>
    </div>
  `).join('');
}

// Sort results
function sortResults(sortBy) {
    let sorted = [...appState.filteredProducts];

    switch (sortBy) {
        case 'price-asc':
            sorted.sort((a, b) => a.price - b.price);
            break;
        case 'price-desc':
            sorted.sort((a, b) => b.price - a.price);
            break;
        case 'name':
            sorted.sort((a, b) => a.name.localeCompare(b.name));
            break;
        case 'rating':
            sorted.sort((a, b) => b.rating - a.rating);
            break;
    }

    appState.filteredProducts = sorted;

    const resultsGrid = document.getElementById('searchResults');
    resultsGrid.innerHTML = sorted.map(product => `
    <div class="product-card" onclick="showProductDetail(${product.id})">
      <div class="product-image">📦</div>
      <span class="product-badge">${product.category}</span>
      <h3 class="product-name">${product.name}</h3>
      <div class="product-rating">${'⭐'.repeat(product.rating)}</div>
      <p class="product-description">${product.description}</p>
      <p class="product-price">${product.price.toFixed(2)} PLN</p>
      <div class="product-actions">
        <button class="btn btn-primary" onclick="event.stopPropagation(); addToCart(${product.id})">Dodaj do koszyka</button>
        <button class="btn btn-outline" onclick="event.stopPropagation(); showProductDetail(${product.id})">Szczegóły</button>
      </div>
    </div>
  `).join('');
}

// Show checkout
function showCheckout() {
    document.getElementById('home').style.display = 'none';
    document.querySelector('.products-section').style.display = 'none';
    document.querySelector('.categories-section').style.display = 'none';
    document.querySelector('.testimonials-section').style.display = 'none';
    document.getElementById('searchPage').style.display = 'none';
    document.getElementById('checkoutPage').style.display = 'block';

    // Update checkout items
    const checkoutItems = document.getElementById('checkoutItems');
    checkoutItems.innerHTML = appState.cart.map(item => `
    <div class="checkout-item">
      <span>${item.name} x${item.quantity}</span>
      <span>${(item.price * item.quantity).toFixed(2)} PLN</span>
    </div>
  `).join('');

    // Update checkout summary
    const subtotal = appState.cart.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    const shipping = 15.00;
    const tax = subtotal * 0.23;
    const total = subtotal + shipping + tax;

    document.getElementById('checkoutSubtotal').textContent = `${subtotal.toFixed(2)} PLN`;
    document.getElementById('checkoutShipping').textContent = `${shipping.toFixed(2)} PLN`;
    document.getElementById('checkoutTax').textContent = `${tax.toFixed(2)} PLN`;
    document.getElementById('checkoutTotal').textContent = `${total.toFixed(2)} PLN`;

    window.scrollTo(0, 0);
}

// Place order
function placeOrder() {
    const form = document.getElementById('checkoutForm');
    if (!form.checkValidity()) {
        form.reportValidity();
        return;
    }

    alert('Zamówienie złożone pomyślnie! Dziękujemy za zakupy w BombPol!');
    appState.cart = [];
    updateCartCount();
    showPage('home');
}

// Show page
function showPage(page) {
    document.getElementById('checkoutPage').style.display = 'none';
    document.getElementById('searchPage').style.display = 'none';

    if (page === 'home') {
        document.getElementById('home').style.display = 'block';
        document.querySelector('.products-section').style.display = 'block';
        document.querySelector('.categories-section').style.display = 'block';
        document.querySelector('.testimonials-section').style.display = 'block';
    }

    window.scrollTo(0, 0);
}

// Scroll to section
function scrollToSection(sectionId) {
    // Show home page if hidden
    showPage('home');

    // Wait for page to render
    setTimeout(() => {
        const element = document.getElementById(sectionId);
        if (element) {
            const headerHeight = document.getElementById('mainHeader').offsetHeight;
            const elementPosition = element.getBoundingClientRect().top + window.pageYOffset;
            const offsetPosition = elementPosition - headerHeight - 20;

            window.scrollTo({
                top: offsetPosition,
                behavior: 'smooth'
            });
        }
    }, 100);
}

// Show notification
function showNotification(message) {
    const notification = document.createElement('div');
    notification.className = 'alert alert-success';
    notification.textContent = message;
    notification.style.position = 'fixed';
    notification.style.top = '100px';
    notification.style.right = '20px';
    notification.style.zIndex = '3000';
    notification.style.minWidth = '250px';
    notification.style.boxShadow = '0 4px 6px rgba(0,0,0,0.1)';

    document.body.appendChild(notification);

    setTimeout(() => {
        notification.remove();
    }, 3000);
}

// Initialize app when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initApp);
} else {
    initApp();
}