import { ValidationErrorHash } from "./validation-error";

export default interface ApiResult <T> {
    Ok: boolean;
    Title?: string;
    Errors?: string[];
    ValidationErrors?: ValidationErrorHash;
    ReturnObject?: T;
}

export function getErrorMessage<T>(apiResult: ApiResult<T>) : string{
    try {
        var message = apiResult.Title ?? "";        
        message += "\n";

        if(apiResult.Errors && apiResult.Errors.length > 0) {
            message += apiResult.Errors!.join(', ');
        }

        return message;
    }
    catch (err) {
        return "";
    }
}