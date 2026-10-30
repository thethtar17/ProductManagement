<template>
  <div id="app">
    <!-- Navigation Bar -->
    <nav>
      <ul class="nav-links">
        <li v-if="!isAuthenticated"><router-link to="/login" class="nav-link">Login</router-link></li>
        <li v-if="!isAuthenticated"><router-link to="/register" class="nav-link">Register</router-link></li>
        <li v-if="isAuthenticated"><router-link to="/product" class="nav-link">Product</router-link></li>
        <li v-if="isAuthenticated"><router-link to="/exchange-rate" class="nav-link">Exchange Rate</router-link></li>
        <li v-if="isAuthenticated"><router-link to="/myproduct" class="nav-link">MyProduct</router-link></li>
        <li v-if="isAuthenticated"><button @click="logout" class="logout-button">Logout</button></li>
      </ul>
    </nav>

    <!-- Display user details if authenticated -->
    <div v-if="isAuthenticated" class="welcome-message">
      <p>Welcome, <strong>{{ user.username }}</strong> (Email: {{ user.userId }})</p>
    </div>

    <!-- Router view to display corresponding component -->
    <router-view></router-view>
  </div>
</template>

<script>
import axios from 'axios';

axios.defaults.baseURL = 'http://localhost:5004';

export default {
  name: 'App',
  data() {
    return {
      user: {} // Store user data here
    };
  },
  computed: {
    isAuthenticated() {
      // Check for JWT token in localStorage to verify if user is authenticated
      return !!localStorage.getItem("token");
    }
  },
  methods: {
    async fetchUserDetails() {
      const token = localStorage.getItem('token');
      try {
        const response = await axios.get("/api/user/current-user", {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });
        this.user = response.data;
      } catch (error) {
        console.error('Error fetching user details:', error);
      }
    },

    logout() {
      localStorage.removeItem("token");
      window.location.reload();
      this.$router.push("/login");  // Redirect to login page after logout
    }
  },
  created() {
    if (this.isAuthenticated) {
      this.fetchUserDetails();
    }
  }
};
</script>

<style>
/* Body and overall layout */
#app {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  background-color: #f4f6f9;
  color: #333;
  margin: 0;
  padding: 0;
}

/* Navigation Bar */
nav {
  background: linear-gradient(135deg, #2c652e, #6da16f);
  padding: 15px 20px;
}

.nav-links {
  list-style: none;
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin: 0;
}

.nav-links li {
  margin: 0 15px;
}

.nav-link {
  color: white;
  font-size: 18px;
  text-decoration: none;
  font-weight: 500;
  transition: color 0.3s ease, transform 0.3s ease;
}

.nav-link:hover {
  color: #f8f8f8;
  transform: scale(1.1);
}

/* Logout Button */
.logout-button {
  background-color: #f44336;
  color: white;
  border: none;
  padding: 10px 15px;
  font-size: 16px;
  cursor: pointer;
  border-radius: 5px;
  transition: background-color 0.3s ease, transform 0.3s ease;
}

.logout-button:hover {
  background-color: #d32f2f;
  transform: scale(1.05);
}

/* Welcome Message */
.welcome-message {
  text-align: center;
  margin-top: 20px;
  font-size: 20px;
}

.welcome-message strong {
  color: #56d25a;
}

/* Mobile Responsiveness */
@media (max-width: 768px) {
  .nav-links {
    flex-direction: column;
    align-items: flex-start;
  }

  .nav-links li {
    margin: 10px 0;
  }

  .logout-button {
    width: 100%;
    padding: 12px;
  }
}
</style>
