import React from "react";

interface CodeBlockProps {
    code: string;
}

export const CopyBlock: React.FC<CodeBlockProps> = ({code}) => {
    const [copied, setCopied] = React.useState(false);

    return (
        <div
            className="code-block"
            data-testid="code-block"
        >
            <div className="code-block__code">
                <pre className="code-block__pre">
                    <p>{code}</p>
                </pre>
            </div>
            <button
                type="button"
                className="code-block__button "
                aria-label="Copy example"
                onClick={async () => {
                    try {
                        await navigator.clipboard.writeText(code);
                        setCopied(true);
                        setTimeout(() => setCopied(false), 1500);
                    } catch {
                        // no-op: clipboard might be blocked
                    }
                }}
                data-testid="copy-button"
            >
                {copied ? 'Copied!' : 'Copy'}
            </button>
        </div>
    );
};