import { cleanup, fireEvent, render, screen, waitFor } from '@testing-library/react'
import '@testing-library/jest-dom'
import { MemoryRouter, Routes, Route } from 'react-router-dom';
import Clients from 'pages/Clients';
//import { mockClientData } from '../../../utils/mocks/api';
import * as ApiModule from 'utils/api';
import { Console } from 'console';
import ClientEditor from 'pages/client-editor';
import { deactivateClient } from 'utils/api';

// jest.mock('../../../utils/api', () => ({
//     fetchClient: jest.fn(),
//   }));

// jest.mock('utils/api.ts');

afterEach(cleanup);

// jest.mock('./utils/api', () => ({
//     ...jest.requireActual('./utils/api'), // behoud de werkelijke implementatie voor niet-geïmiteerde methoden
//     deactivateClient: jest.fn(), // mock deactivateClient
// }));

describe('ClientEditor', () => {

    test("ClientEditor_RenderingPage_RendersCorrect", () => {
        render(
            <MemoryRouter>
                <ClientEditor />
            </MemoryRouter>
        );
    });    
});