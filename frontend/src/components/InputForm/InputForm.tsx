import React, {useState} from "react";
import './InputForm.css'

interface InputFormProps {
    onSubmit: (command: string) => void;
    loading?: boolean;
    error?: string;
}

export const InputForm: React.FC<InputFormProps> = ({
        onSubmit,
        loading = false,
        error
    }: InputFormProps) => {
    const [command, setCommand] = useState("");

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        let trimCommand = command.trim();

        if (trimCommand) {
            onSubmit(trimCommand);
            setCommand('');
        }
    };


    return (
        <div className="input-form">
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

            <div>
                <p>Input Examples</p>
                <ul>
                    <li>Draw</li>
                </ul>
            </div>
        </div>
    );
};