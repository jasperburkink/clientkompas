export interface ValidationErrorHash {
    [key: string]: ValidationError[];
}

export interface ValidationError {
    propertyname: string;
    errormessage: string;
    errorcode: string;
}

export const parseValidationErrors = (json: any): ValidationErrorHash => {
    const validationErrors: ValidationError[] = json;

    const validationErrorHash: ValidationErrorHash = validationErrors.reduce((acc, error) => {
        const key = error.propertyname.toLocaleLowerCase();
        if (!acc[key]) {
            acc[key] = [];
        }
        acc[key].push(error);
        return acc;
    }, {} as ValidationErrorHash);

    return validationErrorHash;
};


export const filterValidationErrors = (errors: ValidationErrorHash, name: string): ValidationErrorHash => {
    const validationErrors: ValidationErrorHash = {};

    for (const key in errors) {
        if (key.toLowerCase().startsWith(name.toLowerCase())) {
            validationErrors[key] = errors[key];
        }
    }

    return validationErrors;
};