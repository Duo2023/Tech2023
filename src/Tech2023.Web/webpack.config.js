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
    filename: "app-bundle.js",
    path: path.resolve(__dirname, "./wwwroot/js"),
  },
};
