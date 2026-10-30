// src/main.js
import { createApp } from 'vue';
import App from './App.vue';
import axios from 'axios';  // Ensure axios is imported
import router from './router';

// Set a default base URL for axios (optional)
axios.defaults.baseURL = 'http://localhost:5004';  // Replace with your backend API base URL

const app = createApp(App);
app.config.globalProperties.$axios = axios;  // Make axios available globally if needed

app.use(router);
app.mount('#app');
