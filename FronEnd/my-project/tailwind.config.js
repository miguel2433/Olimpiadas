/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      height: {
        '10vh': '10vh',
      },
      colors: {
        cyanOscuro: '#162938',
      },
    spacing: {
      '1': '-4px',
      '2': '-8px',
    },
    },
  },
  plugins: [],
}