const path = require("path");

module.exports = {
    entry: "./scripts/index.ts",
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
        filename: "[name].min.js", // for more info: https://webpack.js.org/guides/caching/
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
