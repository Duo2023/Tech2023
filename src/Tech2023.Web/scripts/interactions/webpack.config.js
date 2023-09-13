module.exports = {
    entry: {
        interactions: "./scripts/interactions/index.ts",
    },
    optimization: {
        splitChunks: {
            cacheGroups: {
                vendor: {
                    name: "interactions-vendors",
                },
            },
        },
    },
};
