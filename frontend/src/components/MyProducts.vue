<template>
  <div class="user-products">
    <button class="btn-add">
        <router-link to="/addproduct" class="nav-link">Add</router-link>
       
      </button>    
      <button class="btn-add">
      <router-link to="/displaycart"  class="nav-link">Cart</router-link></button>   
    <h1 class="title">Your Products</h1>


    <div v-if="products.length > 0">
      <div class="product-list">
        <div v-for="product in products" :key="product.id" class="product-card">
          <img :src="product.imageData ? product.imageUrl : 'https://via.placeholder.com/300x200'" alt="Product Image" class="product-image" />
          <!-- <img src="https://via.placeholder.com/300x200" alt="product image" class="product-image" /> -->
          <div class="product-info">
            <h3 class="product-name">{{ product.name }}</h3>
            <p class="product-price">Price: ¥{{ product.price }}</p>
            <p class="product-stock">Stock: {{ product.stock }}</p>
          </div>
          <router-link :to="'/editproduct/' + product.id" class="edit-button">Edit</router-link>
          <button @click="deleteProduct(product.id)" class="delete-button">Delete</button>
        </div>
      </div>
    </div>
    <div v-else>
      <p class="no-products">You don't have any products.</p>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "UserProducts",

  data() {
    return {
      products: [],
      isAuthenticated: false,
 
    };
  },
  computed: {
    token() {
      return localStorage.getItem("token");
    },
  },
  methods: {
    async fetchUserProducts() {
      if (this.token) {
        try {
          const response = await axios.get("http://localhost:5004/api/products/current-user-products", {
            headers: {
              Authorization: `Bearer ${this.token}`,
            },
          });
          this.products = response.data;
          this.isAuthenticated = true;
        } catch (error) {
          console.error("Error fetching user products:", error.response || error.message || error);
          this.isAuthenticated = false;
        }
      } else {
        this.isAuthenticated = false;
      }
    },
    
 
  async deleteProduct(productId) {
      if (confirm("Are you sure you want to delete this product?")) {
        try {
          await axios.delete(`http://localhost:5004/api/products/${productId}`, {
            headers: {
              Authorization: `Bearer ${this.token}`,
            },
          });
          // Remove the deleted product from the list
          this.products = this.products.filter(product => product.id !== productId);
          alert("Product deleted successfully.");
        } catch (error) {
          console.error("Error deleting product:", error);
          alert("Failed to delete product. Please try again.");
        }
      }
    },
   


 
  
  } ,   
  created() {
    this.fetchUserProducts();
  },
  
  
  

};
</script>

<style scoped>
/* General styles */
.user-products {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
  font-family: Arial, sans-serif;
}

.title {
  text-align: center;
  font-size: 36px;
  color: #333;
  margin-bottom: 20px;
}

/* Product List Container */
.product-list {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 20px;
}

/* Product Card */
.product-card {
  background-color: #fff;
  border-radius: 8px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  transition: transform 0.3s ease;
}

.product-card:hover {
  transform: translateY(-10px);
}

/* Product Image */
.product-image {
  width: 100%;
  height: 200px;
  object-fit: cover;
  border-bottom: 2px solid #eee;
}

/* Product Info */
.product-info {
  padding: 15px;
}

.product-name {
  font-size: 20px;
  font-weight: bold;
  color: #333;
}

.product-price,
.product-stock {
  font-size: 16px;
  color: #555;
  margin-top: 8px;
}

.no-products {
  text-align: center;
  font-size: 18px;
  color: #777;
}
 /* Styling for the Add button */
 .btn-add {
    background-color: #4CAF50; /* Green background */
    color: white; /* White text */
    padding: 10px 20px; /* Spacing around text */
    font-size: 16px; /* Font size */
    border: none; /* No border */
    border-radius: 5px; /* Rounded corners */
    cursor: pointer; /* Pointer on hover */
    transition: background-color 0.3s ease; /* Smooth transition */
  }

  .btn-add:hover {
    background-color: #45a049; /* Darker green on hover */
  }

  .btn-add .nav-link {
    text-decoration: none; /* Remove underline */
    color: white; /* Ensure link text is white */
    display: inline-block; /* Make sure the link behaves like a block */
  }
  .delete-button {
  background-color: red;
  color: white;
  border: none;
  padding: 8px 16px;
  cursor: pointer;
  margin-top: 10px;
  border-radius: 4px;
  transition: background-color 0.3s ease;
  margin: 5px;
}

.delete-button:hover {
  background-color: darkred;
}
.edit-button {
  background-color: rgb(0, 61, 153);
  color: white;
  border: none;
  padding: 8px 16px;
  cursor: pointer;
  margin-top: 10px;
  border-radius: 4px;
  transition: background-color 0.3s ease;
  margin: 5px;
  text-decoration: none;
  font-size: medium;
}

.edit-button:hover {
  background-color: rgb(16, 0, 139);
}

</style>
