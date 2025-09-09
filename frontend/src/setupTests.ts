import '@testing-library/jest-dom';
import {server} from '../mocks/server';

// Mock Canvas for SVG testing
Object.defineProperty(HTMLCanvasElement.prototype, 'getContext', {
    value: null
});


const { TextEncoder, TextDecoder } = require('util');
global.TextEncoder = TextEncoder;
global.TextDecoder = TextDecoder;

// Setup MSW
beforeAll(() => server.listen());
afterEach(() => server.resetHandlers());
afterAll(() => server.close());
