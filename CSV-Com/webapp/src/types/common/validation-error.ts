export interface ValidationErrorHash {
    [key: string]: ValidationError[];
}

export interface ValidationError {
    propertyname: string;
    errormessage: string;
}

export const parseValidationErrors = (json: any): ValidationErrorHash => {
    const validationErrorHash: ValidationErrorHash = {};

    if (json.errors) {
        Object.keys(json.errors).forEach((key: string) => {
            const field = key.toLocaleLowerCase();
            
            if (!validationErrorHash[field]) {
                validationErrorHash[field] = [];
            }

            json.errors[key].forEach((message: string) => {
                validationErrorHash[field].push({
                    propertyname: field,
                    errormessage: message
                });
            });
        });
    }

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

export const isHashEmpty = (hash: object): boolean => {
    return Object.keys(hash).length === 0;
};