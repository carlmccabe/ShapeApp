import React from "react";
import {ShapeData} from "../../types/shapes";
import './ShapeCanvas.css'

interface ShapeCanvasProps {
    shape?: ShapeData
}

export const ShapeCanvas: React.FC<ShapeCanvasProps> = ({shape}: ShapeCanvasProps) => {
    if (!shape) {
        return (
            <div className="shape-canvas__empty" data-testid="shape-canvas">
                <p>Enter a shape</p>
            </div>
        )
    }

    const renderCircle = () => {
        let radius = shape.measurements.radius || 50;
        return (
            <circle
                data-testid="circle"
                cx={shape.centre?.x || 50}
                cy={shape.centre?.y || 50}
                r={radius}
                fill="none"
                stroke="black"
                strokeWidth="2"
            />
        );
    }

    const renderPolygon = () => {
        return (
            <polygon
                data-testid={shape.type.toLowerCase()}
                points={shape.points?.join(' ') || "100,10 40,180 190,60"}
                fill="none"
                stroke="black"
                strokeWidth="2"
            />
        );
    }

    const renderUnsupported = () => {
        return (
            <text x="50%" y="50%" textAnchor="middle" fill="black">Unsupported Shape: {shape.type}</text>
        );
    }

    const renderShape = () => {
        if (shape.type === 'Circle') {
            return renderCircle();
        }
        if (shape.points && shape.points.length > 0) {
            return renderPolygon();
        } else {
            return renderUnsupported();
        }
    }

    return (
        <div
            className="shape-canvas"
            data-testid="shape-canvas"
        >
            <div className="shape-canvas__svg-container" data-testid="svg-container">
                <svg
                    role="img"
                    aria-label={`Generated ${shape.type} shape`}
                    viewBox="0 0 300 300"
                    className="shape-canvas__svg" 
                    data-testid="svg"
                >
                    {renderShape()}
                </svg>
            </div>
        </div>
    );
}