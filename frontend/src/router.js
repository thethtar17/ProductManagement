
// src/router.js
import { createRouter, createWebHistory } from 'vue-router';
import Home from './components/HelloWorld.vue'; // Import Home component
import UserLogin from './components/UserLogin.vue'; // Import UserLogin component

import ExchangeRate from './components/ExchangeRate.vue'; // Import ExchangeRate component
import UserRegister from './components/UserRegister.vue';
import ProductList from './components/ProductList.vue';
import MyProductList from './components/MyProducts.vue';
import TranslateLang from './components/TranslateLang.vue';
import AddProduct from './components/AddProduct.vue';
import EditProduct from './components/EditProduct.vue';
import DisplayCart from './components/DisplayCart.vue';
// Define routes
const routes = [
  {
    path: '/', // The root path
    name: 'Home', // Route name
    component: Home, // Component to render
  },
  {
    path: '/login', // Path for login page
    name: 'Login',
    component: UserLogin, // Component to render for login
  },
  {
    path: '/register', // Path for registration page
    name: 'Register',
    component: UserRegister, // Component to render for registration
  },
  {
    path: '/product', // The root path
    name: 'ProductList', // Route name
    component: ProductList, // Component to render
    meta: { requiresAuth: true }
  },
  {
    path: '/myproduct', // The root path
    name: 'MyProductList', // Route name
    component: MyProductList, // Component to render
    meta: { requiresAuth: true }
  },
  {
    path: '/addproduct', // Path for login page
    name: 'AddProduct',
    component: AddProduct, 
    meta: { requiresAuth: true }// Component to render for login
  },
  { path: '/exchange-rate', component: ExchangeRate, meta: { requiresAuth: true } , name: 'ExchangeRate' },
  {
    path: "/translate", // Add a route for the exchange rate page
    name: "TranslateLang",
    component: TranslateLang,
  },
  {
    path: '/editproduct/:id',
    name: 'edit-product',
    component: EditProduct
  }
  , {
    path: '/displaycart',
    name: 'display-cart',
    component: DisplayCart
  }
  // You can add more routes as needed
];

// Create the router instance
const router = createRouter({
  history: createWebHistory(), // Use HTML5 history mode (no hash in URL)
  routes, // Define the routes array here
});
router.beforeEach((to, from, next) => {
  const isAuthenticated = !!localStorage.getItem("token");
  if (to.matched.some(record => record.meta.requiresAuth) && !isAuthenticated) {
    // Redirect to login page if not authenticated
    next('/login');
  } else {
    next();
  }
});
export default router; // Export the router instance
