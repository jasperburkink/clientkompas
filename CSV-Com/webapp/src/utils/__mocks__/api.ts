import ClientQuery from 'types/model/ClientQuery';

// Mock client data
export const mockClient: ClientQuery = {
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
    dateofbirth: new Date('1990-01-01'),
    isintargetgroupregister: false,
    emergencypeople: [],
    diagnoses: 'None',
    maritalstatus: 'Single',
    driverslicences: 'B',
    benefitform: 'None',
    workingcontracts: [],
    remarks: 'No remarks'
};

export const fetchClient = jest.fn().mockResolvedValue(mockClient);
fetchClient.mockResolvedValue(mockClient);
export const fetchClientEditor = jest.fn().mockResolvedValue(mockClient);
export const searchClients = jest.fn();
export const fetchBenefitForms = jest.fn();
export const fetchDiagnosis = jest.fn();
export const fetchMaritalStatuses = jest.fn();
export const fetchDriversLicences = jest.fn();
export const deactivateClient = jest.fn().mockResolvedValue(mockClient);
export const saveClient = jest.fn();