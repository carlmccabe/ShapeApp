import '@testing-library/jest-dom';
import {server} from '../mocks/server';

// Mock Canvas for SVG testing
Object.defineProperty(HTMLCanvasElement.prototype, 'getContext', {
    value: null
});


const { TextEncoder, TextDecoder } = require('util');
global.TextEncoder = TextEncoder;
global.TextDecoder = TextDecoder;

let clipboardText = '';

Object.defineProperty(navigator, 'clipboard', {
    value: {
        readText: () => clipboardText,
        writeText: (text: string) => clipboardText = text
    }
});

// Setup MSW
beforeAll(() => server.listen());
afterEach(() => server.resetHandlers());
afterAll(() => server.close());
