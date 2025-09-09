import {parseShape} from './shapeApi';
import {server} from '../../mocks/server';
import {http, HttpResponse} from 'msw';

describe('shapeApi', () => {
    it('should call the API and return the shape data for a circle', async () => {
        // Arrange
        const commands = "Draw a circle with a radius of 100";

        // Act
        const response = await parseShape(commands);

        // Assert
        expect(response.success).toBe(true);
        expect(response.shape?.type).toBe("Circle");
        expect(response.shape?.measurements.radius).toBe(100);
        expect(response.shape?.centre).toEqual({x: 100, y: 100})
    });

    it('should call the API and return the shape data fro a square', async () => {
        // Arrange
        const commands = "Draw a square with a side length of 100";

        // Act
        const response = await parseShape(commands);

        // Assert
        expect(response.success).toBe(true);
        expect(response.shape?.type).toBe("Square");
        expect(response.shape?.measurements.sideLength).toBe(100);
        expect(response.shape?.centre).toEqual({x: 100, y: 100})
    });

    it('should handle API errors gracefully', async () => {
        // Arrange
        server.use(
            http.post('/shape/parse', async ({request}) => {
                return new Response(JSON.stringify({
                        success: false,
                        errorMessages: 'Invalid command format'
                    }), {
                        status: 400,
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    }
                )
            })
        )

        // Act
        const result = await parseShape('Invalid command format');

        // Assert
        expect(result.success).toBe(false);
        expect(result.errorMessages).toBe('Invalid command format');
    })

    it('should handle network errors', async () => {
        // Arrange
        server.use(
            http.post('/shape/parse', async (res) => {
                return HttpResponse.error();
            })
        )

        // Act
        const result = await parseShape('Draw a square with a side length of 100');

        // Assert
        expect(result.success).toBe(false);
        expect(result.errorMessages).toBe('Network error');
    })

    it('should handle server errors', async () => {
        // Arrange
        server.use(
            http.post('/shape/parse', async (res) => {
                return HttpResponse.json({
                    success: false,
                    errorMessages: 'Internal server error'
                }, {
                    status: 500
                })
            })
        )

        // Act
        const result = await parseShape('Draw a square with a side length of -100');

        // Assert
        expect(result.success).toBe(false);
        expect(result.errorMessages).toBe('Internal server error');
    })
    
});