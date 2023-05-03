const path = require("path");
const webpack = require('webpack');

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
    plugins: [
        new webpack.DefinePlugin({
            API_BASE_URL: "'https://localhost:7098/api'"
        })
    ],
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
};
