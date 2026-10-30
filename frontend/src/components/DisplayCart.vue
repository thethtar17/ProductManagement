<template>
    <div>
      <h1>Your Cart</h1>
      <div v-if="loading" class="loading">Loading your cart...</div>
      <div v-else>
        <div v-if="cartItems.length === 0" class="empty-cart">
          Your cart is empty.
        </div>
        <div v-else class="cart-items">
          <div class="cart-item" v-for="item in cartItems" :key="item.id">
            <img :src="item.imageUrl" :alt="item.name" class="product-image" />
            <div class="product-details">
              <h3>{{ item.name }}</h3>
              <p>Price: ${{ item.price.toFixed(2) }}</p>
              <p>Stock: {{ item.stock }}</p>
              <p>Quantity: {{ item.quantity }}</p> <!-- Display the quantity -->
            </div>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import axios from "axios";
  
  export default {
    data() {
      return {
        cartItems: [],
        loading: true,
        error: null,
      };
    },
    async created() {
      try {
        const response = await axios.get("http://localhost:5004/api/products/current-user-cart", {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        });
        this.cartItems = response.data;
      } catch (err) {
        this.error = err.response ? err.response.data : "Failed to fetch cart.";
      } finally {
        this.loading = false;
      }
    },
  };
  </script>
  
  <style scoped>
  .loading {
    font-size: 18px;
    text-align: center;
  }
  
  .empty-cart {
    text-align: center;
    font-size: 18px;
    color: #888;
  }
  
  .cart-items {
    display: flex;
    flex-wrap: wrap;
    gap: 20px;
    margin-top: 20px;
  }
  
  .cart-item {
    border: 1px solid #ccc;
    border-radius: 8px;
    padding: 10px;
    width: 200px;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  }
  
  .product-image {
    width: 100%;
    height: 250px;
    object-fit: cover;
    border-radius: 4px;
  }
  
  .product-details {
    margin-top: 10px;
  }
  
  .product-details h3 {
    font-size: 18px;
    margin: 0 0 10px;
  }
  
  .product-details p {
    margin: 5px 0;
  }
  </style>
  