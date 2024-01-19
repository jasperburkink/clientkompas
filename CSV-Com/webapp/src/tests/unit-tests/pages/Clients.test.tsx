import { render, screen, waitFor } from '@testing-library/react'
import '@testing-library/jest-dom'
import { MemoryRouter, Route } from 'react-router-dom';
import Clients from '../../../pages/Clients';
import { mockClientData } from '../../../tests/unit-tests/mocks/apiMock';
import * as ApiModule from '../../../utils/api';

jest.mock('../../../utils/api', () => ({
    fetchClient: jest.fn(),
  }));

describe('ClientPage', () => {
    beforeEach(() => {
        jest.clearAllMocks();
    });

    test("ClientsPage_Renders_Successfully", () => {
        render(<Clients />);
    });

    test('renders with correct params', () => {
        const clientId = '1';
        render(
          <MemoryRouter initialEntries={[`/pages/Clients/${clientId}`]}>
            <Route path="/pages/Clients/:id">
              <Clients />
            </Route>
          </MemoryRouter>
        );
      
        expect(screen.getByText(`Client ${clientId}`)).toBeInTheDocument();
      });

    test('renders client data after loading', async () => {
        jest.mock("react-router-dom", () => ({
            ...jest.requireActual("react-router-dom"),
            useParams: jest.fn(),
           }));

        jest.spyOn(ApiModule, 'fetchClient').mockResolvedValue(mockClientData);
      
        const { getByText } = render(<Clients />);
        
        // Wait for the data to be loaded
        await waitFor(() => {
          expect(screen.getByText('')).toBeInTheDocument();
        });
      });

    test('ClientsPage_FetchesClientData_ShowsClientData', async () => {
        render(<Clients />);
        await waitFor(() => {
            const element = screen.getByText("<p>Cli�nt info</p>");
            expect(element).toBeInTheDocument();
        });
    });
});