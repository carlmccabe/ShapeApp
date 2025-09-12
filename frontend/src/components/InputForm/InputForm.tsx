import React, {useState} from "react";
import './InputForm.css'
import {CopyBlock} from "../CopyBlock";

interface InputFormProps {
    onSubmit: (command: string) => void;
    loading?: boolean;
    error?: string;
}

let examples = [
    "Draw a circle with a radius of 100",
    "Draw a square with a side length of 200",
    "Draw a rectangle with a width of 250 and a height of 400",
    "Draw an octagon with a side length of 200",
    "Draw an isosceles triangle with a height of 200 and a width of 100"
]

export const InputForm: React.FC<InputFormProps> = ({
                                                        onSubmit,
                                                        loading = false,
                                                        error
                                                    }: InputFormProps) => {
    const [command, setCommand] = useState("");
    const [showExamples, setShowExamples] = useState(false);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        let trimCommand = command.trim();

        if (trimCommand) {
            onSubmit(trimCommand);
            setCommand('');
        }
    };


    return (
        <div className="input-form" data-testid="input-form">
            <h1>Shape Generator</h1>
            <p>Enter a command to generate the shape</p>

            <form onSubmit={handleSubmit}>
                <div>
                    <input
                        type="text"
                        placeholder="Enter command to generate the shape"
                        value={command}
                        onChange={(e) => setCommand(e.target.value)}
                        disabled={loading}
                        className="input"
                    />
                    <button
                        disabled={loading}
                        role="button"
                        name="generate"
                        aria-label="Generate"
                        aria-disabled={loading}
                        className="button" type="submit">
                        {loading ? 'Generating shape...' : "Generate shape"}
                    </button>
                </div>
                {error && (
                    <div role="alert" className="alert alert-danger">
                        {error}
                    </div>
                )}
            </form>

            <div className="examples">
                <h4>Input Examples</h4>
                <button className="button" onClick={() => setShowExamples(!showExamples)}>
                    {showExamples ? 'Hide Examples' : 'Show Examples'}
                </button>
                <div
                    id="examples-content"
                    className={`examples__content ${showExamples ? 'examples__content--open' : ''}`}
                    role="region"
                    aria-label="Example inputs"
                >
                    {examples.map((example, index) => (
                        <CopyBlock code={example} key={index}/>
                    ))}
                </div>
            </div>
        </div>
    );
};