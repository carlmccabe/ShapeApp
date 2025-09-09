// shapes.ts

// Define the types for the shape data

// Define the type for a point
export interface Point {
    x: number;
    y: number;
}

// Define the type for the shape data
export interface ShapeData {
    type: string;
    measurements: Record<string ,number>;
    points: Point[];
    centre?: Point;
}


// Define the types for the API requests and responses
export interface ParseShapeRequest {
    command: string;
}

export interface ParseShapeResponse {
    success: boolean;
    errorMessages?: string;
    shape?: ShapeData;
}

export interface ApiError {
    message: string;
    statusCode: number;
}