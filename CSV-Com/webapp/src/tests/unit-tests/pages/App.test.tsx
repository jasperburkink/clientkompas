import React from 'react';
import { render, screen } from '@testing-library/react';
import App from 'pages/App';
import '@testing-library/jest-dom'

test('renders components header', () => {
    render(<App />);
    const linkElement = screen.getByText('Dit zijn alle beschikbare componenten.');
    expect(linkElement).toBeInTheDocument();
});
