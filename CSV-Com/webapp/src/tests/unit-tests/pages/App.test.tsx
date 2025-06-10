import React from 'react';
import { render, screen } from '@testing-library/react';
import App from 'pages/App';
import '@testing-library/jest-dom'
import { MemoryRouter } from 'react-router-dom';

test('renders components header', () => {
    render(
        <MemoryRouter>
            <App />
        </MemoryRouter>
    );
    const linkElement = screen.getByText('Dit zijn alle beschikbare componenten.');
    expect(linkElement).toBeInTheDocument();
});
