import React from 'react';
import { render, screen } from '@testing-library/react';
import App from './App';

test('renders learn react link', () => {
  render(<App />);
  const linkElement = screen.getByText(/learn react/i);
  expect(linkElement).toBeInTheDocument();
});

describe('App', () => { 
    it('renders the App component', () => { 
        render(<App />);
        const appElement = screen.getByTestId('app');
        expect(appElement).toBeInTheDocument();
        expect(screen.getByText(/Shape Generator/i)).toBeInTheDocument();
    });
    
    it('renders the InputForm component', () => { 
        render(<App />);
        const inputFormElement = screen.getByTestId('input-form');
        expect(inputFormElement).toBeInTheDocument();
        expect(screen.getByPlaceholderText(/Enter command/i)).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /Generate/i })).toBeInTheDocument();
    })
});