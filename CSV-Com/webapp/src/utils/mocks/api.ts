// apiMock.ts
import ClientQuery from 'types/model/ClientQuery';

export const mockClient: ClientQuery = {
    id: 1,
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

export const fetchClient = async (clientId: string): Promise<ClientQuery> => {
  console.log('API-mock wordt uitgevoerd');
  return Promise.resolve(mockClient);
};