/** @type {import('jest').Config} */
const config = {
  verbose: true,
  moduleNameMapper: {
    '^utils/api$': '<rootDir>/src/utils/__mocks__/api.ts',
  },
};

module.exports = config;