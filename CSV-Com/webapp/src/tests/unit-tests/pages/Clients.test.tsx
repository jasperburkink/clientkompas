import { render, screen, waitFor, fireEvent } from '@testing-library/react'
// import '@testing-library/jest-dom'
import { MemoryRouter, Routes, Route } from 'react-router-dom';
import Clients from 'pages/Clients';
import * as api from 'utils/api';

jest.mock('utils/api'); // Mock de hele api module

describe('ClientPage', () => {
    beforeEach(() => {
        jest.setTimeout(30000); // Stel een langere time-out in voor de test        
        jest.clearAllMocks();
    });
/*
    test("Clients_RenderingPage_RendersCorrect", () => {
        render(
            
            <MemoryRouter>
                <Clients />
            </MemoryRouter>
        );
    });

    test('fetchClient is called with correct client ID', async () => {
        const clientId = '1'; // Definieer een testclient ID
    
        // Render de Clients-component (of een ander component waarin fetchClient wordt aangeroepen)
        const route = `/clients/${clientId}`;

        render(
            <MemoryRouter initialEntries={[route]}>
                <Routes>
                    <Route path="/clients/:id" element={<Clients />} />
                </Routes>
            </MemoryRouter>
        );
    
        // Wacht tot de fetchClient-functie wordt aangeroepen
        await waitFor(() => {
          // Controleer of fetchClient is aangeroepen met de juiste client ID
          expect(api.fetchClient).toHaveBeenCalledWith(clientId);
        });
    });
*/
//TODO: Onderstaande tests doorspreken met Maurice. Hoe mock je de calls naar de database? 

    // test('Clients_InsertParam_RendersWithCorrectId', async () => {
    //     const clientId = '1';
        
    //     render(
    //       <MemoryRouter initialEntries={[`/Clients/${clientId}`]}>
    //         <Routes>
    //           <Route path='/Clients/:id' element={<Clients />} />
    //         </Routes>
    //       </MemoryRouter>
    //     );

    //     await waitFor(() => {
    //         expect(screen.getByTestId('client-number-value')).toBeInTheDocument();            
    //       });
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

    test.skip('Deactivate_ClickDeactivateButton_ShowsConfirm', async () => {        

        const mockClient = {
            id: 1,
            firstname: 'John',
            initials: 'J.H',
            gender: 'Female',
            lastname: 'Doe',
            streetname: 'Main St',
            housenumber: '123',
            postalcode: '12345',
            residence: 'City',
            telephonenumber: '1234567890',
            emailaddress: 'john.doe@example.com',
            isintargetgroupregister: false,
            dateofbirth: new Date('1990-01-01'),
            emergencypeople: [],
            diagnoses: 'None',
            maritalstatus: 'Single',
            driverslicences: 'B',
            benefitform: 'None',
            workingcontracts: [],
            remarks: 'No remarks'
        };

        const mockFetchClient = api.fetchClient as jest.MockedFunction<typeof api.fetchClient>;
        mockFetchClient.mockResolvedValue(mockClient); //TODO: kijken of dit via een mock api klasse kan

        const clientId = '1';
        const route = `/clients/${clientId}`;

        await render(
            <MemoryRouter initialEntries={[route]}>
                <Routes>
                    <Route path="/clients/:id" element={<Clients />} />
                </Routes>
            </MemoryRouter>
        );        

        await waitFor(() => expect(api.fetchClient).toHaveBeenCalledWith(clientId));        

        screen.debug();

        await waitFor(() => {
            screen.getByTestId('deactivate-button');


            const spinner = screen.queryByTestId('clients-spinner');
            expect(spinner).not.toBeInTheDocument();
        }, { timeout: 10000 });

        screen.debug(undefined, Infinity, { highlight: false });

        // await waitFor(() => {
        //     expect(screen.getByText(mockClient.firstname)).toBeInTheDocument();
        //     expect(mockFetchClient).toHaveBeenCalledWith(clientId);
        //     const deactivateButton = screen.getByText('Deactivateer');
        //     expect(deactivateButton).toBeInTheDocument();      
        //   });

        const deactivateButton = screen.getByText('Deactivateer');

        // const deactivateButton = screen.getByTestId('deactivate-button');
        expect(deactivateButton).toBeInTheDocument();

        // Simuleer deactiveren van de cliënt
        fireEvent.click(deactivateButton);

        const confirmPopup = screen.getByTestId('confirm-popup');
        expect(confirmPopup).toBeInTheDocument();

        const isConfirmPopupOneButtonOpen = confirmPopup.getAttribute('data-isOpen');
        expect(confirmPopup.textContent).toBe('Cliënt succesvol gedeactiveerd');
        expect(isConfirmPopupOneButtonOpen).toBe('true');
        expect(confirmPopup.textContent).toMatchSnapshot();
    });

    test.skip('SearchClients_FillInSearchTerm_ShowsClients', async () => {        

        jest.setTimeout(50000); // Increase timeout to 10 seconds

        const mockClients = [
            {
                id: 1,
                firstname: 'John',
                initials: 'J.H',
                gender: 'Female',
                lastname: 'Doe',
                streetname: 'Main St',
                housenumber: '123',
                postalcode: '12345',
                residence: 'City',
                telephonenumber: '1234567890',
                emailaddress: 'john.doe@example.com',
                isintargetgroupregister: false,
                dateofbirth: new Date('1990-01-01'),
                emergencypeople: [
                ],
                diagnoses: 'None',
                maritalstatus: 'Single',
                driverslicences: 'B',
                benefitform: 'None',
                workingcontracts: [                    
                ],
                remarks: 'No remarks'
            },
            {
                id: 2,
                firstname: 'Jane',
                initials: 'J.E',
                gender: 'Female',
                lastname: 'Smith',
                streetname: 'Second St',
                housenumber: '456',
                postalcode: '67890',
                residence: 'Townsville',
                telephonenumber: '0987654321',
                emailaddress: 'jane.smith@example.com',
                isintargetgroupregister: false,
                dateofbirth: new Date('1985-05-15'),
                emergencypeople: [
                ],
                diagnoses: 'Diabetes',
                maritalstatus: 'Married',
                driverslicences: 'B, C',
                benefitform: 'Social Security',
                workingcontracts: [
                ],
                remarks: 'Requires insulin'
            },
            {
                id: 3,
                firstname: 'Michael',
                initials: 'M.T',
                gender: 'Male',
                lastname: 'Johnson',
                streetname: 'Third Ave',
                housenumber: '789',
                postalcode: '11223',
                residence: 'Village',
                telephonenumber: '1231231234',
                emailaddress: 'michael.johnson@example.com',
                isintargetgroupregister: true,
                dateofbirth: new Date('1975-03-20'),
                emergencypeople: [
                ],
                diagnoses: 'Hypertension',
                maritalstatus: 'Married',
                driverslicences: 'A, B',
                benefitform: 'Unemployment',
                workingcontracts: [
                ],
                remarks: 'Under medication'
            },
            {
                id: 4,
                firstname: 'Emily',
                initials: 'E.A',
                gender: 'Female',
                lastname: 'Davis',
                streetname: 'Fourth Blvd',
                housenumber: '321',
                postalcode: '44556',
                residence: 'Metropolis',
                telephonenumber: '3213214321',
                emailaddress: 'emily.davis@example.com',
                isintargetgroupregister: true,
                dateofbirth: new Date('1992-07-12'),
                emergencypeople: [
                ],
                diagnoses: 'Asthma',
                maritalstatus: 'Single',
                driverslicences: 'None',
                benefitform: 'Disability',
                workingcontracts: [
                ],
                remarks: 'Carries inhaler'
            },
            {
                id: 5,
                firstname: 'Robert',
                initials: 'R.K',
                gender: 'Male',
                lastname: 'Brown',
                streetname: 'Fifth St',
                housenumber: '654',
                postalcode: '77889',
                residence: 'Hamlet',
                telephonenumber: '4324325432',
                emailaddress: 'robert.brown@example.com',
                isintargetgroupregister: true,
                dateofbirth: new Date('1980-11-30'),
                emergencypeople: [
                ],
                diagnoses: 'None',
                maritalstatus: 'Divorced',
                driverslicences: 'B, D',
                benefitform: 'Retirement',
                workingcontracts: [
                ],
                remarks: 'No remarks'
            }
        ];
        
        const mockSearchClients = api.searchClients as jest.MockedFunction<typeof api.searchClients>;
        mockSearchClients.mockResolvedValue(mockClients); //TODO: kijken of dit via een mock api klasse kan

        const route = `/clients/`;

        await render(
            <MemoryRouter initialEntries={[route]}>
                <Routes>
                    <Route path="/clients/" element={<Clients />} />
                </Routes>
            </MemoryRouter>
        );      
        
        await waitFor(() => {
            screen.getByTestId('search-term');
        }, { timeout: 50000 });

        const searchTermInput = screen.getByTestId('search-term');

        fireEvent.change(searchTermInput, { target: { value: 'John Doe' } });

        await waitFor(() => {
            // Assuming the Clients component displays a list of clients based on the search term
            const clients = screen.getAllByTestId('results-item'); // Adjust the test id based on your implementation
            expect(clients.length).toBeGreaterThan(0); // Make sure at least one client is displayed
        });
    });

    test.skip('Deactivate_ClickDeactivateButton_ShowsConfirm', async () => {        

        const mockClient = {
            id: 1,
            firstname: 'John',
            initials: 'J.H',
            gender: 'Female',
            lastname: 'Doe',
            streetname: 'Main St',
            housenumber: '123',
            postalcode: '12345',
            residence: 'City',
            telephonenumber: '1234567890',
            emailaddress: 'john.doe@example.com',
            isintargetgroupregister: false,
            dateofbirth: new Date('1990-01-01'),
            emergencypeople: [],
            diagnoses: 'None',
            maritalstatus: 'Single',
            driverslicences: 'B',
            benefitform: 'None',
            workingcontracts: [],
            remarks: 'No remarks'
        };

        const mockFetchClient = api.fetchClient as jest.MockedFunction<typeof api.fetchClient>;
        mockFetchClient.mockResolvedValue(mockClient); //TODO: kijken of dit via een mock api klasse kan

        const clientId = '1';
        const route = `/clients/${clientId}`;

        await render(
            <MemoryRouter initialEntries={[route]}>
                <Routes>
                    <Route path="/clients/:id" element={<Clients />} />
                </Routes>
            </MemoryRouter>
        );        

        await waitFor(() => expect(api.fetchClient).toHaveBeenCalledWith(clientId));        

        screen.debug();

        await waitFor(() => {
            screen.getByTestId('deactivate-button');


            const spinner = screen.queryByTestId('clients-spinner');
            expect(spinner).not.toBeInTheDocument();
        }, { timeout: 10000 });

        screen.debug(undefined, Infinity, { highlight: false });

        // await waitFor(() => {
        //     expect(screen.getByText(mockClient.firstname)).toBeInTheDocument();
        //     expect(mockFetchClient).toHaveBeenCalledWith(clientId);
        //     const deactivateButton = screen.getByText('Deactivateer');
        //     expect(deactivateButton).toBeInTheDocument();      
        //   });

          const deactivateButton = screen.getByText('Deactivateer');

        // const deactivateButton = screen.getByTestId('deactivate-button');
        expect(deactivateButton).toBeInTheDocument();

        // Simuleer deactiveren van de cliënt
        fireEvent.click(deactivateButton);

        const confirmPopup = screen.getByTestId('confirm-popup');
        expect(confirmPopup).toBeInTheDocument();

        const isConfirmPopupOneButtonOpen = confirmPopup.getAttribute('data-isOpen');
        expect(confirmPopup.textContent).toBe('Cliënt succesvol gedeactiveerd');
        expect(isConfirmPopupOneButtonOpen).toBe('true');
        expect(confirmPopup.textContent).toMatchSnapshot();
    });

    test.skip('Deactivate_ClickDeactivateButton_ShowsConfirm', async () => {        

        const mockClient = {
            id: 1,
            firstname: 'John',
            initials: 'J.H',
            gender: 'Female',
            lastname: 'Doe',
            streetname: 'Main St',
            housenumber: '123',
            postalcode: '12345',
            residence: 'City',
            telephonenumber: '1234567890',
            emailaddress: 'john.doe@example.com',
            isintargetgroupregister: false,
            dateofbirth: new Date('1990-01-01'),
            emergencypeople: [],
            diagnoses: 'None',
            maritalstatus: 'Single',
            driverslicences: 'B',
            benefitform: 'None',
            workingcontracts: [],
            remarks: 'No remarks'
        };

        const mockFetchClient = api.fetchClient as jest.MockedFunction<typeof api.fetchClient>;
        mockFetchClient.mockResolvedValue(mockClient); //TODO: kijken of dit via een mock api klasse kan

        const clientId = '1';
        const route = `/clients/${clientId}`;

        await render(
            <MemoryRouter initialEntries={[route]}>
                <Routes>
                    <Route path="/clients/:id" element={<Clients />} />
                </Routes>
            </MemoryRouter>
        );        

        await waitFor(() => expect(api.fetchClient).toHaveBeenCalledWith(clientId));        

        screen.debug();

        await waitFor(() => {
            screen.getByTestId('deactivate-button');


            const spinner = screen.queryByTestId('clients-spinner');
            expect(spinner).not.toBeInTheDocument();
        }, { timeout: 10000 });

        screen.debug(undefined, Infinity, { highlight: false });

        // await waitFor(() => {
        //     expect(screen.getByText(mockClient.firstname)).toBeInTheDocument();
        //     expect(mockFetchClient).toHaveBeenCalledWith(clientId);
        //     const deactivateButton = screen.getByText('Deactivateer');
        //     expect(deactivateButton).toBeInTheDocument();      
        //   });

        const deactivateButton = screen.getByText('Deactivateer');

        // const deactivateButton = screen.getByTestId('deactivate-button');
        expect(deactivateButton).toBeInTheDocument();

        // Simuleer deactiveren van de cliënt
        fireEvent.click(deactivateButton);

        const confirmPopup = screen.getByTestId('confirm-popup');
        expect(confirmPopup).toBeInTheDocument();

        const isConfirmPopupOneButtonOpen = confirmPopup.getAttribute('data-isOpen');
        expect(confirmPopup.textContent).toBe('Cliënt succesvol gedeactiveerd');
        expect(isConfirmPopupOneButtonOpen).toBe('true');
        expect(confirmPopup.textContent).toMatchSnapshot();
    });

    test.skip('SearchClients_FillInSearchTerm_ShowsClients', async () => {        

        jest.setTimeout(50000); // Increase timeout to 10 seconds

        const mockClients = [
            {
                id: 1,
                firstname: 'John',
                initials: 'J.H',
                gender: 'Female',
                lastname: 'Doe',
                streetname: 'Main St',
                housenumber: '123',
                postalcode: '12345',
                residence: 'City',
                telephonenumber: '1234567890',
                emailaddress: 'john.doe@example.com',
                isintargetgroupregister: false,
                dateofbirth: new Date('1990-01-01'),
                emergencypeople: [
                ],
                diagnoses: 'None',
                maritalstatus: 'Single',
                driverslicences: 'B',
                benefitform: 'None',
                workingcontracts: [                    
                ],
                remarks: 'No remarks'
            },
            {
                id: 2,
                firstname: 'Jane',
                initials: 'J.E',
                gender: 'Female',
                lastname: 'Smith',
                streetname: 'Second St',
                housenumber: '456',
                postalcode: '67890',
                residence: 'Townsville',
                telephonenumber: '0987654321',
                emailaddress: 'jane.smith@example.com',
                isintargetgroupregister: false,
                dateofbirth: new Date('1985-05-15'),
                emergencypeople: [
                ],
                diagnoses: 'Diabetes',
                maritalstatus: 'Married',
                driverslicences: 'B, C',
                benefitform: 'Social Security',
                workingcontracts: [
                ],
                remarks: 'Requires insulin'
            },
            {
                id: 3,
                firstname: 'Michael',
                initials: 'M.T',
                gender: 'Male',
                lastname: 'Johnson',
                streetname: 'Third Ave',
                housenumber: '789',
                postalcode: '11223',
                residence: 'Village',
                telephonenumber: '1231231234',
                emailaddress: 'michael.johnson@example.com',
                isintargetgroupregister: false,
                dateofbirth: new Date('1975-03-20'),
                emergencypeople: [
                ],
                diagnoses: 'Hypertension',
                maritalstatus: 'Married',
                driverslicences: 'A, B',
                benefitform: 'Unemployment',
                workingcontracts: [
                ],
                remarks: 'Under medication'
            },
            {
                id: 4,
                firstname: 'Emily',
                initials: 'E.A',
                gender: 'Female',
                lastname: 'Davis',
                streetname: 'Fourth Blvd',
                housenumber: '321',
                postalcode: '44556',
                residence: 'Metropolis',
                telephonenumber: '3213214321',
                emailaddress: 'emily.davis@example.com',
                isintargetgroupregister: false,
                dateofbirth: new Date('1992-07-12'),
                emergencypeople: [
                ],
                diagnoses: 'Asthma',
                maritalstatus: 'Single',
                driverslicences: 'None',
                benefitform: 'Disability',
                workingcontracts: [
                ],
                remarks: 'Carries inhaler'
            },
            {
                id: 5,
                firstname: 'Robert',
                initials: 'R.K',
                gender: 'Male',
                lastname: 'Brown',
                streetname: 'Fifth St',
                housenumber: '654',
                postalcode: '77889',
                residence: 'Hamlet',
                telephonenumber: '4324325432',
                emailaddress: 'robert.brown@example.com',
                isintargetgroupregister: false,
                dateofbirth: new Date('1980-11-30'),
                emergencypeople: [
                ],
                diagnoses: 'None',
                maritalstatus: 'Divorced',
                driverslicences: 'B, D',
                benefitform: 'Retirement',
                workingcontracts: [
                ],
                remarks: 'No remarks'
            }
        ];
        
        const mockSearchClients = api.searchClients as jest.MockedFunction<typeof api.searchClients>;
        mockSearchClients.mockResolvedValue(mockClients); //TODO: kijken of dit via een mock api klasse kan

        const route = `/clients/`;

        await render(
            <MemoryRouter initialEntries={[route]}>
                <Routes>
                    <Route path="/clients/" element={<Clients />} />
                </Routes>
            </MemoryRouter>
        );      
        
        await waitFor(() => {
            screen.getByTestId('search-term');
        }, { timeout: 50000 });

        const searchTermInput = screen.getByTestId('search-term');

        fireEvent.change(searchTermInput, { target: { value: 'John Doe' } });

        await waitFor(() => {
            // Assuming the Clients component displays a list of clients based on the search term
            const clients = screen.getAllByTestId('results-item'); // Adjust the test id based on your implementation
            expect(clients.length).toBeGreaterThan(0); // Make sure at least one client is displayed
        });
    });
});
