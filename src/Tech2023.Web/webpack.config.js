const { mergeWithCustomize, customizeObject } = require("webpack-merge");
const commonConfig = require("./webpack.common.js");
const prodConfig = require("./webpack.prod.js");
const devConfig = require("./webpack.dev.js");

module.exports = (env, args) => {
    switch (args.mode) {
        case "production":
            return mergeWithCustomize({ customizeObject: customizeObject({ entry: "prepend" }) })(
                commonConfig,
                prodConfig
            );
        case "development":
            return mergeWithCustomize({ customizeObject: customizeObject({ entry: "prepend" }) })(
                commonConfig,
                devConfig
            );
        default:
            throw new Error(`Can't find matching Configuration for mode: ${args.mode}!`);
    }
};
