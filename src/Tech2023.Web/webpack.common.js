const path = require("path");
const webpack = require("webpack");
const copyPlugin = require("copy-webpack-plugin");

module.exports = {
    module: {
        rules: [
            {
                test: /\.ts?$/,
                use: "ts-loader",
                exclude: /node_modules/,
            },
        ],
    },
    resolve: {
        extensions: [".ts", ".js"],
    },
    output: {
        library: {
            name: "TS",
            type: "var",
        },
        filename: "[name].min.js",
        path: path.resolve(__dirname, "./wwwroot/js"),
        clean: true,
    },
    optimization: {
        moduleIds: "deterministic",
        runtimeChunk: "single",
        splitChunks: {
            cacheGroups: {
                vendor: {
                    test: /[\\/]node_modules[\\/]/,
                    name: "vendors",
                    chunks: "all",
                },
            },
        },
    },
    plugins: [
        new copyPlugin({
            patterns: [
                {
                    from: path.resolve(__dirname, "./node_modules/pdfjs-dist/build/pdf.worker.min.js"),
                    to: path.resolve(__dirname, "./wwwroot/js"),
                    info: { minimized: true },
                },
            ],
        }),
    ],
};
