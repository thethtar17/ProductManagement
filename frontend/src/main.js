// src/main.js
import { createApp } from 'vue';
import App from './App.vue';
import axios from 'axios';  // Ensure axios is imported
import router from './router';

axios.defaults.baseURL = process.env.VUE_APP_API_BASE_URL || '';

const app = createApp(App);
app.config.globalProperties.$axios = axios;  // Make axios available globally if needed

app.use(router);
app.mount('#app');
