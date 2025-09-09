import '@testing-library/jest-dom';
import {server} from '../mocks/server';

// import { TextEncoder, TextDecoder } from 'util';
// (global as any).TextEncoder ??= TextEncoder;
// (global as any).TextDecoder ??= TextDecoder as unknown as typeof global.TextDecoder;

// Mock Canvas for SVG testing
Object.defineProperty(HTMLCanvasElement.prototype, 'getContext', {
    value: jest.fn(),
});

// Setup MSW
beforeAll(() => server.listen());
afterEach(() => server.resetHandlers());
afterAll(() => server.close());