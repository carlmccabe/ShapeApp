import React from 'react';
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { describe, it, expect, beforeEach, vi } from 'vitest';
import { InputForm } from './InputForm';

describe('InputForm', () => {
    const mockOnSubmit = vi.fn();

    beforeEach(() => {
        vi.clearAllMocks();
    });

    it('should render input and button', async () => {
        render(<InputForm onSubmit={mockOnSubmit} loading={false} />);

        expect(screen.getByPlaceholderText(/enter command/i)).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /generate/i })).toBeInTheDocument();
    });

    it('should call onSubmit when form is submitted with valid command', async () => {
        const user = userEvent.setup();
        const command = 'Draw a circle with a radius of 100';

        render(<InputForm onSubmit={mockOnSubmit} />);

        await user.type(screen.getByPlaceholderText(/enter command/i), command);
        await user.click(screen.getByRole('button', { name: /generate/i }));

        expect(mockOnSubmit).toHaveBeenCalledWith(command);
    });

    it('should prevent submission of empty command', async () => {
        const user = userEvent.setup();

        render(<InputForm onSubmit={mockOnSubmit} />);

        await user.click(screen.getByRole('button', { name: /Generate/ }));

        expect(mockOnSubmit).not.toHaveBeenCalled();
    });

    it('should show loading state when loading prop is true', () => {
        render(<InputForm onSubmit={mockOnSubmit} loading={true} />);

        const button = screen.getByRole('button');
        expect(button).toBeDisabled();
        expect(button).toHaveTextContent(/Generating/i);
    });

    it('should display error message when error prop is provided', () => {
        const error = 'Invalid command format';

        render(<InputForm onSubmit={mockOnSubmit} error={error} />);

        const alert = screen.getByRole('alert');
        expect(alert).toHaveTextContent(error);
    });
});