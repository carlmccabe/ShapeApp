export interface Point {
    x: number;
    y: number;
}

export interface ShapeData {
    type: string;
    measurements: Record<string ,number>;
    points: Point[];
    centre?: Point;
}

export interface ParseShapeRequest {
    commands: string;
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