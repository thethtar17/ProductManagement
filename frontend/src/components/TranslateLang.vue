<template>
    <div class="translate-container">
      <h1>Language Translator</h1>
      <div class="input-container">
        <textarea v-model="textToTranslate" placeholder="Enter text to translate" rows="4"></textarea>
      </div>
      <div class="language-selector">
        <select v-model="sourceLanguage">
          <option value="en">English</option>
          <option value="es">Spanish</option>
          <option value="fr">French</option>
          <option value="de">German</option>
          <!-- Add more languages as needed -->
        </select>
        <span>to</span>
        <select v-model="targetLanguage">
          <option value="es">Spanish</option>
          <option value="en">English</option>
          <option value="fr">French</option>
          <option value="de">German</option>
          <!-- Add more languages as needed -->
        </select>
      </div>
      <button @click="translateText">Translate</button>
  
      <div class="translated-text" v-if="translatedText">
        <h3>Translated Text:</h3>
        <p>{{ translatedText }}</p>
      </div>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    data() {
      return {
        textToTranslate: '',
        sourceLanguage: 'en',
        targetLanguage: 'es',
        translatedText: ''
      };
    },
    methods: {
      async translateText() {
        try {
          const response = await axios.post('http://localhost:5004/translate', {
            text: this.textToTranslate,
            sourceLanguage: this.sourceLanguage,
            targetLanguage: this.targetLanguage
          });
          this.translatedText = response.data.translatedText;
        } catch (error) {
          console.error('Error translating text:', error);
        }
      }
    }
  };
  </script>
  
  <style scoped>
  /* Style your page here */
  </style>
  