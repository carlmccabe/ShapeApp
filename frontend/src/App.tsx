import React from 'react';
import {InputForm} from './components/InputForm';
import {ShapeCanvas} from './components/ShapeCanvas';
import {parseShape} from "./services/shapeApi";
import {ShapeData} from './types/shapes';
import './App.css';

function App() {
    const [shape, setShape] = React.useState<ShapeData | null>(null);
    const [loading, setLoading] = React.useState(false);
    const [error, setError] = React.useState<string | null>(null);

    const handleSubmit = async (command: string) => {
        setLoading(true);
        setError(null);
        setShape(null);

        try {
            let result = await parseShape(command);
            if (result.success && result.shape) {
                setShape(result.shape);
            } else {
                setError(result.errorMessage || 'Invalid command format');
            }
        } catch (error) {
            setError('An unexpected error occurred');
            console.error('Error parsing shape:', error);
        } finally {
            setLoading(false);
        }
    }

    // todo add better responsiveness
    return (
        <div className="app" data-testid="app">
            <div className="app__container">
                <div className="app__input">
                    <InputForm
                        onSubmit={handleSubmit}
                        loading={loading}
                        error={error ? error : undefined}
                    />
                </div>
                <div className="app__canvas">
                    <ShapeCanvas shape={shape} />
                </div>
            </div>
        </div>
    );
}

export default App;
