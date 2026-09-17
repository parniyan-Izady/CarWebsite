/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Views/**/*.cshtml",
    "./Areas/**/*.cshtml",
    "./wwwroot/js/**/*.js"
  ],
  theme: {
    extend: {
      colors: {
        brand: {
          50: '#f0f7ff',
          100: '#e0effe',
          200: '#bae0fd',
          300: '#7cc7fb',
          400: '#36abf7',
          500: '#0c90e7',
          600: '#0284c7', // Primary highlight / Cyan Sapphire
          700: '#035ea6',
          800: '#074f88',
          900: '#0c4270',
          950: '#082a4a'
        },
        navy: {
          800: '#1e293b',
          900: '#0f172a', // Deep Luxury Automotive Dark
          950: '#090d16'
        }
      },
      fontFamily: {
        sans: ['Inter', 'system-ui', '-apple-system', 'Segoe UI', 'Roboto', 'sans-serif']
      }
    },
  },
  plugins: [],
}
