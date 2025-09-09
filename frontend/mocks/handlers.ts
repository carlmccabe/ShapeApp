import { http } from 'msw';
import {ParseShapeRequest, ParseShapeResponse} from "../src/types/shapes";

export const handlers = [
    http.post< {command: string}, ParseShapeRequest >('/shape/parse', async ({request}) => {
       const newReq = await request.clone().json()
        console.log(newReq)
        
        if (newReq.command.toLowerCase().includes('circle')) {
            const response: ParseShapeResponse = {
                success: true,
                shape: {
                    type: 'circle',
                    measurements: {
                        radius: 100
                    },
                    points: [
                        {
                            x: 100,
                            y: 100
                        }
                    ]
                }
            };
            return new Response(JSON.stringify(response), {
                status: 200,
                headers: {
                    'Content-Type': 'application/json'
                }
            })
        }
        
        if (newReq.command.toLowerCase().includes('square')) {
            const response: ParseShapeResponse = {
                success: true,
                shape: {
                    type: 'square',
                    measurements: {
                        side: 100
                    },
                    points: []
                }
            }
            return new Response(JSON.stringify(response), {
                status: 200,
                headers: {
                    'Content-Type': 'application/json'
                }
            })
        }
        
        if (newReq.command.toLowerCase().includes('rectangle')) {
            const response: ParseShapeResponse = {
                success: true,
                shape: {
                    type: 'rectangle',
                    measurements: {
                        width: 100,
                        height: 100
                    },
                    points: []
                }
            }
            return new Response(JSON.stringify(response), {
                status: 200,
                headers: {
                    'Content-Type': 'application/json'
                }
            }) 
        }
        
        const errorResponse: ParseShapeResponse = {
            success: false,
            errorMessages: 'Invalid command format'
        }
        return new Response(JSON.stringify(errorResponse), {
            status: 400,
            headers: {
                'Content-Type': 'application/json'
            }
        })
    })
]