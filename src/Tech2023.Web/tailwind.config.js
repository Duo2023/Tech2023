/** @type {import('tailwindcss').Config} */

const defaultTheme = require("tailwindcss/defaultTheme");

module.exports = {
  content: ["./Views/**/*.cshtml"],
  theme: {
    extend: {
      fontFamily: {
        sans: ["Manrope", ...defaultTheme.fontFamily.sans],
      },
      colors: {
        primary: "#3C38FA",
        secondary: "#191A1F",
        background: "#131418",
        hint: "#3E3E42",
      },
    },
  },
  plugins: [],
};
