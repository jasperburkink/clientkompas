import WorkingContract from "types/model/WorkingContract";

export interface RequiredFieldsConfig {
    [typeName: string]: {
        [fieldName: string]: boolean; // true betekent dat het veld optioneel is
    };
}


const RequiredFields: RequiredFieldsConfig = {
    emergencypeople: {
        name: true,
        telephonenumber: true
    },
    workingcontracts: {
        organizationid: true,
        function: true,
        contracttype: true,
        fromdate: false,
        todate: false
    }
};

export default RequiredFields;
