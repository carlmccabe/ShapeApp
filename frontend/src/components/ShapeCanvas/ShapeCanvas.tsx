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
        if (!shape.centre) return null;

        let radius = shape.measurements.radius || 50;

        return (
            <circle
                data-testid="circle"
                cx={shape.centre.x}
                cy={shape.centre.y}
                r={radius}
                fill="none"
                stroke="magenta"
                strokeWidth="2"
            />
        );
    }

    const renderPolygon = () => {
        if (!shape.points || shape.points.length === 0) return null;

        const points = shape.points
            .map(point => `${point.x},${point.y}`)
            .join(' ');

        return (
            <polygon
                data-testid={shape.type.toLowerCase()}
                points={points}
                fill="none"
                stroke="magenta"
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
        // for circle shapes, use centre and radius
        if (shape.centre && (shape.type === 'Circle' || shape.type === 'Oval')) {
            const radius = shape.measurements.radius || Math.max(shape.measurements['width']) / 2;
            const padding = 5;
            const size = (radius * 2) + (padding * 2);
            const offsetX = shape.centre.x - radius - padding;
            const offsetY = shape.centre.y - radius - padding;
            return `${offsetX} ${offsetY} ${size} ${size}`;
        } else {

            // for polygon shapes, use the bounding box
            //todo align shape better
            if (shape.points && shape.points.length > 0) {
                const xValues = shape.points.map(point => point.x);
                const yValues = shape.points.map(point => point.y);

                const minX = Math.min(...xValues);
                const maxX = Math.max(...xValues);

                const minY = Math.min(...yValues);
                const maxY = Math.max(...yValues);

                const padding = 20;
                const size = Math.max(maxX - minX, maxY - minY) + padding;
                const offsetX = (minX + maxX) / 2 - size / 2;
                const offsetY = (minY + maxY) / 2 - size / 2;

                return `${offsetX} ${offsetY} ${size} ${size}`;
            }
        }
        return "0 0 300 300"
    };

    const renderShapeInfo = () => (
        <div className="shape-canvas__info">
            <h3>{shape.type}</h3>
            <div className="shape-canvas__dimensions">
                {Object.entries(shape.measurements).map(([key, value]) => (
                    <span style={{marginRight: '5px'}} key={key} className="shape-canvas__dimension">
            {key}: {value} 
          </span> 
                ))}
            </div>
        </div>
    );
    
    return (
        <div
            className="shape-canvas"
            data-testid="shape-canvas"
        >
            <h2>Generated Shape</h2>
            {/*Todo add clear button*/}
            {renderShapeInfo()}
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