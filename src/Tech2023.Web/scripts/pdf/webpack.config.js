const path = require("path");
const copyPlugin = require("copy-webpack-plugin");

module.exports = {
    entry: {
        pdf: "./scripts/pdf/index.ts",
    },
    optimization: {
        splitChunks: {
            cacheGroups: {
                vendor: {
                    name: "pdf-vendors",
                },
            },
        },
    },
    plugins: [
        new copyPlugin({
            patterns: [
                {
                    from: path.resolve(__dirname, "../.././node_modules/pdfjs-dist/build/pdf.worker.min.js"),
                    to: path.resolve(__dirname, "../.././wwwroot/js"),
                    info: { minimized: true },
                },
            ],
        }),
    ],
};
