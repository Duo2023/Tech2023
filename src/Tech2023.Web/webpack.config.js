const { merge } = require("webpack-merge");

// Base config:
const commonConfig = require("./webpack/webpack.common.js");
// Custom configs respective to the libraries:
const libconfigs = [
    require("./scripts/interactions/webpack.config.js") /*pdf-js-dist: , require("./scripts/pdf/webpack.config.js")*/,
];
// Prod and Dev specific configs to apply to previous configs:
const prodConfig = require("./webpack/webpack.prod.js");
const devConfig = require("./webpack/webpack.dev.js");

function setupConfigs(modeConfig) {
    let configs = libconfigs.map((lib) => {
        // Apply order: base <- lib <- mode (prod or dev):
        return merge(commonConfig, lib, modeConfig);
    });

    // Clean the output folder before compiling:
    configs[0] = merge(configs[0], { output: { clean: true } });

    return configs;
}

module.exports = (env, args) => {
    switch (args.mode) {
        case "production":
            return setupConfigs(prodConfig);
        case "development":
            return setupConfigs(devConfig);
        default:
            throw new Error(`Can't find matching Configuration for mode: ${args.mode}!`);
    }
};
