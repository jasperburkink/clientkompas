// apiMock.ts
import Client from '../../../types/model/Client';

export const mockClientData: Client = {
    identificationnumber: 1,
    firstname: 'John',
    lastname: 'Smith',
    emailaddress: 'test@example.com',
    initials: 'J',
    gender: 'male',
    streetname: 'Dorpstraat',
    housenumber: '3',
    postalcode: '1234AB',
    residence: 'Amsterdam',
    telephonenumber: '123456789',
    maritalstatus: 'Getrouwd',
    dateofbirth: new Date(1960,1 ,1 ),
    driverslicences: 'A, B, BE'
};

export const fetchClient = async (clientId: string): Promise<Client> => {
  return Promise.resolve(mockClientData);
};