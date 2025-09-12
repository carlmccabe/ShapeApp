import React from "react";
import {ShapeData} from "../../types/shapes";
import './ShapeCanvas.css'

interface ShapeCanvasProps {
    shape?: ShapeData | null;
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

    const calculateViewBox = () => {
        // for larger shapes, we can use the bounding box
        //todo align shape better
        if (shape.points && shape.points.length > 0) {
            const xValues = shape.points.map(point => point.x);
            const yValues = shape.points.map(point => point.y);
            const minX = Math.min(...xValues);
            const maxX = Math.max(...xValues);
            const minY = Math.min(...yValues);
            const maxY = Math.max(...yValues);
            return `${minX} ${minY} ${maxX - minX} ${maxY - minY}`;
            
        }
        return "0 0 300 300"

    };
    return (
        <div
            className="shape-canvas"
            data-testid="shape-canvas"
        >
            <h2>Generated Shape</h2>
            {/*Todo add clear button*/}
            <div className="shape-canvas__svg-container" data-testid="svg-container">
                <svg
                    role="img"
                    aria-label={`Generated ${shape.type} shape`}
                    viewBox={calculateViewBox()}
                    className="shape-canvas__svg" 
                    data-testid="svg"
                >
                    {renderShape()}
                </svg>
            </div>
        </div>
    );
}