import {http, HttpResponse} from 'msw';
import {ParseShapeRequest, ParseShapeResponse} from "../src/types/shapes";

export const handlers = [
    http.post<{command: string}, ParseShapeRequest>('http://localhost:5010/shape/parse', async ({request}) => {
        const newReq = await request.json()
        console.log('MSW intercepted request:', newReq)
        
        // Handle network error test case
        if (newReq.command.toLowerCase().includes('network error')) {
            return HttpResponse.error()
        }


        // Handle specific test cases that expect exact error messages
        if (newReq.command === 'Invalid command format') {
            const errorResponse: ParseShapeResponse = {
                success: false,
                errorMessage: 'Invalid command format'
            }
            return HttpResponse.json(errorResponse, { status: 400 })
        }

        // handle the different shape types
        if (newReq.command.toLowerCase().includes('circle')) {
            const response: ParseShapeResponse = {
                success: true,
                shape: {
                    type: 'Circle',
                    measurements: {
                        radius: 100
                    },
                    points: [],
                    centre: { x: 100, y: 100 }
                }
            };
            return HttpResponse.json(response)
        }
        
        if (newReq.command.toLowerCase().includes('square')) {
            const response: ParseShapeResponse = {
                success: true,
                shape: {
                    type: 'Square',
                    measurements: {
                        'side length': 100
                    },
                    points: [
                        { x: 0, y: 0 },
                        { x: 100, y: 0 },
                        { x: 100, y: 100 },
                        { x: 0, y: 100 }
                    ]
                }
            }
            return HttpResponse.json(response)
        }
        
        if (newReq.command.toLowerCase().includes('rectangle')) {
            const response: ParseShapeResponse = {
                success: true,
                shape: {
                    type: 'rectangle',
                    measurements: {
                        'width': 100,
                        'height': 200
                    },
                    points: [
                        { x: 0, y: 0 },
                        { x: 100, y: 0 },
                        { x: 100, y: 100 },
                        { x: 0, y: 100 }
                    ]
                }
            }
            return HttpResponse.json(response)
        }
        
        const errorResponse: ParseShapeResponse = {
            success: false,
            errorMessage: 'Invalid command format'
        }
        
        return HttpResponse.json(errorResponse, {status: 400})
    })
]