import '@testing-library/jest-dom';
import { server } from './mocks/server';

// Mock Canvas for SVG testing
Object.defineProperty(HTMLCanvasElement.prototype, 'getContext', {
    value: jest.fn(),
});

// Setup MSW
beforeAll(() => server.listen());
afterEach(() => server.resetHandlers());
afterAll(() => server.close());