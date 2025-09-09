module.exports = {
    jest: {
        configure: (config) => {
            config.setupFiles = [
                ...(config.setupFiles || []),
                '<rootDir>/src/jest.polyfills.ts',
            ];

            // for msw
            config.transformIgnorePatterns = ['node_modules/(?!(msw)/)'];

            // Force axios to use the CJS build. for jest
            config.moduleNameMapper = {
                ...(config.moduleNameMapper || {}),
                '^axios$': 'axios/index.cjs',
            };

            return config;
        },
    },
};