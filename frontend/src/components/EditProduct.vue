<template>
  <div class="edit-product-container">
    <h1 class="page-title">Edit Product</h1>
    <form @submit.prevent="updateProduct" class="edit-form">
      <div class="form-group">
        <label for="name">Product Name</label>
        <input
          v-model="editableProduct.name"
          id="name"
          type="text"
          placeholder="Enter product name"
          required
          class="input-field"
        />
      </div>

      <div class="form-group">
        <label for="price">Price</label>
        <input
          v-model="editableProduct.price"
          id="price"
          type="number"
          placeholder="Enter price"
          required
          class="input-field"
        />
      </div>

      <div class="form-group">
        <label for="stock">Stock</label>
        <input
          v-model="editableProduct.stock"
          id="stock"
          type="number"
          placeholder="Enter stock quantity"
          required
          class="input-field"
        />
      </div>

      <div class="form-group">
        <label for="image">Product Image (optional)</label>
        <input
          type="file"
          id="image"
          @change="handleImageUpload"
          class="input-file"
        />
      </div>

      <div class="button-container">
        <button type="submit" class="submit-button">Update Product</button>
      </div>
    </form>
  </div>
</template>

<script>
import axios from "axios";
export default {
  data() {
    return {
      editableProduct: {
        id: null,
        name: '',
        price: '',
        stock: '',
        image: null, // This will hold the new image file
      },
    };
  },
  methods: {
    async fetchProduct(id) {
      try {
        const response = await axios.get(`http://localhost:5004/api/products/${id}`);
        this.editableProduct = response.data;
      } catch (error) {
        console.error("Error fetching product:", error);
        alert("Failed to load product details.");
      }
    },
    handleImageUpload(event) {
      const file = event.target.files[0];
      if (file) {
        this.editableProduct.image = file; // Store the selected file
      }
    },
    async updateProduct() {
      const formData = new FormData();
      formData.append("name", this.editableProduct.name);
      formData.append("price", this.editableProduct.price);
      formData.append("stock", this.editableProduct.stock);

      if (this.editableProduct.image) {
        formData.append("image", this.editableProduct.image);
      }

      try {
        await axios.put(`http://localhost:5004/api/products/${this.editableProduct.id}`, formData, {
          headers: {
            "Authorization": `Bearer ${localStorage.getItem("token")}`,
            "Content-Type": "multipart/form-data",
          },
        });

        alert("Product updated successfully.");
        this.$router.push("/myproduct"); // Redirect after successful update
      } catch (error) {
        console.error("Error updating product:", error);
        alert("Failed to update product. Please try again.");
      }
    }
  },
  created() {
    const productId = this.$route.params.id;
    this.fetchProduct(productId); // Fetch the product details when the component is created
  }
};
</script>

<style scoped>
.edit-product-container {
  max-width: 700px;
  margin: 40px auto;
  padding: 20px;
  background-color: #fff;
  border-radius: 8px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
}

.page-title {
  text-align: center;
  font-size: 32px;
  color: #333;
  margin-bottom: 30px;
}

.edit-form {
  display: flex;
  flex-direction: column;
  gap: 20px;
  padding: 20px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.label {
  font-weight: bold;
  font-size: 16px;
  color: #555;
}

.input-field {
  padding: 12px;
  font-size: 16px;
  border: 1px solid #ccc;
  border-radius: 8px;
  outline: none;
  transition: border-color 0.3s;
}

.input-field:focus {
  border-color: #4CAF50;
}

.input-file {
  font-size: 16px;
  padding: 6px;
  border-radius: 8px;
  border: 1px solid #ccc;
  cursor: pointer;
}

.button-container {
  text-align: center;
}

.submit-button {
  background-color: #4CAF50;
  color: white;
  padding: 14px 30px;
  font-size: 18px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.submit-button:hover {
  background-color: #45a049;
}

.submit-button:focus {
  outline: none;
}
</style>
