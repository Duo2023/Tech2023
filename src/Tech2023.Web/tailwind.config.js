/** @type {import('tailwindcss').Config} */

const defaultTheme = require("tailwindcss/defaultTheme");

module.exports = {
    content: ["./Views/**/*.cshtml", "./Areas/**/*.cshtml"],
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
                "light-hint": "#ACADAE",
            },
            keyframes: {
                "fade-in": {
                    from: { opacity: 0.75 },
                    to: { opacity: 1 },
                },
                grow: {
                    from: { transform: "scale(0.99)" },
                    to: { transform: "scale(1)" },
                },

                "slide-down": {
                    from: { transform: "translateY(-0.125rem)" },
                    to: { transfom: "translateY(0);" },
                },
            },
            animation: {
                "fade-in": "fade-in 150ms ease-out",
                grow: "grow 75ms ease-in-out",
                "slide-down": "slide-down 75ms ease-out, fade-in 75ms ease-out",
            },
        },
    },
    plugins: [],
};
