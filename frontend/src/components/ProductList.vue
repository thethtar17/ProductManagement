<template>
  <div class="product-list-container">
    <h1 class="title">Product List</h1>
    <div v-if="products.length > 0" class="product-list">
      <div v-for="product in products" :key="product.id" class="product-card">
        <img :src="product.imageData ? product.imageUrl : 'https://via.placeholder.com/300x200'" alt="Product Image" class="product-image" />


        <!-- <img :src="product.imageUrl" alt="Product Image" class="product-image" /> -->
        <div class="product-info">
          <h3 class="product-name">{{ product.name }}</h3>
          <p class="product-price">¥{{ product.price }}</p>
          <p class="product-stock">Stock: {{ product.stock }}</p>
          <button @click="addToCart(product.id)">Add to Cart</button>
        </div>
      </div>
    </div>
    <div v-else>
      <p class="no-products">No products available at the moment.</p>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      products: []
        // Make sure token is set properly
    };
  },
  computed: {
    token() {
      return localStorage.getItem("token");
    },
  },

  methods: {
    async fetchProducts() {
  try {
    console.log("Token:", this.token); // Log the token
    const response = await axios.get("http://localhost:5004/api/products", {
      headers: {
        Authorization: `Bearer ${this.token}`,
      },
    });
    console.log(response.data);
    this.products = response.data;
  } catch (error) {
    console.error("Error fetching products:", error);
  }
},
async addToCart(productId) {
      try {
        await axios.post(
          "http://localhost:5004/api/products/add-to-cart",
          { productId, quantity: 1 }, // Default quantity is 1
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("token")}`,
            },
          }
        );
        alert("Product added to cart!");
      } catch (error) {
        console.error("Error adding to cart:", error);
        alert("Failed to add product to cart.");
      }
    }
,
  },
  created() {
    this.fetchProducts();
  },
};
</script>

<style scoped>
/* General styles */
.product-list-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
  font-family: 'Arial', sans-serif;
  background-color: #f8f8f8;
}

.title {
  text-align: center;
  font-size: 36px;
  font-weight: bold;
  color: #333;
  margin-bottom: 20px;
  text-transform: uppercase;
}

/* Product List */
.product-list {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 20px;
}

.product-card {
  background-color: #fff;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.product-card:hover {
  transform: translateY(-10px);
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
}

.product-image {
  width: 100%;
  height: 200px;
  object-fit: cover;
}

.product-info {
  padding: 15px;
}

.product-name {
  font-size: 20px;
  font-weight: bold;
  color: #333;
}

.product-price {
  font-size: 18px;
  color: #ff6f61;
  margin-top: 10px;
}

/* No Products */
.no-products {
  text-align: center;
  font-size: 18px;
  color: #777;
  margin-top: 20px;
}
</style>
