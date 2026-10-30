
<template>
  <div class="add-product">
    <h2 class="page-title">Add Product</h2>
    <form @submit.prevent="addProduct" class="form-container">
      <div class="form-group">
        <label for="name">Product Name</label>
        <input type="text" id="name" v-model="product.name" placeholder="Enter product name" required class="input-field"/>
      </div>

      <div class="form-group">
        <label for="price">Price</label>
        <input type="number" id="price" v-model="product.price" step="0.01" placeholder="Enter price" required class="input-field"/>
      </div>

      <div class="form-group">
        <label for="stock">Stock</label>
        <input type="number" id="stock" v-model="product.stock" placeholder="Enter stock" required class="input-field"/>
      </div>

      <div class="form-group">
        <label for="image">Product Image (optional)</label>
        <input type="file" id="image" @change="handleFileUpload" class="input-file"/>
      </div>

      <div class="button-container">
        <button type="submit" class="submit-button">Add Product</button>
      </div>
    </form>
    <p v-if="message" class="message">{{ message }}</p>
  </div>
</template>


<script>
import axios from "axios";

export default {
  data() {
    return {
      product: {
        name: "",
        price: 0,
        stock: 0,
        image: null,
      },
      message: "",
    };
  },
  methods: {
    handleFileUpload(event) {
      this.product.image = event.target.files[0];
    },
    async addProduct() {
      const formData = new FormData();
      formData.append("name", this.product.name);
      formData.append("price", this.product.price);
      formData.append("stock", this.product.stock);
      formData.append("image", this.product.image);

      try {
        const response = await axios.post(
          "http://localhost:5004/api/products/add",
          formData,
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("token")}`,
              "Content-Type": "multipart/form-data",
            },
          }
        );
        this.message = response.data.Message || "Product added successfully!";
        this.product = { name: "", price: 0, stock: 0, image: null }; // Reset form
      } catch (error) {
        console.error("Error adding product:", error.response.data);
        this.message = error.response.data || "Failed to add product.";
      }
    },
  },
};
</script>

<style scoped>
.add-product {
  max-width: 700px;
  margin: 40px auto;
  padding: 30px;
  background-color: #fff;
  border-radius: 10px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
}

.page-title {
  text-align: center;
  font-size: 28px;
  color: #333;
  margin-bottom: 30px;
}

.form-container {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

label {
  font-size: 16px;
  font-weight: 600;
  color: #444;
}

.input-field, .input-file {
  padding: 12px;
  font-size: 16px;
  border-radius: 8px;
  border: 1px solid #ccc;
  outline: none;
  transition: border-color 0.3s;
}

.input-field:focus, .input-file:focus {
  border-color: #4CAF50;
}

.input-file {
  font-size: 14px;
}

.button-container {
  text-align: center;
}

.submit-button {
  padding: 14px 30px;
  font-size: 18px;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.submit-button:hover {
  background-color: #0056b3;
}

.submit-button:focus {
  outline: none;
}

.message {
  text-align: center;
  margin-top: 20px;
  font-size: 16px;
  color: #4CAF50;
}
</style>
