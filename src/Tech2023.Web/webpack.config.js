const { merge } = require("webpack-merge");
const commonConfig = require("./webpack/webpack.common.js");
const libconfigs = [require("./scripts/interactions/webpack.config.js"), require("./scripts/pdf/webpack.config.js")];
const prodConfig = require("./webpack/webpack.prod.js");
const devConfig = require("./webpack/webpack.dev.js");

function setupConfigs(modeConfig) {
    let configs = libconfigs.map((lib) => {
        return merge(commonConfig, lib, modeConfig);
    });

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
