import { render, screen, waitFor } from '@testing-library/react'
import '@testing-library/jest-dom'
import { MemoryRouter, Routes, Route } from 'react-router-dom';
import Clients from '../../../pages/Clients';
import { mockClientData } from '../../../tests/unit-tests/mocks/apiMock';
import * as ApiModule from '../../../utils/api';
import { Console } from 'console';

jest.mock('../../../utils/api', () => ({
    fetchClient: jest.fn(),
  }));

describe('ClientPage', () => {
    beforeEach(() => {
        jest.clearAllMocks();
    });

    test("Clients_RenderingPage_RendersCorrect", () => {
        render(<Clients />);
    });

    // test('Clients_InsertParam_RendersWithCorrectId', () => {
    //     const clientId = '1';
    //     render(
          
    //       <MemoryRouter initialEntries={[`/Clients/${clientId}`]}>
    //         <Routes>
    //           <Route path='/Clients/:id' element={<Clients />} />
    //         </Routes>
    //       </MemoryRouter>
    //     );

    //     const { getByText } = render(<Clients />);
      
    //     expect(getByText(`Cliëntnummer: ${clientId}`)).toBeInTheDocument();
    //   });

    // test('renders loading state while fetching data', async () => {
    //   var sd  = jest.mock("react-router-dom", () => ({
    //   ...jest.requireActual("react-router-dom"),
    //   useParams: jest.fn(),
    //   }));

    //   jest.spyOn(ApiModule, 'fetchClient').mockResolvedValue(mockClientData);
  
    //   render(
    //     <MemoryRouter initialEntries={['/clients/1']}>
    //       <Routes>
    //         <Route path='/Clients/:id' element={<Clients />} />
    //       </Routes>
    //     </MemoryRouter>
    //   );
      

    //   console.log();
  
    //   expect(screen.getByText(/loading/i)).toBeInTheDocument();
    //   await waitFor(() => expect(sd).toHaveBeenCalledWith('1'));
    // });

    // test('renders client data after loading', async () => {
    //     jest.mock("react-router-dom", () => ({
    //         ...jest.requireActual("react-router-dom"),
    //         useParams: jest.fn(),
    //        }));

    //     jest.spyOn(ApiModule, 'fetchClient').mockResolvedValue(mockClientData);
      
    //     const { getByText } = render(<Clients />);
        
    //     // Wait for the data to be loaded
    //     await waitFor(() => {
    //       expect(getByText(`${mockClientData.id}`)).toBeInTheDocument();
    //     });
    //   });

    // test('ClientsPage_FetchesClientData_ShowsClientData', async () => {
    //     render(<Clients />);
    //     await waitFor(() => {
    //         const element = screen.getByText("<p>Cli�nt info</p>");
    //         expect(element).toBeInTheDocument();
    //     });
    // });
});