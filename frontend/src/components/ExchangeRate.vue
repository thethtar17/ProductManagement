<template>
    <div class="currency-converter">
      <h1 class="title">Currency Converter</h1>
  
      <div class="form-group">
        <label for="fromCurrency">From Country:</label>
        <select v-model="fromCurrency" @change="fetchExchangeRate" class="currency-select">
          <option v-for="(country, code) in countries" :key="code" :value="code">
            {{ country.name }}
          </option>
        </select>
      </div>
  
      <div class="form-group">
        <label for="amount">Amount:</label>
        <input type="number" v-model.number="amount" @input="convertCurrency" placeholder="Enter amount" class="amount-input"/>
      </div>
  
      <div class="form-group">
        <label for="toCurrency">To Country:</label>
        <select v-model="toCurrency" @change="fetchExchangeRate" class="currency-select">
          <option v-for="(country, code) in countries" :key="code" :value="code">
            {{ country.name }}
          </option>
        </select>
      </div>
  
      <div v-if="convertedAmount !== null" class="conversion-result">
        <p>{{ amount }} {{ countries[fromCurrency].currency }} = {{ convertedAmount }} {{ countries[toCurrency].currency }}</p>
      </div>
  
      <div v-else class="loading-state">
        Loading exchange rates...
      </div>
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        // Include exchange rates in the countries object
        countries: {
           
  "USD": { name: "United States", currency: "USD", rate: 1 },
  "AED": { name: "United Arab Emirates", currency: "AED", rate: 3.67 },
  "AFN": { name: "Afghanistan", currency: "AFN", rate: 70.25 },
  "ALL": { name: "Albania", currency: "ALL", rate: 94.2 },
  "AMD": { name: "Armenia", currency: "AMD", rate: 398.09 },
  "ANG": { name: "Netherlands Antilles", currency: "ANG", rate: 1.79 },
  "AOA": { name: "Angola", currency: "AOA", rate: 920.72 },
  "ARS": { name: "Argentina", currency: "ARS", rate: 1027.75 },
  "AUD": { name: "Australia", currency: "AUD", rate: 1.6 },
  "AWG": { name: "Aruba", currency: "AWG", rate: 1.79 },
  "AZN": { name: "Azerbaijan", currency: "AZN", rate: 1.7 },
  "BAM": { name: "Bosnia and Herzegovina", currency: "BAM", rate: 1.88 },
  "BBD": { name: "Barbados", currency: "BBD", rate: 2 },
  "BDT": { name: "Bangladesh", currency: "BDT", rate: 119.49 },
  "BGN": { name: "Bulgaria", currency: "BGN", rate: 1.88 },
  "BHD": { name: "Bahrain", currency: "BHD", rate: 0.376 },
  "BIF": { name: "Burundi", currency: "BIF", rate: 2950.68 },
  "BMD": { name: "Bermuda", currency: "BMD", rate: 1 },
  "BND": { name: "Brunei", currency: "BND", rate: 1.36 },
  "BOB": { name: "Bolivia", currency: "BOB", rate: 6.92 },
  "BRL": { name: "Brazil", currency: "BRL", rate: 6.19 },
  "BSD": { name: "Bahamas", currency: "BSD", rate: 1 },
  "BTN": { name: "Bhutan", currency: "BTN", rate: 85.27 },
  "BWP": { name: "Botswana", currency: "BWP", rate: 13.87 },
  "BYN": { name: "Belarus", currency: "BYN", rate: 3.36 },
  "BZD": { name: "Belize", currency: "BZD", rate: 2 },
  "CAD": { name: "Canada", currency: "CAD", rate: 1.44 },
  "CDF": { name: "Congo", currency: "CDF", rate: 2844.56 },
  "CHF": { name: "Switzerland", currency: "CHF", rate: 0.9 },
  "CLP": { name: "Chile", currency: "CLP", rate: 991.72 },
  "CNY": { name: "China", currency: "CNY", rate: 7.3 },
  "COP": { name: "Colombia", currency: "COP", rate: 4414.23 },
  "CRC": { name: "Costa Rica", currency: "CRC", rate: 507.07 },
  "CUP": { name: "Cuba", currency: "CUP", rate: 24 },
  "CVE": { name: "Cape Verde", currency: "CVE", rate: 106.03 },
  "CZK": { name: "Czech Republic", currency: "CZK", rate: 24.17 },
  "DJF": { name: "Djibouti", currency: "DJF", rate: 177.72 },
  "DKK": { name: "Denmark", currency: "DKK", rate: 7.17 },
  "DOP": { name: "Dominican Republic", currency: "DOP", rate: 60.8 },
  "DZD": { name: "Algeria", currency: "DZD", rate: 135.2 },
  "EGP": { name: "Egypt", currency: "EGP", rate: 50.91 },
  "ERN": { name: "Eritrea", currency: "ERN", rate: 15 },
  "ETB": { name: "Ethiopia", currency: "ETB", rate: 126.59 },
  "EUR": { name: "Eurozone", currency: "EUR", rate: 0.962 },
  "FJD": { name: "Fiji", currency: "FJD", rate: 2.32 },
  "FKP": { name: "Falkland Islands", currency: "FKP", rate: 0.797 },
  "FOK": { name: "Faroe Islands", currency: "FOK", rate: 7.17 },
  "GBP": { name: "United Kingdom", currency: "GBP", rate: 0.797 },
  "GEL": { name: "Georgia", currency: "GEL", rate: 2.8 },
  "GGP": { name: "Guernsey", currency: "GGP", rate: 0.797 },
  "GHS": { name: "Ghana", currency: "GHS", rate: 14.99 },
  "GIP": { name: "Gibraltar", currency: "GIP", rate: 0.797 },
  "GMD": { name: "Gambia", currency: "GMD", rate: 72.5 },
  "GNF": { name: "Guinea", currency: "GNF", rate: 8609.62 },
  "GTQ": { name: "Guatemala", currency: "GTQ", rate: 7.7 },
  "GYD": { name: "Guyana", currency: "GYD", rate: 209.14 },
  "HKD": { name: "Hong Kong", currency: "HKD", rate: 7.77 },
  "HNL": { name: "Honduras", currency: "HNL", rate: 25.4 },
  "HRK": { name: "Croatia", currency: "HRK", rate: 7.25 },
  "HTG": { name: "Haiti", currency: "HTG", rate: 130.76 },
  "HUF": { name: "Hungary", currency: "HUF", rate: 394.95 },
  "IDR": { name: "Indonesia", currency: "IDR", rate: 16214.86 },
  "ILS": { name: "Israel", currency: "ILS", rate: 3.65 },
  "IMP": { name: "Isle of Man", currency: "IMP", rate: 0.797 },
  "INR": { name: "India", currency: "INR", rate: 85.27 },
  "IQD": { name: "Iraq", currency: "IQD", rate: 1310.4 },
  "IRR": { name: "Iran", currency: "IRR", rate: 42036.06 },
  "ISK": { name: "Iceland", currency: "ISK", rate: 139.62 },
  "JEP": { name: "Jersey", currency: "JEP", rate: 0.797 },
  "JMD": { name: "Jamaica", currency: "JMD", rate: 155.85 },
  "JOD": { name: "Jordan", currency: "JOD", rate: 0.709 },
  "JPY": { name: "Japan", currency: "JPY", rate: 157.17 },
  "KES": { name: "Kenya", currency: "KES", rate: 129.2 },
  "KGS": { name: "Kyrgyzstan", currency: "KGS", rate: 86.97 },
  "KHR": { name: "Cambodia", currency: "KHR", rate: 4007.84 },
  "KID": { name: "Kiribati", currency: "KID", rate: 1.6 },
  "KMF": { name: "Comoros", currency: "KMF", rate: 473.09 },
  "KRW": { name: "South Korea", currency: "KRW", rate: 1457.91 },
  "KWD": { name: "Kuwait", currency: "KWD", rate: 0.308 },
  "KYD": { name: "Cayman Islands", currency: "KYD", rate: 0.833 },
  "KZT": { name: "Kazakhstan", currency: "KZT", rate: 515.87 },
  "LAK": { name: "Laos", currency: "LAK", rate: 21912.78 },
  "LBP": { name: "Lebanon", currency: "LBP", rate: 89500 },
  "LKR": { name: "Sri Lanka", currency: "LKR", rate: 294.67 },
  "LRD": { name: "Liberia", currency: "LRD", rate: 181.71 },
  "LSL": { name: "Lesotho", currency: "LSL", rate: 18.66 },
  "LYD": { name: "Libya", currency: "LYD", rate: 4.91 },
  "MAD": { name: "Morocco", currency: "MAD", rate: 10.05 },
  "MDL": { name: "Moldova", currency: "MDL", rate: 18.38 },
  "MGA": { name: "Madagascar", currency: "MGA", rate: 4723.1 },
  "MKD": { name: "North Macedonia", currency: "MKD", rate: 59.16 },
  "MMK": { name: "Myanmar", currency: "MMK", rate: 2097.61 },
  "MNT": { name: "Mongolia", currency: "MNT", rate: 3399.84 },
  "MOP": { name: "Macau", currency: "MOP", rate: 8 },
  "MRU": { name: "Mauritania", currency: "MRU", rate: 40 },
  "MUR": { name: "Mauritius", currency: "MUR", rate: 46.93 },
  "MVR": { name: "Maldives", currency: "MVR", rate: 15.45 },
  "MWK": { name: "Malawi", currency: "MWK", rate: 1738.84 },
  "MXN": { name: "Mexico", currency: "MXN", rate: 20.16 },
  "MYR": { name: "Malaysia", currency: "MYR", rate: 4.49 },
  "MZN": { name: "Mozambique", currency: "MZN", rate: 63.85 },
  "NAD": { name: "Namibia", currency: "NAD", rate: 18.66 },
  "NGN": { name: "Nigeria", currency: "NGN", rate: 1536.22 },
  "NIO": { name: "Nicaragua", currency: "NIO", rate: 36.79 },
  "NOK": { name: "Norway", currency: "NOK", rate: 11.35 },
  "NPR": { name: "Nepal", currency: "NPR", rate: 136.43 },
  "NZD": { name: "New Zealand", currency: "NZD", rate: 1.77 },
  "OMR": { name: "Oman", currency: "OMR", rate: 0.384 },
  "PAB": { name: "Panama", currency: "PAB", rate: 1 },
  "PEN": { name: "Peru", currency: "PEN", rate: 3.73 },
  "PGK": { name: "Papua New Guinea", currency: "PGK", rate: 4.03 },
  "PHP": { name: "Philippines", currency: "PHP", rate: 58.59 },
  "PKR": { name: "Pakistan", currency: "PKR", rate: 278.39 },
  "PLN": { name: "Poland", currency: "PLN", rate: 4.1 },
  "PYG": { name: "Paraguay", currency: "PYG", rate: 7752 },
  "QAR": { name: "Qatar", currency: "QAR", rate: 3.64 },
  "RON": { name: "Romania", currency: "RON", rate: 4.78 },
  "RSD": { name: "Serbia", currency: "RSD", rate: 112.39 },
  "RUB": { name: "Russia", currency: "RUB", rate: 99.82 },
  "RWF": { name: "Rwanda", currency: "RWF", rate: 1380.47 },
  "SAR": { name: "Saudi Arabia", currency: "SAR", rate: 3.75 },
  "SBD": { name: "Solomon Islands", currency: "SBD", rate: 8.49 },
  "SCR": { name: "Seychelles", currency: "SCR", rate: 13.95 },
  "SDG": { name: "Sudan", currency: "SDG", rate: 511.72 },
  "SEK": { name: "Sweden", currency: "SEK", rate: 11.08 },
  "SGD": { name: "Singapore", currency: "SGD", rate: 1.36 },
  "SHP": { name: "Saint Helena", currency: "SHP", rate: 0.797 },
  "SLE": { name: "Sierra Leone", currency: "SLE", rate: 22.88 },
  "SLL": { name: "Sierra Leone", currency: "SLL", rate: 22879.65 },
  "SOS": { name: "Somalia", currency: "SOS", rate: 571.44 },
  "SRD": { name: "Suriname", currency: "SRD", rate: 35.18 },
  "SSP": { name: "South Sudan", currency: "SSP", rate: 3898.73 },
  "STN": { name: "São Tomé and Príncipe", currency: "STN", rate: 23.56 },
  "SYP": { name: "Syria", currency: "SYP", rate: 12940.1 },
  "SZL": { name: "Eswatini", currency: "SZL", rate: 18.66 },
  "THB": { name: "Thailand", currency: "THB", rate: 34.16 },
  "TJS": { name: "Tajikistan", currency: "TJS", rate: 10.91 },
  "TMT": { name: "Turkmenistan", currency: "TMT", rate: 3.5 },
  "TND": { name: "Tunisia", currency: "TND", rate: 3.19 },
  "TOP": { name: "Tonga", currency: "TOP", rate: 2.4 },
  "TRY": { name: "Turkey", currency: "TRY", rate: 35.22 },
  "TTD": { name: "Trinidad and Tobago", currency: "TTD", rate: 6.78 },
  "TVD": { name: "Tuvalu", currency: "TVD", rate: 1.6 },
  "TWD": { name: "Taiwan", currency: "TWD", rate: 32.67 },
  "TZS": { name: "Tanzania", currency: "TZS", rate: 2423.37 },
  "UAH": { name: "Ukraine", currency: "UAH", rate: 41.89 },
  "UGX": { name: "Uganda", currency: "UGX", rate: 3663.91 },
  "UYU": { name: "Uruguay", currency: "UYU", rate: 44.17 },
  "UZS": { name: "Uzbekistan", currency: "UZS", rate: 12868.92 },
  "VES": { name: "Venezuela", currency: "VES", rate: 51.64 },
  "VND": { name: "Vietnam", currency: "VND", rate: 25424.78 },
  "VUV": { name: "Vanuatu", currency: "VUV", rate: 119.87 },
  "WST": { name: "Samoa", currency: "WST", rate: 2.81 },
  "XAF": { name: "Central Africa", currency: "XAF", rate: 630.78 },
  "XCD": { name: "East Caribbean", currency: "XCD", rate: 2.7 },
  "XDR": { name: "Special Drawing Rights", currency: "XDR", rate: 0.767 },
  "XOF": { name: "West African", currency: "XOF", rate: 630.78 },
  "XPF": { name: "Pacific Franc", currency: "XPF", rate: 114.75 },
  "YER": { name: "Yemen", currency: "YER", rate: 249.5 },
  "ZAR": { name: "South Africa", currency: "ZAR", rate: 18.66 },
  "ZMW": { name: "Zambia", currency: "ZMW", rate: 27.74 },
  "ZWL": { name: "Zimbabwe", currency: "ZWL", rate: 25.77 }

        // Add more countries as needed
      },
      amount: 0,
      fromCurrency: "USD",
      toCurrency: "JPY",
      exchangeRate: 1,
      convertedAmount: null,
    };
  },
    methods: {
      convertCurrency() {
        if (this.fromCurrency && this.toCurrency) {
          const fromRate = this.countries[this.fromCurrency]?.rate;
          const toRate = this.countries[this.toCurrency]?.rate;
  
          if (fromRate && toRate) {
            const rate = toRate / fromRate;
            this.convertedAmount = (this.amount * rate).toFixed(2);
          } else {
            console.error("Invalid exchange rate data");
          }
        }
      },
      fetchExchangeRate() {
        // Calculate exchange rate only when both currencies are selected
        this.convertCurrency();
      },
    },
  };
  </script>
  
  <style scoped>
  .currency-converter {
    padding: 2rem;
    max-width: 600px;
    margin: 0 auto;
    text-align: center;
  }
  .title {
    font-size: 2rem;
    margin-bottom: 1rem;
  }
  .form-group {
    margin-bottom: 1rem;
  }
  .currency-select
  {
    padding: 0.5rem;
    font-size: 1rem;
    width: 100%;
  }
  .amount-input 
  {
    padding: 0.5rem;
    font-size: 1rem;
    width: 97%;
  }
  .conversion-result {
    margin-top: 1rem;
    font-size: 1.25rem;
    font-weight: bold;
  }
  </style>
  