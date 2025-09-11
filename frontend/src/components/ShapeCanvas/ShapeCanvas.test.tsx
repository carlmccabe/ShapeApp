import { render, screen } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import { ShapeCanvas } from './ShapeCanvas';
import { ShapeData} from "../../types/shapes";

describe('ShapeCanvas', () => {
    const circleShape: ShapeData ={
        type: 'Circle',
        measurements: {
            radius: 100
        },
        points: [],
        centre: { x: 100, y: 100 }
    };
    
    const squareShape: ShapeData ={
        type: 'Square',
        measurements: {
            sideLength: 100
        },
        points: [
            { x: 0, y: 0 },
            { x: 100, y: 0 },
            { x: 100, y: 100 },
            { x: 0, y: 100 }
        ],
    };
    
    it('should render empty canvas when no shapes are provided', () => {
        render(<ShapeCanvas />);
        const shapeCanvasElement = screen.getByTestId('shape-canvas');
        expect(shapeCanvasElement).toBeInTheDocument();
        expect(shapeCanvasElement).toHaveTextContent('Enter a shape');
    })
    
    it('renders the circle shape', () => {
        render(<ShapeCanvas shape={circleShape} />);
        const circleElement = screen.getByTestId('circle');
        expect(circleElement).toBeInTheDocument();
    })
    
    it('should render SVG container when shape is provided', () => {
        render(<ShapeCanvas shape={circleShape} />);
        const svgElement = screen.getByTestId(/svg-container/);
        expect(svgElement).toBeInTheDocument();
    })
    
    it('it should render circle with correct attributes', () => {
        render(<ShapeCanvas shape={circleShape} />);
        const circleElement = screen.getByTestId('circle');
        expect(circleElement).toHaveAttribute('cx', '100');
        expect(circleElement).toHaveAttribute('cy', '100');
        expect(circleElement).toHaveAttribute('r', '100');
    })
    
    it('renders the square shape', () => {
        render(<ShapeCanvas shape={squareShape} />);
        const squareElement = screen.getByTestId('square');
        expect(squareElement).toBeInTheDocument();
    })
    
    it('should have accessibility attributes', () => {
        render(<ShapeCanvas shape={circleShape} />);

        const svg = screen.getByRole('img');
        expect(svg).toHaveAttribute('aria-label');
        expect(svg.getAttribute('aria-label')).toContain('Circle');
    });
});
