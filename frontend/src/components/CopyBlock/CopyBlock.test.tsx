import React from "react";
import { render } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, it, expect } from "vitest";
import { CopyBlock } from "./CopyBlock";

describe('CopyBlock', () => {

    it('should render the code block', () => {
        const {getByTestId} = render(<CopyBlock code="Draw"/>);
        expect(getByTestId('code-block')).toBeInTheDocument();
    })

    it('should copy the code', async () => {
        const {getByTestId} = render(<CopyBlock code="Draw"/>);
        const button = getByTestId('copy-button');
        await userEvent.click(button);
        expect(await navigator.clipboard.readText()).toBe('Draw');
    });
});
